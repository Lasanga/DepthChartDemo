using FanDuel.DepthChart.Application.Interfaces.Repositories;
using FanDuel.DepthChart.Application.NFL;
using FanDuel.DepthChart.Contracts;
using FanDuel.DepthChart.Domain.Types;
using FanDuel.DepthChart.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace FanDuel.DepthChart.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var services = new ServiceCollection();
                services.AddScoped<INflDepthChartManager, NflDepthChartManager>();
                services.AddKeyedSingleton<IRepository, InMemoryRepository>("Local");
                services.AddKeyedSingleton<IRepository, MongoRepository>("Mongo");

                var serviceProvder = services.BuildServiceProvider();

                using var scope = serviceProvder.CreateScope();
                var nflDepthChartManager = scope.ServiceProvider.GetRequiredService<INflDepthChartManager>();

                var tomBrady = new PlayerDto { Name = "Tom Brady", Number = 12 };
                var blaineGabbert = new PlayerDto { Name = "Blaine Gabbert", Number = 11 };
                var kyleTrask = new PlayerDto { Name = "Kyle Trask", Number = 2 };
                var mikeEvans = new PlayerDto { Name = "Mike Evans", Number = 13 };
                var jaelonDarden = new PlayerDto { Name = "Jaelon Darden", Number = 1 };
                var scottMiller = new PlayerDto { Name = "Scott Miller", Number = 10 };

                nflDepthChartManager.AddPlayerToDepthChart(NflPositionTypes.QB.ToString(), tomBrady, 0);
                nflDepthChartManager.AddPlayerToDepthChart(NflPositionTypes.QB.ToString(), blaineGabbert, 1);
                nflDepthChartManager.AddPlayerToDepthChart(NflPositionTypes.QB.ToString(), kyleTrask, 2);

                nflDepthChartManager.AddPlayerToDepthChart(NflPositionTypes.LWR.ToString(), mikeEvans, 0);
                nflDepthChartManager.AddPlayerToDepthChart(NflPositionTypes.LWR.ToString(), jaelonDarden, 1);
                nflDepthChartManager.AddPlayerToDepthChart(NflPositionTypes.LWR.ToString(), scottMiller, 2);

                Console.WriteLine("Print Backups");
                PrintPlayers(() => nflDepthChartManager.GetBackups(NflPositionTypes.QB.ToString(), tomBrady));
                PrintPlayers(() => nflDepthChartManager.GetBackups(NflPositionTypes.LWR.ToString(), jaelonDarden));
                PrintPlayers(() => nflDepthChartManager.GetBackups(NflPositionTypes.QB.ToString(), mikeEvans));
                PrintPlayers(() => nflDepthChartManager.GetBackups(NflPositionTypes.QB.ToString(), blaineGabbert));
                PrintPlayers(() => nflDepthChartManager.GetBackups(NflPositionTypes.QB.ToString(), kyleTrask));

                Console.WriteLine("Print full DepthChart");
                PrintFullDepthChart(nflDepthChartManager.GetFullDepthChart);

                Console.WriteLine("\nRemoved Player");
                PrintPlayers(() => nflDepthChartManager.RemovePlayerFromDepthChart(NflPositionTypes.LWR.ToString(), mikeEvans));

                Console.WriteLine("Print full DepthChart");
                PrintFullDepthChart(nflDepthChartManager.GetFullDepthChart);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("\nPress any key to close the application...");
            }

            Console.ReadLine();
        }

        private static void PrintPlayers(Func<List<PlayerDto>> action)
        {
            var players = action();
            if (!players.Any())
            {
                Console.WriteLine($"<NO LIST>\n");
                return;
            }

            foreach (var player in players)
            {
                Console.WriteLine($"#{player.Number} - {player.Name}");
            }

            Console.WriteLine();
        }

        private static void PrintFullDepthChart(Func<Dictionary<string, List<DepthChartEntryDto>>> action)
        {
            var depthChart = action();
            foreach (var chart in depthChart)
            {
                var displayText = $"{chart.Key} - ";
                displayText += String.Join(", ", chart.Value.Select(x => $"(#{x?.Player?.Number}, {x?.Player?.Name})"));

                Console.WriteLine(displayText);
            }
        }
    }
}
