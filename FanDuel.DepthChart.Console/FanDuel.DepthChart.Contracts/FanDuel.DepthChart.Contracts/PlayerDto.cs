namespace FanDuel.DepthChart.Contracts
{
    public record PlayerDto
    {
        public int Number { get; set; }
        public string? Name { get; set; }
        public int TeamId { get; set; }
    }
}
