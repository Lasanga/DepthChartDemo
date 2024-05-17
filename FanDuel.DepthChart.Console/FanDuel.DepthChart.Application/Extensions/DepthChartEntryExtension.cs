using FanDuel.DepthChart.Contracts;
using FanDuel.DepthChart.Domain.Entities;

namespace FanDuel.DepthChart.Application.Extensions
{
    public static class DepthChartEntryExtension
    {
        public static DepthChartEntryDto ToDepthChartEntryDto(this DepthChartEntry entry)
        {
            return new DepthChartEntryDto
            {
                Position = entry.Position,
                Rank = entry.Rank,
                Player = new PlayerDto
                {
                    Name = entry.Player.Name,
                    Number = entry.Player.Number
                }
            };
        }
    }
}
