# Структурное логирование в ASP.NET Core проектах

Необходимые библиотеки

* Serilog для ASP.NET Core: [Serilog.AspNetCore](https://github.com/serilog/serilog-aspnetcore) ([NuGet Gallery](https://www.nuget.org/packages/Serilog.AspNetCore/));
* Для записи в консоль понадобится: [Serilog.Sinks.Console](https://github.com/serilog/serilog-sinks-console) ([NuGet Gallery](https://www.nuget.org/packages/Serilog.Sinks.Console/));
* Для удобства настройки понадобится: [Serilog.Settings.Configuration](https://github.com/serilog/serilog-settings-configuration) ([NuGet Gallery](https://www.nuget.org/packages/Serilog.Settings.Configuration/)) - он позволят конфигурировать Serilog в файле `appsettings.json` вместо кода в `Program.cs`;

## Включаю структурное логирование

В `appsettings.json` добавляю:

```json
"Serilog":{
  "MinimumLevel": {
    "Default": "Information"
  },
  "WriteTo":[
    {
      "Name": "Console"
    }
  ]
}
```

Модифицирую `Program.cs`. Было:

```csharp
public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
```

Стало:

```csharp

```