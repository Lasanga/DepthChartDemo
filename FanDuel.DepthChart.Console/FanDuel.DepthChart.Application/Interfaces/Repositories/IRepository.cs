using FanDuel.DepthChart.Domain.Entities;

namespace FanDuel.DepthChart.Application.Interfaces.Repositories
{
    public interface IRepository
    {
        TeamDepthChart GetTeamDepthChart(int week);
    }
}
