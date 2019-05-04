# Serilog в AspNetCore приложении на базе RazorPages с активацией в Startup

1. Регестрируем логер в Startup.cs, метод ConfigureServices:
   ```csharp
   services.AddSingleton<Serilog.ILogger>(options =>
            {
                return new LoggerConfiguration().CreateLogger();
            });
   ```
2. Активируем и конфигурируем логер в Startup.cs, метод Configure:
   ```csharp
   loggerFactory.AddFile("log-{Date}.txt");
   ```

Это минимум.

Дальше мы можем сконфигурировать логер и подробности которые пишутся в лог. Например:

```csharp
services.AddSingleton<Serilog.ILogger>(options =>
{
    return new LoggerConfiguration()
        .Enrich.FromLogContext()
        .MinimumLevel.Verbose()
        .WriteTo.Console()
        .CreateLogger();
});
```

## Ссылки

* [Serilog](https://serilog.net/) ([Github](https://github.com/serilog/serilog))
* [Serilog.Extensions.Logging.File - NuGet Gallery](https://www.nuget.org/packages/Serilog.Extensions.Logging.File/) ([Github](https://github.com/serilog/serilog-extensions-logging-file))
