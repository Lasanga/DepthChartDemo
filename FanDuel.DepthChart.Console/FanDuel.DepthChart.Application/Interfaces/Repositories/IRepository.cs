using FanDuel.DepthChart.Domain.Entities;

namespace FanDuel.DepthChart.Application.Interfaces.Repositories
{
    public interface IRepository
    {
        Task<int> GetSportIdByNameAsync(string name);
        Task<Team> CreateTeamAsync(Team team);
        Task<Player> CreatePlayerAsync(Player player);
        Task<Player?> GetPlayerByTeamNumberAsync(int teamID, int number);
        Task<TeamDepthChart> GetTeamDepthChartAsync(int week);
        Task<int> UpdateTeamDepthChartAsync(TeamDepthChart teamDepthChart);
    }
}
