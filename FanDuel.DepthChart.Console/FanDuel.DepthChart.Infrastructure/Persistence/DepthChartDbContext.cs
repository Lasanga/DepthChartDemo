using FanDuel.DepthChart.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FanDuel.DepthChart.Infrastructure.Persistence
{
    public partial class DepthChartDbContext : DbContext
    {
        public DepthChartDbContext(DbContextOptions<DepthChartDbContext> options) : base(options)
        {
            Sports.AddRange(InMemoryData.GetSports());
            SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(DepthChartDbContext).Assembly);
        }
    }
}
