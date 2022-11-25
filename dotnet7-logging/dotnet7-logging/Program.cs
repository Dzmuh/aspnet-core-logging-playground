using Microsoft.AspNetCore.Hosting;

namespace dotnet7_logging
{
    public class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                var host = CreateHostBuilder(args).Build();
                var logger = host.Services.GetRequiredService<ILogger<Program>>();

                logger.LogInformation("Application has started");
                host.Run();

                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        // Levels of Log
        /*
        - Trace: Lowest level, for heavy debugging. very detailed 
        - Debug: Detailed but not as low as Trace
        - Information: More friendly way to see the flow of the application
        - Warning: Exception like issues
        - Error: Unhandled things like crash and things we cannot fix
        - Critical: Only most critical level logs

        - Customize in appsettings.json to show
        */
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                // ILogger Configuration
                .ConfigureLogging((context, logging) =>
                {
                    logging.ClearProviders();
                    logging.AddConfiguration(context.Configuration.GetSection("Logging")); // Gets Configuration from appsettings.json
                    logging.AddDebug();
                    logging.AddConsole(); // EventSource, EventLog, TraceSource, AzureAppServicesFile, AzureAppServicesBlob
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}