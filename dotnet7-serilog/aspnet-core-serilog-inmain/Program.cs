using Serilog;

namespace aspnet_core_serilog_inmain
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

                builder.Host.UseSerilog((context, services, configuration) => configuration
                            .WriteTo.Console()
                            .ReadFrom.Configuration(context.Configuration)
                            .ReadFrom.Services(services));

                var app = builder.Build();

                app.UseSerilogRequestLogging();

                app.MapGet("/", () => "Hello World!");
                app.MapGet("/oops", new Func<string>(() => throw new InvalidOperationException("Oops!")));

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