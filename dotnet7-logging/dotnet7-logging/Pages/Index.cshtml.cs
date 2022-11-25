using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace dotnet7_logging.Pages
{
    public class IndexModel : PageModel
    {
        //private readonly ILogger<IndexModel> _logger;
        private readonly ILogger _logger;

        // public IndexModel(ILogger<IndexModel> logger)
        public IndexModel(ILoggerFactory factory)
        {
            //_logger = logger;
            this._logger = factory.CreateLogger("CustomCategory");
        }

        public void OnGet()
        {
            _logger.LogTrace("TRACE.");
            _logger.LogDebug("DEBUG.");
            _logger.LogInformation("INFORMATION.");
            _logger.LogWarning("WARNING.");
            _logger.LogError("ERROR.");
            _logger.LogCritical("CRITICAL.");

            _logger.LogError("Server went down temporarily at {Time}", DateTime.UtcNow);

            try
            {
                throw new Exception("Unhandled");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "There was a bad exception at {Time}", DateTime.UtcNow);
            }
        }

        public class LoggingId
        {
            public const int DemoCode = 1001;
        }
    }
}