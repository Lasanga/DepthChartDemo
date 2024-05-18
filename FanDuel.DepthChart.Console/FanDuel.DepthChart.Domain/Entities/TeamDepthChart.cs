namespace FanDuel.DepthChart.Domain.Entities
{
    public class TeamDepthChart
    {
        public TeamDepthChart()
        {

        }
        public TeamDepthChart(int week)
        {
            Week = week;
            Entries = [];
        }

        public Guid Id { get; set; }
        public int Week { get; set; }
        public int TeamId { get; set; }
        public Dictionary<string, List<DepthChartEntry>> Entries { get; }
        public Team Team { get; set; }
    }
}
