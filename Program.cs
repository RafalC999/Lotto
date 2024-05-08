using LottoScaper.DAL.Services.Implementation;
using LottoScaper.DAL.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace LottoScaper.DAL
{
    public class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
            Log.Logger.Information("Application starting...");

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddScoped<IWebScrapingService, WebScrapingService>();
                    services.AddScoped<IConsoleMenu, ConsoleMenu>();
                    services.AddScoped<IUserActionsService, UserActionsService>();
                    services.AddScoped<IStatisticsService, StatisticsService>();
                })
                .UseSerilog()
                .Build();
            var menu = ActivatorUtilities.CreateInstance<ConsoleMenu>(host.Services);
            menu.ShowMenu();

            var web = ActivatorUtilities.CreateInstance<WebScrapingService>(host.Services);
            var document = web.GetDocument("https://megalotto.pl/wyniki/lotto/30-ostatnich-losowan");
            web.ScrapLottoData(document);


        }
        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIROMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables();
        }

    }
}