using FanDuel.DepthChart.Application.Interfaces.Repositories;
using FanDuel.DepthChart.Application.NFL;
using FanDuel.DepthChart.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FanDuel.DepthChart.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var app = CreateHostBuilder(args).Build();
            app.Services.GetRequiredService<NflDepthChartService>().Start();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                {
                    services.AddSingleton<NflDepthChartService>();
                    services.AddScoped<INflDepthChartManager, NflDepthChartManager>();
                    services.AddKeyedSingleton<IRepository, InMemoryRepository>("Local");
                    services.AddKeyedSingleton<IRepository, MongoRepository>("Mongo");
                });
    }
}
