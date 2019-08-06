using System.Threading;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;

namespace aspnet_core_pages_serilog_inline.Pages
{
    public class IndexPageViewModel : PageModel
    {
        public void OnGet()
        {
            // Simultate something me do for 1 second
            Thread.Sleep(1000);

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
