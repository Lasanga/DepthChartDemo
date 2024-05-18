using FanDuel.DepthChart.Application.Interfaces.Repositories;
using FanDuel.DepthChart.Domain.Entities;

namespace FanDuel.DepthChart.Infrastructure.Repositories
{
    public class InMemoryRepository : IRepository
    {
        private readonly ICollection<TeamDepthChart> _depthCharts;

        public InMemoryRepository()
        {
            _depthCharts = new HashSet<TeamDepthChart>();
        }

        public TeamDepthChart GetTeamDepthChart(int week)
        {
            if (week < 1)
            {
                throw new InvalidOperationException("Week is invalid");
            }

            var depthChart = _depthCharts.FirstOrDefault(x => x.Week == week);
            if (depthChart is null)
            {
                depthChart = new TeamDepthChart(week);
                _depthCharts.Add(depthChart);
            }

            return depthChart;
        }
    }
}
