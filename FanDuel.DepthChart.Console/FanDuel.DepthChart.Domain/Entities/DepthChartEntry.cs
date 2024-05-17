namespace FanDuel.DepthChart.Domain.Entities
{
    public class DepthChartEntry
    {
        public int Rank { get; set; }
        public string Position { get; set; }
        public Player Player { get; set; } = new Player();
    }
}
