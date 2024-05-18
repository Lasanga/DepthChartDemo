using FanDuel.DepthChart.Application.Interfaces.Repositories;
using FanDuel.DepthChart.Domain.Entities;

namespace FanDuel.DepthChart.Infrastructure.Repositories
{
    public class InMemoryRepository : IRepository
    {
        private readonly ICollection<Sport> _sports;
        private readonly ICollection<Team> _teams;
        private readonly ICollection<Player> _players;
        private readonly ICollection<TeamDepthChart> _depthCharts;

        public InMemoryRepository()
        {
            _sports = new HashSet<Sport>();
            _teams = new HashSet<Team>();
            _depthCharts = new HashSet<TeamDepthChart>();
            _players = new HashSet<Player>();
        }

        public Task<Team> CreateTeamAsync(Team team)
        {
            _teams.Add(team);
            return Task.FromResult(team);
        }

        public Task<Player> CreatePlayerAsync(Player player)
        {
            _players.Add(player);
            return Task.FromResult(player);
        }

        public Task<Player?> GetPlayerByTeamNumberAsync(int teamID, int number)
        {
            var player = _players
                .Where(x => x.TeamId == teamID && x.Number == number)
                .FirstOrDefault();
            return Task.FromResult(player);
        }

        public Task<TeamDepthChart> GetTeamDepthChartAsync(int week)
        {
            if (week < 1)
            {
                throw new InvalidOperationException("Week is invalid");
            }

            var depthChart = _depthCharts
                .Where(x => x.Week == week)
                .FirstOrDefault();
            if (depthChart is null)
            {
                depthChart = new TeamDepthChart(week);
                _depthCharts.Add(depthChart);
            }

            return Task.FromResult(depthChart);
        }

        public Task<int> UpdateTeamDepthChartAsync(TeamDepthChart teamDepthChart)
        {
            // Updated by refernce
            return Task.FromResult(1);
        }

        public Task<int> GetSportIdByNameAsync(string name)
        {
            var id = _sports.Where(x => x.Name == name)
                .Select(x => x.Id)
                .FirstOrDefault();

            return Task.FromResult(id);
        }
    }
}
