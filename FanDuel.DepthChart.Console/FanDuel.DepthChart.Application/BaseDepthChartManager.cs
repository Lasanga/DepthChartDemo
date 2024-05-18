using FanDuel.DepthChart.Application.Extensions;
using FanDuel.DepthChart.Application.Interfaces.Repositories;
using FanDuel.DepthChart.Contracts;
using FanDuel.DepthChart.Domain.Entities;

namespace FanDuel.DepthChart.Application
{
    public abstract class BaseDepthChartManager
    {
        protected readonly IRepository _repository;

        protected BaseDepthChartManager(IRepository repository)
        {
            _repository = repository;
        }

        /// <inheritdoc />
        public Task<int> GetSportByNameAsync(string name)
        {
            return _repository.GetSportIdByNameAsync(name);
        }

        /// <inheritdoc />
        public virtual async Task<TeamDto> CreateTeamAsync(TeamDto teamDto)
        {
            var team = await _repository.CreateTeamAsync(new Team
            {
                Name = teamDto.Name,
                SportId = teamDto.SportId
            });

            return new TeamDto
            {
                Name = team.Name,
                TeamId = team.Id,
                SportId = team.SportId
            };
        }

        /// <inheritdoc />
        public virtual async Task<PlayerDto> CreatePlayerAsync(PlayerDto playerDto)
        {
            var player = await _repository.CreatePlayerAsync(new Player
            {
                Name = playerDto.Name,
                Number = playerDto.Number,
                TeamId = playerDto.TeamId
            });

            return new PlayerDto
            {
                Name = player.Name,
                TeamId = player.TeamId,
                Number = player.Number
            };
        }

        /// <inheritdoc />
        public abstract Task AddPlayerToDepthChart(string position, PlayerDto player, int? positionDepth = null, int? week = null);

        /// <inheritdoc />
        public abstract Task<List<PlayerDto>> GetBackups(string position, PlayerDto player);

        /// <inheritdoc />
        public virtual async Task<Dictionary<string, List<DepthChartEntryDto>>> GetFullDepthChart(int? week = null)
        {
            var depthChart = await _repository.GetTeamDepthChartAsync(week ?? 1);
            return depthChart.Entries.ToDictionary(x => x.Key, x => x.Value.Select(y => y.ToDepthChartEntryDto()).ToList());
        }

        /// <inheritdoc />
        public abstract Task<List<PlayerDto>> RemovePlayerFromDepthChart(string position, PlayerDto player);
    }
}
