using FanDuel.DepthChart.Application.Interfaces.Repositories;
using FanDuel.DepthChart.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace FanDuel.DepthChart.Application.NHL
{
    public class NhlDepthChartManager([FromKeyedServices("Local")] IRepository repository) : BaseDepthChartManager(repository), INhlDepthChartManager
    {
        public override void AddPlayerToDepthChart(string position, PlayerDto player, int? positionDepth = null)
        {
            throw new NotImplementedException();
        }

        public override List<PlayerDto> GetBackups(string position, PlayerDto player)
        {
            throw new NotImplementedException();
        }

        public override List<PlayerDto> RemovePlayerFromDepthChart(string position, PlayerDto player)
        {
            throw new NotImplementedException();
        }
    }
}
