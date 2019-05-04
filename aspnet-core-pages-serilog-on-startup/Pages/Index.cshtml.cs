using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
            this._logger.LogInformation("Index/OnGet (LogInformation)");
            this._logger.LogDebug("Index/OnGet (LogDebug)");
            this._logger.LogWarning("Index/OnGet (LogWarning)");
            this._logger.LogError("Index/OnGet (LogError)");

            this._seriLogger.Verbose("Index/OnGet (Serilog.Verbose)");
            this._seriLogger.Error("Index/OnGet (Serilog.Error)");
        }
    }
}
