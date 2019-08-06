# Serilog в AspNetCore приложении на базе RazorPages с встроенной активацией по официальной рекомендации

Serilog имеет методы встроенной активации через `BulidWebHost()` и вы можете конфигурировать Serilog используя делегаты, как показано ниже:

```csharp
// dotnet add package Serilog.Settings.Configuration
.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
    .ReadFrom.Configuration(hostingContext.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console())
```

## Инструкция

**Сперва** установим необходимые пакеты:

```bash
dotnet add package Serilog.AspNetCore
dotnet add package Serilog.Sinks.Console
dotnet add package Serilog.Settings.Configuration
```

**Далее** отредактируем файл `Program.cs` расширив `WebHostBuilder` делегатами `Serilog`, например:

```csharp
public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
    WebHost.CreateDefaultBuilder(args)
        .UseStartup<Startup>()
        .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
            .ReadFrom.Configuration(hostingContext.Configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console());
```

## Запись журнала событий

В этом демонстрационном проекте я не использую DI и для отправки лога поставщикам ведения журнала в этой ситуации следует использовать статичный класс `Log` или методы `ILogger`.
Например, используя статичный класс `Log`:

```csharp
Log.Verbose("Serilog VERBOSE message");
Log.Debug("Serilog DEBUG message");
Log.Information("Serilog INFO message");
Log.Warning("Serilog WARN message");
Log.Error("Serilog ERROR message");
Log.Fatal("Serilog FATAL message");
```

Используя методы `ILogger`: 

```csharp
private readonly ILogger<IndexPageViewModel> _logger;

public IndexPageViewModel(ILogger<IndexPageViewModel> logger)
{
    this._logger = logger;
}

public void OnGet()
{
    this._logger.LogTrace("DoSomething TRACE message");
    this._logger.LogDebug("DoSomething DEBUG message");
    this._logger.LogInformation("DoSometing INFO message");
    this._logger.LogWarning("DoSometing WARN message");
    this._logger.LogError("DoSometing ERROR message");
    this._logger.LogCritical("DoSometing CRITICAL message");
}
```

Обращу внимание на то, что использование `ILogger` обеспечивает переносимость кода.

## Ссылки и источники

* [Serilog.AspNetCore - Github](https://github.com/serilog/serilog-aspnetcore)
* [Serilog.AspNetCore - NuGet Gallery](https://www.nuget.org/packages/Serilog.AspNetCore)
* [Writing Log Events](https://github.com/serilog/serilog/wiki/Writing-Log-Events)
