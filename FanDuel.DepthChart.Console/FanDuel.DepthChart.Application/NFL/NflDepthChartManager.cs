using FanDuel.DepthChart.Application.Interfaces.Repositories;
using FanDuel.DepthChart.Contracts;
using FanDuel.DepthChart.Domain.Entities;
using FanDuel.DepthChart.Domain.Types;
using Microsoft.Extensions.DependencyInjection;

namespace FanDuel.DepthChart.Application.NFL
{
    public class NflDepthChartManager([FromKeyedServices("Ef")] IRepository localRepository) : BaseDepthChartManager(localRepository), INflDepthChartManager
    {
        public override async Task AddPlayerToDepthChart(string position, PlayerDto playerDto, int? positionDepth = null, int? week = null)
        {
            ValidatePosition(position);

            var player = await GetPlayerAsync(playerDto);
            var depthChart = await _repository.GetTeamDepthChartAsync(week ?? 1);

            if (!depthChart.Entries.TryGetValue(position, out _))
            {
                depthChart.Entries[position] = [];
            }

            if (depthChart.Entries[position].Any(x => x.Player.Number == player.Number))
            {
                throw new InvalidOperationException("Cannot have the same player multiple times for the same position");
            }

            var index = depthChart.Entries[position].FindIndex(x => x.Rank == positionDepth);
            if (index is -1)
            {
                depthChart.Entries[position].Add(new DepthChartEntry
                {
                    Rank = depthChart.Entries[position].Count,
                    Position = position,
                    Player = player
                });
            }
            else
            {
                depthChart.Entries[position].Insert(index, new DepthChartEntry
                {
                    Rank = positionDepth.GetValueOrDefault(),
                    Position = position,
                    Player = player
                });

                for (int i = index + 1; i < depthChart.Entries[position].Count; i++)
                {
                    depthChart.Entries[position][i].Rank += 1;
                }
            }

            depthChart.Entries[position] = [.. depthChart.Entries[position].OrderBy(x => x.Rank)];
            await _repository.UpdateTeamDepthChartAsync(depthChart);
        }

        public override async Task<List<PlayerDto>> GetBackups(string position, PlayerDto player)
        {
            ValidatePosition(position);
            var depthChart = await _repository.GetTeamDepthChartAsync(1);

            if (!depthChart.Entries.TryGetValue(position, out var entries))
            {
                throw new InvalidDataException("Depth chart for the requested position was not found");
            }

            var index = entries.FindIndex(x => x.Player.Number == player.Number);
            if (index is -1)
            {
                return [];
            }

            return entries.Skip(index + 1).Select(x => new PlayerDto
            {
                Name = x.Player.Name,
                Number = x.Player.Number
            }).ToList();
        }

        public override async Task<List<PlayerDto>> RemovePlayerFromDepthChart(string position, PlayerDto player)
        {
            ValidatePosition(position);
            var depthChart = await _repository.GetTeamDepthChartAsync(1);

            if (!depthChart.Entries.TryGetValue(position, out var entries))
            {
                throw new InvalidDataException("Depth chart for the requested position was not found");
            }

            var index = entries.FindIndex(x => x.Player.Number == player.Number);
            if (index is not -1)
            {
                entries.RemoveAt(index);
                for (int i = index; i < entries.Count; i++)
                {
                    entries[i].Rank -= 1;
                }

                return new List<PlayerDto> { player };
            }

            return [];
        }

        #region Private methods
        private static void ValidatePosition(string position)
        {
            var validNflPositions = Enum.GetValues(typeof(NflPositionTypes))
                .Cast<NflPositionTypes>()
                .Select(x => x.ToString())
                .ToList();

            if (!validNflPositions.Contains(position))
            {
                throw new InvalidOperationException("Invalid NFL position");
            }
        }

        private async Task<Player?> GetPlayerAsync(PlayerDto playerDto)
        {
            var player = await _repository.GetPlayerByTeamNumberAsync(playerDto.TeamId, playerDto.Number);
            return player is null ? throw new InvalidOperationException($"Player number => {playerDto.Number} could not be found") : player;
        }
        #endregion
    }
}
