namespace FanDuel.DepthChart.Domain.Entities
{
    public record Player
    {
        public Player()
        {
            Team = new Team();
        }

        public int Number { get; set; }
        public string? Name { get; set; }
        public Team Team { get; set; }
    }
}
