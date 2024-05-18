using FanDuel.DepthChart.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FanDuel.DepthChart.Infrastructure.Configurations
{
    internal class SportEntityConfiguration : IEntityTypeConfiguration<Sport>
    {
        public void Configure(EntityTypeBuilder<Sport> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.Teams)
                .WithOne(x => x.Sport)
                .HasForeignKey(x => x.SportId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
