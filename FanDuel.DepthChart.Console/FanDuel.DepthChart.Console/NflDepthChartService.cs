using FanDuel.DepthChart.Application.NFL;
using FanDuel.DepthChart.Contracts;
using FanDuel.DepthChart.Domain.Types;
using Microsoft.Extensions.Logging;

namespace FanDuel.DepthChart.ConsoleApp
{
    public class NflDepthChartService(
        ILogger<NflDepthChartService> _logger,
        INflDepthChartManager _nflDepthChartManager)
    {
        public void Start()
        {
            _logger.LogInformation("Starting Nfl DepthChart Service");

            var tomBrady = new PlayerDto { Name = "Tom Brady", Number = 12 };
            var blaineGabbert = new PlayerDto { Name = "Blaine Gabbert", Number = 11 };
            var kyleTrask = new PlayerDto { Name = "Kyle Trask", Number = 2 };
            var mikeEvans = new PlayerDto { Name = "Mike Evans", Number = 13 };
            var jaelonDarden = new PlayerDto { Name = "Jaelon Darden", Number = 1 };
            var scottMiller = new PlayerDto { Name = "Scott Miller", Number = 10 };

            _nflDepthChartManager.AddPlayerToDepthChart(NflPositionTypes.QB.ToString(), tomBrady, 0);
            _nflDepthChartManager.AddPlayerToDepthChart(NflPositionTypes.QB.ToString(), blaineGabbert, 1);
            _nflDepthChartManager.AddPlayerToDepthChart(NflPositionTypes.QB.ToString(), kyleTrask, 2);

            _nflDepthChartManager.AddPlayerToDepthChart(NflPositionTypes.LWR.ToString(), mikeEvans, 0);
            _nflDepthChartManager.AddPlayerToDepthChart(NflPositionTypes.LWR.ToString(), jaelonDarden, 1);
            _nflDepthChartManager.AddPlayerToDepthChart(NflPositionTypes.LWR.ToString(), scottMiller, 2);

            _logger.LogInformation("Print Backups");
            PrintPlayers(() => _nflDepthChartManager.GetBackups(NflPositionTypes.QB.ToString(), tomBrady));
            PrintPlayers(() => _nflDepthChartManager.GetBackups(NflPositionTypes.LWR.ToString(), jaelonDarden));
            PrintPlayers(() => _nflDepthChartManager.GetBackups(NflPositionTypes.QB.ToString(), mikeEvans));
            PrintPlayers(() => _nflDepthChartManager.GetBackups(NflPositionTypes.QB.ToString(), blaineGabbert));
            PrintPlayers(() => _nflDepthChartManager.GetBackups(NflPositionTypes.QB.ToString(), kyleTrask));

            _logger.LogInformation("Print full DepthChart");
            PrintFullDepthChart(_nflDepthChartManager.GetFullDepthChart);

            _logger.LogInformation("Removed Player");
            PrintPlayers(() => _nflDepthChartManager.RemovePlayerFromDepthChart(NflPositionTypes.LWR.ToString(), mikeEvans));

            _logger.LogInformation("Print full DepthChart");
            PrintFullDepthChart(_nflDepthChartManager.GetFullDepthChart);

            _logger.LogInformation("End of Nfl DepthChart Service");
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
