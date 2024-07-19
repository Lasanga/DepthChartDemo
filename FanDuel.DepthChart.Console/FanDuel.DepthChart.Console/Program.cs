using FanDuel.DepthChart.Application.Interfaces.Repositories;
using FanDuel.DepthChart.Application.NFL;
using FanDuel.DepthChart.Infrastructure.Persistence;
using FanDuel.DepthChart.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FanDuel.DepthChart.ConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var app = CreateHostBuilder(args).Build();
            await app.Services.GetRequiredService<NflDepthChartService>().Start();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices((_, services) =>
            {
                services.AddSingleton<NflDepthChartService>();
                services.AddScoped<INflDepthChartManager, NflDepthChartManager>();
                services.AddKeyedSingleton<IRepository, InMemoryRepository>("Local");
                services.AddKeyedScoped<IRepository, EfInMemoryRepository>("Ef");
                services.AddDbContext<DepthChartDbContext>(options => options.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            });
    }
}
