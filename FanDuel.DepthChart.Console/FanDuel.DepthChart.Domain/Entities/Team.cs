namespace FanDuel.DepthChart.Domain.Entities
{
    public record Team
    {
        public Team()
        {
            Players = new HashSet<Player>();
            DepthCharts = [];
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public List<TeamDepthChart> DepthCharts { get; set; }
        public ICollection<Player> Players { get; set; }
    }
}
