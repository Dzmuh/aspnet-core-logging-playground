# Serilog и .NET 7

Этот проект демонстрирует использование Serilog в базовом шаблоне веб-приложени .NET 7. Конфигурирование логирования выполняется через файл конфигурации.

Проект демонстрирует: 

* Поддержку .NET 7 `ILogger<T>` и `WebApplicationBuilder`.
* Уровень ведения журнала, зависящий от пространства имен, для подавления шума от фреймворка.
* Конфигурацию через JSON. Смотрите файлы `appsettings.json` и `appsettings.Development.json`.
* Чистый, тематический вывод консоли.
* Локальное ведение журнала в файл со скользящим графиком ведения журнала, напримр можно хранить журнал за 1 день.
* Упрощенное ведение журнала HTTP-запросов.
* Фильтрация излишних событий с помощью _Serilog.Expressions_.
* Регистрация исключений.
* Корректная очистка логов при выходе.

Код в проекте не комментируется, чтобы его структуру было легче сравнивать со структурой базовых шаблонов. Конкретные решения прокомментированы ниже.

## Пробуем это

Для запуска примера потребуется пакет .NET 7.0 SDK или выше. Проверим версию:

```shell
dotnet --version
```

После проверки выполним в каталоге проекта:

```shell
dotnet run
```

Некоторые URL-адреса будут отображены в терминале: откройте их в браузере, чтобы увидеть логирование запросов в действии.

* `/` &mdash; должен показать "Hello, world!" вместе с ответом успеха
* `/oops` &mdash; выбрасывает исключение, которое будет зарегистрировано

## Делаем это с нуля

### 1. Создаём проект используя шаблон `web`

```shell
mkdir myproject
cd myproject
dotnet new web
```

### 2. Устанавливаю пакеты Serilog

```shell
dotnet add package serilog
dotnet add package serilog.aspnetcore
```

### 3. Инициализирую Serilog при старте в `Program.cs`

Важно, чтобы ведение журнала было инициализировано как можно раньше, чтобы в журнал записывались ошибки, которые могут помешать запуску приложения.

В самом верху `Program.cs`:

```csharp
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");
```

Обратим внимание на то, что я использую метод внедрения логера отличный от методов в инструкции к пакету `Serilog.AspNetCore`. 
`CreateBootstrapLogger()` настраивает Serilog в минимальной конфигурации, запись ведётся только в консоль, и настроить логер можно будет позже, в процессе инициализации, как только будет доступна инфраструктура веб-хостинга.

### 4. Оберну оставшуюся часть `Program.cs` в блоки `try`/`catch`/`finally`

Конфигурацию веб-приложения в `Program.cs` может быть заключена в блок `try`. Более того, результат метода `Main` я меняю с `void` на `int`.

```csharp
public static int Main(string[] args)
{
    try
    {
        // <snip>
        return 1;
    }
    catch (Exception ex)
    {
        Log.Fatal(ex, "Unhandled exception");
        return 0;
    }
    finally
    {
        Log.Information("Shut down complete");
        Log.CloseAndFlush();
    }
}
```

Код возврата добавляет удобство при отслеживании развёртывании приложения.

А блок `catch` регистрирует любые исключения, возникшие во время запуска.

Метод `Log.CloseAndFlush()` в блоке `finally` гарантирует, что любые события журнала в очереди будут правильно записаны при выходе из программы и я кое-как смогу понять - где искать источник проблемы.

### 5. Подключаю Serilog к `WebApplicationBuilder`

Минимальный вариант:
```csharp
    builder.Host.UseSerilog((context, configuration) => configuration
        .WriteTo.Console()
        .ReadFrom.Configuration(context.Configuration));
```

Я по старинке делаю так:
```csharp
    builder.Host.UseSerilog((context, services, configuration) => configuration
           .WriteTo.Console()
           .ReadFrom.Configuration(context.Configuration)
           .ReadFrom.Services(services));
```

### 6. Заменяю конфигурацию логированния в `appsettings.json`

В файле `appsettings.json` удаляю секцию `"Logging"` и добавляю `"Serilog"`.

Полная конфигурация Serilog в JSON может выглядеть так:

```json
{
    "Serilog": {
    "MinimumLevel": {
        "Default": "Information",
        "Override": {
            "Microsoft": "Warning",
            "Microsoft.Hosting.Lifetime": "Information"
        }
    },
    "Filter": [
        {
        "Name": "ByExcluding",
        "Args": {
            "expression": "@mt = 'An unhandled exception has occurred while executing the request.'"
            }
        }
    ],
    "WriteTo": [
        {
        "Name": "File",
        "Args": { "path":  "./logs/log-.log", "rollingInterval": "Day" }
        }
    ]
    },
    "AllowedHosts": "*"
}
```

Из `appsettings.Development.json` секцию `"Logging"` можно удалить целиком.
Аргумент за: во время разработки обычно стоит использовать тот же уровень ведения журнала, который используется в рабочей среде, так как если мы не способны найти проблемы, используя рабочие журналы в процессе разработки, найти проблемы в реальной производственной среде будет еще труднее.

### 7. Добавляю связующие механизмы Serilog для логирования запросов

По умолчанию платформа ASP.NET Core регистрирует несколько событий информационного уровня для каждого запроса.

Логирование запросов через Serilog упрощает это до единого сообщения на каждый запрос, включая путь, метод, время, код состояния и исключение.

```csharp
    app.UseSerilogRequestLogging();
```

## Пишем события журнала

Эти настройки позволяют использовать Serilog как глобальный логер через статический класс `Log`, как в примерах выше, так и задействовать абстракцию _Microsoft.Extensions.Logging_'s
`ILogger<T>`, что позволит использовать Serilog через методы внедрения зависимостей в контроллеры и другие компоненты.
