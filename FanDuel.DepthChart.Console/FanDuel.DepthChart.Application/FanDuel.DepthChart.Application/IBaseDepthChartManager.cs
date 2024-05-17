using FanDuel.DepthChart.Contracts;

namespace FanDuel.DepthChart.Application
{
    public interface IBaseDepthChartManager
    {
        /// <summary>
        /// Add a player to weekly depth chart for a given position and depth
        /// </summary>
        /// <param name="position"></param>
        /// <param name="player"></param>
        /// <param name="positionDepth"></param>
        void AddPlayerToDepthChart(string position, PlayerDto player, int? positionDepth = null);

        /// <summary>
        /// Remove a player from the depth chart for a given position
        /// </summary>
        /// <param name="position"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        List<PlayerDto> RemovePlayerFromDepthChart(string position, PlayerDto player);

        /// <summary>
        /// Get all competing players below a given player in a position
        /// </summary>
        /// <param name="position"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        List<PlayerDto> GetBackups(string position, PlayerDto player);

        /// <summary>
        /// Returns the full depth chart for all positions
        /// </summary>
        /// <returns></returns>
        Dictionary<string, List<DepthChartEntryDto>> GetFullDepthChart();
    }
}
