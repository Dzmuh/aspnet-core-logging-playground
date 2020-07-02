# Integrate JavaScript Logging with ASP.NET Core Logging APIs.

Интеграция логирования JavaScript с API логирования в ASP.NET Core.

ASP.NET Core предоставляет богатый API для ведения логирования и целую связку поставщиков средств ведения журналов, таких как `ConsoleLoggerPtovider`, `AzureAppServicesDiagnosticsLoggerProvider`, `EventLogLoggerProvider` и многих других.

Эти инструменты делают разработчиков на C# счастливыми, но факт таков, что разрабатывая веб-приложения приходится часто писать код на JavaScript, что не сложно, до тех пор пока не появляется задача найти исключения и ошибки на стороне клиента.

Конечно, есть много логеров на стороне клиента которые соответсвуют задачам отладки веб-приложения, но давате глянем на проблему под другим углом, а именно под углом непредвиденных ошибок, наших невинных и безобидных глупостей, в виде забытой фигурной скобки, нерабочей ссылки и т.д..

В этом проекте я попробую интегрировать журналы на стороне сервера и на стороне клиента, регистрирую все журналы у правайдера логирования ASP.NET Core, вместо того, чтобы управлять двумя разными поставщики логгеров для клиентской и серверной сторон.

В качестве руководства и наглядного пособия я буду использовать [статью Хишам Бен Аттиеха](http://hishambinateya.com/integrate-javascript-logging-with-asp.net-core-logging-apis) и его [проект jsLogger](https://github.com/hishamco/jsLogger).

## Логирование событий в JavaScript

Идея решения задачи достаточно простая: необходимо реализовать и внедрить API логирования на стороне клиента, после чего необходимо реализовать `JavaScriptLoggingMiddleware` для прослушивания логов отсылаемых скриптом и перенаправления их поставщикам средств ведения журнала ASP.NET Core.

На данный момент реализацию и внедрение API логирования на стороне клиента я опущу, воспользовавшись функционалом предоставляемым консолью JavaScript, этого функционала для демонстрации идеи более чем достаточно. В обоих случаях нужно перехватить журналы консоли следующим образом:

```js
(function () {
    var trace = console.trace;
    var debug = console.debug;
    var info = console.info;
    var warn = console.warn;
    var error = console.error;

    console.trace = function (message) {
        log(logLevel.Trace, message);
        trace.call(this, arguments);
    };

    console.debug = function (message) {
        log(logLevel.Debug, message);
        debug.call(this, arguments);
    };

    console.info = function (message) {
        log(logLevel.Information, message);
        info.call(this, arguments);
    };

    console.warn = function (message) {
        log(logLevel.Warning, message);
        warn.call(this, arguments);
    };

    console.error = function (message) {
        log(logLevel.Error, message);
        error.call(this, arguments);
    };
})();
```

Здесь, `log` - это метод, который я создал для публикации текущего лога на сервер, который будет обрабатываться с помощью `JavaScriptLoggingMiddleware`.

Хишам Бен Аттиех, в проекте jsLogger, сперва хотел сохранить скрипт jsLogger на диске и отображать с помощью `TagHelpers`, но вдохновившись Application Insights решил хранить его в файле resx и извлекать его с помощью `JavaScriptLoggingSnippet`.

```csharp
public class JavaScriptLoggingMiddleware
{
    private readonly ILogger _logger;
    private readonly RequestDelegate _next;

    public JavaScriptLoggingMiddleware(ILoggerFactory loggerFactory, RequestDelegate next)
    {
        _logger = loggerFactory.CreateLogger<JavaScriptLoggingMiddleware>();
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path == "/log" && context.Request.Method == "POST" && context.Request.HasFormContentType)
        {
            var form = await context.Request.ReadFormAsync();
            var level = Convert.ToInt32(form["level"].First());
            var message = form["message"].First();

            switch ((LogLevel)level)
            {
                case LogLevel.Trace:
                    _logger.LogTrace(message);
                    break;
                case LogLevel.Debug:
                    _logger.LogDebug(message);
                    break;
                case LogLevel.Information:
                    _logger.LogInformation(message);
                    break;
                case LogLevel.Warning:
                    _logger.LogWarning(message);
                    break;
                case LogLevel.Error:
                    _logger.LogError(message);
                    break;
                default:
                    return;
            }
        }
        else
        {
            await _next(context);
        }
    }
}
```

В коде выше вы видете что `JavaScriptLoggingMiddleware` прослушивает определённый URL и когда я обращаюсь к нему, обработчик сопостовляет уровень журналирования на сервере и на клиенте, после чего производит регистрацию поступившего события.

## Источники

* [Hisham's Blog - Integrate JavaScript Logging with ASP.NET Core Logging APIs](http://hishambinateya.com/integrate-javascript-logging-with-asp.net-core-logging-apis)
* [jsLogger - Github](https://github.com/hishamco/jsLogger)
