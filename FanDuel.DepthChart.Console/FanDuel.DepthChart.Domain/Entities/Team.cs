namespace FanDuel.DepthChart.Domain.Entities
{
    public record Team
    {
        public Team()
        {
            DepthCharts = new HashSet<TeamDepthChart>();
            Players = new HashSet<Player>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int SportId { get; set; }
        public Sport Sport { get; set; }
        public ICollection<TeamDepthChart> DepthCharts { get; set; }
        public ICollection<Player> Players { get; set; }
    }
}
