using FanDuel.DepthChart.Application.Interfaces.Repositories;
using FanDuel.DepthChart.Domain.Entities;
using FanDuel.DepthChart.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FanDuel.DepthChart.Infrastructure.Repositories
{
    public class EfInMemoryRepository(DepthChartDbContext dbContext) : IRepository
    {
        private readonly DepthChartDbContext _dbContext = dbContext;

        public async Task<Player> CreatePlayerAsync(Player player)
        {
            await _dbContext.AddAsync(player);
            await _dbContext.SaveChangesAsync();

            return player;
        }

        public async Task<Team> CreateTeamAsync(Team team)
        {
            await _dbContext.AddAsync(team);
            await _dbContext.SaveChangesAsync();

            return team;
        }

        public Task<int> GetSportIdByNameAsync(string name)
        {
            return _dbContext.Sports.AsNoTracking()
                .Where(x => x.Name == name)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();
        }

        public Task<Player?> GetPlayerByTeamNumberAsync(int teamID, int number)
        {
            return _dbContext.Players.AsNoTracking()
                .Where(x => x.TeamId == teamID && x.Number == number)
                .FirstOrDefaultAsync();
        }

        public async Task<TeamDepthChart> GetTeamDepthChartAsync(int week)
        {
            if (week < 1)
            {
                throw new InvalidOperationException("Week is invalid");
            }

            var depthChart = await _dbContext.TeamDepthCharts
                .Where(x => x.Week == week)
                .FirstOrDefaultAsync();
            if (depthChart is null)
            {
                depthChart = new TeamDepthChart(week);
                _dbContext.TeamDepthCharts.Add(depthChart);
                await _dbContext.SaveChangesAsync();
            }

            return depthChart;
        }

        public Task<int> UpdateTeamDepthChartAsync(TeamDepthChart teamDepthChart)
        {
            _dbContext.TeamDepthCharts.Update(teamDepthChart);
            return _dbContext.SaveChangesAsync();
        }
    }
}
