using FanDuel.DepthChart.Contracts;
using FanDuel.DepthChart.Domain.Entities;

namespace FanDuel.DepthChart.Application.Extensions
{
    public static class PlayerExtensions
    {
        public static Player MapToPlayer(this PlayerDto playerDto)
            => new()
            {
                Name = playerDto.Name,
                Number = playerDto.Number
            };
    }
}
