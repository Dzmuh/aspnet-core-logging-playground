using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace aspnet_core_pages_serilog_on_startup.Pages
{
    public class FormLogerModel : PageModel
    {
        private readonly ILogger<FormLogerModel> _logger;

        public FormLogerModel(ILogger<FormLogerModel> logger)
        {
            this._logger = logger;
        }

        public void OnGet()
        {
            this._logger.LogInformation("[FormLoger] [OnGet] => BEGIN ...");
        }

        public void OnPost()
        {
            this._logger.LogInformation("[FormLoger] [OnPost] => BEGIN ...");
        }
    }
}