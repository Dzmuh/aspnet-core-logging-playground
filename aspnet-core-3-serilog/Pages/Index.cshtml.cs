using System;
using System.Threading;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Serilog;

namespace aspnet_core_3_serilog.Pages
{
    public class IndexPageViewModel : PageModel
    {
        private readonly ILogger<IndexPageViewModel> _logger;

        public IndexPageViewModel(ILogger<IndexPageViewModel> logger)
        {
            this._logger = logger;
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
            Log.Verbose("[Index] [OnGet]: Serilog VERBOSE message");
            Log.Debug("[Index] [OnGet]: Serilog DEBUG message");
            Log.Information("[Index] [OnGet]: Serilog INFO message");
            Log.Warning("[Index] [OnGet]: Serilog WARN message");
            Log.Error("[Index] [OnGet]: Serilog ERROR message");
            Log.Fatal("[Index] [OnGet]: Serilog FATAL message");
        }
    }
}
