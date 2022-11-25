using Serilog;
using Serilog.Events;

namespace aspnet_core_serilog_details
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                             .WriteTo.Console()
                             .CreateBootstrapLogger();

            Log.Information("Starting up");

            try
            {
                var builder = WebApplication.CreateBuilder(args);

                const string outputTemplate = "[{Timestamp:HH:mm:ss.fff} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}";

                builder.Host.UseSerilog((context, services, configuration) => configuration
                            .MinimumLevel.Information()
                            .MinimumLevel.Override("System", LogEventLevel.Error)
                            .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                            .Enrich.FromLogContext()
                            .WriteTo.Console(outputTemplate: outputTemplate));

                var app = builder.Build();

                app.MapGet("/", () => "Hello World!");

                app.Run();

                Log.Information("Stopped cleanly");
                return 1;
            }
            catch (Exception ex)
            {

                Log.Fatal(ex, "An unhandled exception occured during bootstrapping");
                return 0;
            }        
            finally
            {
                Log.Information("Shut down complete");
                Log.CloseAndFlush();
            }            
        }
    }
}