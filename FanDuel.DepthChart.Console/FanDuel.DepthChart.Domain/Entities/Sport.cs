namespace FanDuel.DepthChart.Domain.Entities
{
    public record Sport
    {
        public Sport()
        {
            Teams = new HashSet<Team>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Team> Teams { get; set; }
    }
}
