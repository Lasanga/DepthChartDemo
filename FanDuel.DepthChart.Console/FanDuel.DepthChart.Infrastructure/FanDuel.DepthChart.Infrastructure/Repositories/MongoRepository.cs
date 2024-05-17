using FanDuel.DepthChart.Application.Interfaces.Repositories;
using FanDuel.DepthChart.Domain.Entities;

namespace FanDuel.DepthChart.Infrastructure.Repositories
{
    public class MongoRepository : IRepository
    {
        public TeamDepthChart GetTeamDepthChart(int week)
        {
            throw new NotImplementedException();
        }
    }
}
