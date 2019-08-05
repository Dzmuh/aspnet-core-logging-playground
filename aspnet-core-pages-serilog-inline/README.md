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
