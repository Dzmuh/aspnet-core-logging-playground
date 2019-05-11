using System.Threading;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace aspnet_core_pages_serilog_on_startup.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly Serilog.ILogger _seriLogger;

        public IndexModel(
            ILogger<IndexModel> logger,
            Serilog.ILogger seriLogger
            )
        {
            this._logger = logger;
            this._seriLogger = seriLogger;
        }

        public void OnGet()
        {
            // Simultate something me do for 1 second
            Thread.Sleep(1000);

            // Test logs at each level
            this._logger.LogTrace("[Index] [OnGet]: DoSomething TRACE message");
            this._logger.LogDebug("[Index] [OnGet]: DoSomething DEBUG message");
            this._logger.LogInformation("[Index] [OnGet]: DoSometing INFO message");
            this._logger.LogWarning("[Index] [OnGet]: DoSometing WARN message");
            this._logger.LogError("[Index] [OnGet]: DoSometing ERROR message");
            this._logger.LogCritical("[Index] [OnGet]: DoSometing CRITICAL message");

            // Test logs at each level for Serilog
            // https://github.com/serilog/serilog/wiki/Configuration-Basics
            this._seriLogger.Verbose("[Index] [OnGet]: Serilog VERBOSE message");
            this._seriLogger.Debug("[Index] [OnGet]: Serilog DEBUG message");
            this._seriLogger.Information("[Index] [OnGet]: Serilog INFO message");
            this._seriLogger.Warning("[Index] [OnGet]: Serilog WARN message");
            this._seriLogger.Error("[Index] [OnGet]: Serilog ERROR message");
            this._seriLogger.Fatal("[Index] [OnGet]: Serilog FATAL message");
        }
    }
}
