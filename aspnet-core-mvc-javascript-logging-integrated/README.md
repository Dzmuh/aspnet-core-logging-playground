# Integrate JavaScript Logging with ASP.NET Core Logging APIs.

Интеграция логирования JavaScript с API логирования в ASP.NET Core.

ASP.NET Core предоставляет богатый API для ведения логирования и целую связку поставщиков средств ведения журналов, таких как `ConsoleLoggerPtovider`, `AzureAppServicesDiagnosticsLoggerProvider`, `EventLogLoggerProvider` и многих других.

Эти инструменты делают разработчиков на C# счастливыми, но факт таков, что разрабатывая веб-приложения приходится часто писать код на JavaScript, что не сложно, до тех пор пока не появляется задача найти исключения и ошибки на стороне клиента.

Конечно, есть много логеров на стороне клиента которые соответсвуют задачам отладки веб-приложения, но давате глянем на проблему под другим углом, а именно под углом непредвиденных ошибок, наших невинных и безобидных глупостей, в виде забытой фигурной скобки, нерабочей ссылки и т.д..

В этом проекте я попробую интегрировать журналы на стороне сервера и на стороне клиента, регистрирую все журналы у правайдера логирования ASP.NET Core, вместо того, чтобы управлять двумя разными поставщики логгеров для клиентской и серверной сторон.

В качестве руководства и наглядного пособия я буду использовать [статью Хишам Бен Аттиеха](http://hishambinateya.com/integrate-javascript-logging-with-asp.net-core-logging-apis) и его [проект jsLogger](https://github.com/hishamco/jsLogger).

## Источники

* [Hisham's Blog - Integrate JavaScript Logging with ASP.NET Core Logging APIs](http://hishambinateya.com/integrate-javascript-logging-with-asp.net-core-logging-apis)
* [jsLogger - Github](https://github.com/hishamco/jsLogger)
