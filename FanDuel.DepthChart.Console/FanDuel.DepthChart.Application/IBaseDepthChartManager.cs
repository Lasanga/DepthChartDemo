using FanDuel.DepthChart.Contracts;

namespace FanDuel.DepthChart.Application
{
    public interface IBaseDepthChartManager
    {
        /// <summary>
        /// Get Sport Id by name to create the teams
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<int> GetSportByNameAsync(string name);

        /// <summary>
        /// Creates a team for a sport
        /// </summary>
        /// <param name="teamDto"></param>
        /// <returns></returns>
        Task<TeamDto> CreateTeamAsync(TeamDto teamDto);

        /// <summary>
        /// Creates a player for a team
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        Task<PlayerDto> CreatePlayerAsync(PlayerDto player);

        /// <summary>
        /// Add a player to weekly depth chart for a given position and depth
        /// </summary>
        /// <param name="position"></param>
        /// <param name="player"></param>
        /// <param name="positionDepth"></param>
        /// <param name="week"></param>
        /// <returns></returns>
        Task AddPlayerToDepthChart(string position, PlayerDto player, int? positionDepth = null, int? week = null);

        /// <summary>
        /// Remove a player from the depth chart for a given position
        /// </summary>
        /// <param name="position"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        Task<List<PlayerDto>> RemovePlayerFromDepthChart(string position, PlayerDto player);

        /// <summary>
        /// Get all competing players below a given player in a position
        /// </summary>
        /// <param name="position"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        Task<List<PlayerDto>> GetBackups(string position, PlayerDto player);

        /// <summary>
        /// Returns the full depth chart for all positions
        /// </summary>
        /// <param name="week"></param>
        /// <returns></returns>
        Task<Dictionary<string, List<DepthChartEntryDto>>> GetFullDepthChart(int? week = null);
    }
}
