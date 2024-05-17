namespace FanDuel.DepthChart.Contracts
{
    public class DepthChartEntryDto
    {
        public int Rank { get; set; }
        public string Position { get; set; }
        public PlayerDto? Player { get; set; }
    }
}
