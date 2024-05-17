using FanDuel.DepthChart.Application.Extensions;
using FanDuel.DepthChart.Application.Interfaces.Repositories;
using FanDuel.DepthChart.Contracts;

namespace FanDuel.DepthChart.Application
{
    public abstract class BaseDepthChartManager
    {
        protected readonly IRepository _repository;

        protected BaseDepthChartManager(IRepository repository)
        {
            _repository = repository;
        }

        public abstract void AddPlayerToDepthChart(string position, PlayerDto player, int? positionDepth = null);

        public abstract List<PlayerDto> GetBackups(string position, PlayerDto player);

        public virtual Dictionary<string, List<DepthChartEntryDto>> GetFullDepthChart()
        {
            var depthChart = _repository.GetTeamDepthChart(1);
            return depthChart.Entries.ToDictionary(x => x.Key, x => x.Value.Select(y => y.ToDepthChartEntryDto()).ToList());
        }

        public abstract List<PlayerDto> RemovePlayerFromDepthChart(string position, PlayerDto player);
    }
}
