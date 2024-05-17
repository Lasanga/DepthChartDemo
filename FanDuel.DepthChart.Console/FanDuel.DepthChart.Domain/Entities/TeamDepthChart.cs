namespace FanDuel.DepthChart.Domain.Entities
{
    public class TeamDepthChart
    {
        public TeamDepthChart(int week)
        {
            Week = week;
            Entries = [];
        }

        public int Week { get; }
        public Dictionary<string, List<DepthChartEntry>> Entries { get; }
    }
}
