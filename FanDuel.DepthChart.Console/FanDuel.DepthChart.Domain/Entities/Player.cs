namespace FanDuel.DepthChart.Domain.Entities
{
    public record Player
    {
        public int Number { get; set; }
        public string? Name { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
    }
}
