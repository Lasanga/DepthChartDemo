using FanDuel.DepthChart.Application.Interfaces.Repositories;
using FanDuel.DepthChart.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace FanDuel.DepthChart.Application.MLB
{
    public class MlbDepthChartManager([FromKeyedServices("Ef")] IRepository mongoRepository) : BaseDepthChartManager(mongoRepository), IMlbDepthChartManager
    {
        public override Task AddPlayerToDepthChart(string position, PlayerDto player, int? positionDepth = null, int? week = null)
        {
            throw new NotImplementedException();
        }

        public override Task<List<PlayerDto>> GetBackups(string position, PlayerDto player)
        {
            throw new NotImplementedException();
        }

        public override Task<List<PlayerDto>> RemovePlayerFromDepthChart(string position, PlayerDto player)
        {
            throw new NotImplementedException();
        }
    }
}
