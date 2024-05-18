using FanDuel.DepthChart.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FanDuel.DepthChart.Infrastructure.Persistence
{
    public partial class DepthChartDbContext : DbContext
    {
        public virtual DbSet<Sport> Sports { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<TeamDepthChart> TeamDepthCharts { get; set; }
    }
}
