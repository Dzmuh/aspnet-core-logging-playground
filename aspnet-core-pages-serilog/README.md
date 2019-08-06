# Serilog в AspNetCore приложении на базе RazorPages с активацией по официальной рекомендации

## Инструкция

**Сперва** установим необходимые пакеты: 

```bash
dotnet add package Serilog.AspNetCore
dotnet add package Serilog.Sinks.Console
```

**Далее** отредактируем файл `Program.cs` для конфигурирования `Serilog`.
Использования блока `try`/`catch` гарантирует что проблемы конфигурации будут соответствующим образом залогированы:

```csharp
using Serilog;

public class Program
{
    public static int Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        try
        {
            Log.Information("Starting web host");
            CreateWebHostBuilder(args).Build().Run();
            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
```

**Потом** добавим `UseSerilog()` в `BuildWebHost()`

```csharp
public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
    WebHost.CreateDefaultBuilder(args)
        .UseStartup<Startup>()
        .UseSerilog();
```

**Завершим** на том, что очистим и удалим оставшуюся конфигурацию для логера по умолчанию:

* Удалим вызовы `AddLogging()`
* Удалим секцию `Logging` в файлах `appsettings.json` (его можно заменить конфигурацией Serilog, если это требуется)
* Удаляем параметры `ILoggerFactory ` и все вызовы `Add*()` связанные с фабрикой логирования в файле `Startup.cs`
* Удаляем `UseApplicationInsights()` (это может быть заменено пакетом [Serilog.Sinks.ApplicationInsights](https://github.com/serilog/serilog-sinks-applicationinsights), если это необходимо)

## Запись журнала событий

В этом демонстрационном проекте я не использую DI и для отправки лога поставщикам ведения журнала в этой ситуации следует использовать статичный класс `Log` или методы `ILogger`.
Например:

```csharp
Log.Verbose("Serilog VERBOSE message");
Log.Debug("Serilog DEBUG message");
Log.Information("Serilog INFO message");
Log.Warning("Serilog WARN message");
Log.Error("Serilog ERROR message");
Log.Fatal("Serilog FATAL message");
```

## Ссылки и источники

* [Serilog.AspNetCore - Github](https://github.com/serilog/serilog-aspnetcore)
* [Serilog.AspNetCore - NuGet Gallery](https://www.nuget.org/packages/Serilog.AspNetCore)
* [Writing Log Events](https://github.com/serilog/serilog/wiki/Writing-Log-Events)
