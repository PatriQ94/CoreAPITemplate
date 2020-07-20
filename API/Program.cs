using System.Threading.Tasks;
using DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //Setup Serilog configuration
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();

            try
            {
                var host = CreateHostBuilder(args).Build();

                //Run EFCore migrations
                using (var serviceScope = host.Services.CreateScope())
                {
                    var dbContext = serviceScope.ServiceProvider.GetRequiredService<DataContext>();
                    //await dbContext.Database.MigrateAsync();
                }

                Log.Information("Application starting up");

                //Run app
                await host.RunAsync();
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, "The application failed to start correctly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
