using FanDuel.DepthChart.Domain.Entities;

namespace FanDuel.DepthChart.Infrastructure.Repositories
{
    public static class InMemoryData
    {
        public static List<Sport> GetSports()
        {
            return
            [
                new()
                {
                    Name = "NFL"
                }
            ];
        }
    }
}
