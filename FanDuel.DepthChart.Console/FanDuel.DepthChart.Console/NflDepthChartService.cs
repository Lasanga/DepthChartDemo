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
        public async Task Start()
        {
            try
            {
                _logger.LogInformation("Starting Nfl DepthChart Service");

                var sportId = await _nflDepthChartManager.GetSportByNameAsync("NFL");
                var team1 = await _nflDepthChartManager.CreateTeamAsync(new TeamDto { Name = "Tampa Bay Buccaneers", SportId = sportId }) ?? throw new InvalidOperationException("Team does not exist");

                var tomBrady = await _nflDepthChartManager.CreatePlayerAsync(new PlayerDto { Name = "Tom Brady", Number = 12, TeamId = team1.TeamId });
                var blaineGabbert = await _nflDepthChartManager.CreatePlayerAsync(new PlayerDto { Name = "Blaine Gabbert", Number = 11, TeamId = team1.TeamId });
                var kyleTrask = await _nflDepthChartManager.CreatePlayerAsync(new PlayerDto { Name = "Kyle Trask", Number = 2, TeamId = team1.TeamId });
                var mikeEvans = await _nflDepthChartManager.CreatePlayerAsync(new PlayerDto { Name = "Mike Evans", Number = 13, TeamId = team1.TeamId });
                var jaelonDarden = await _nflDepthChartManager.CreatePlayerAsync(new PlayerDto { Name = "Jaelon Darden", Number = 1, TeamId = team1.TeamId });
                var scottMiller = await _nflDepthChartManager.CreatePlayerAsync(new PlayerDto { Name = "Scott Miller", Number = 10, TeamId = team1.TeamId });

                await _nflDepthChartManager.AddPlayerToDepthChart(NflPositionTypes.QB.ToString(), tomBrady, 0);
                await _nflDepthChartManager.AddPlayerToDepthChart(NflPositionTypes.QB.ToString(), blaineGabbert, 1);
                await _nflDepthChartManager.AddPlayerToDepthChart(NflPositionTypes.QB.ToString(), kyleTrask, 2);

                await _nflDepthChartManager.AddPlayerToDepthChart(NflPositionTypes.LWR.ToString(), mikeEvans, 0);
                await _nflDepthChartManager.AddPlayerToDepthChart(NflPositionTypes.LWR.ToString(), jaelonDarden, 1);
                await _nflDepthChartManager.AddPlayerToDepthChart(NflPositionTypes.LWR.ToString(), scottMiller, 2);

                Console.WriteLine("Print Backups");
                await PrintPlayers(async () => await _nflDepthChartManager.GetBackups(NflPositionTypes.QB.ToString(), tomBrady));
                await PrintPlayers(() => _nflDepthChartManager.GetBackups(NflPositionTypes.LWR.ToString(), jaelonDarden));
                await PrintPlayers(() => _nflDepthChartManager.GetBackups(NflPositionTypes.QB.ToString(), mikeEvans));
                await PrintPlayers(() => _nflDepthChartManager.GetBackups(NflPositionTypes.QB.ToString(), blaineGabbert));
                await PrintPlayers(() => _nflDepthChartManager.GetBackups(NflPositionTypes.QB.ToString(), kyleTrask));

                Console.WriteLine("Print full DepthChart");
                await PrintFullDepthChart(async () => await _nflDepthChartManager.GetFullDepthChart());

                Console.WriteLine("\nRemoved Player");
                await PrintPlayers(async () => await _nflDepthChartManager.RemovePlayerFromDepthChart(NflPositionTypes.LWR.ToString(), mikeEvans));

                Console.WriteLine("Print full DepthChart");
                await PrintFullDepthChart(async () => await _nflDepthChartManager.GetFullDepthChart());

                _logger.LogInformation("End of Nfl DepthChart Service");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occured while execution: {ex.Message}");
                throw;
            }
        }

        private async Task PrintPlayers(Func<Task<List<PlayerDto>>> action)
        {
            var players = await action();
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

        private async Task PrintFullDepthChart(Func<Task<Dictionary<string, List<DepthChartEntryDto>>>> action)
        {
            var depthChart = await action();
            foreach (var chart in depthChart)
            {
                var displayText = $"{chart.Key} - ";
                displayText += String.Join(", ", chart.Value.Select(x => $"(#{x?.Player?.Number}, {x?.Player?.Name})"));

                Console.WriteLine(displayText);
            }
        }
    }
}
