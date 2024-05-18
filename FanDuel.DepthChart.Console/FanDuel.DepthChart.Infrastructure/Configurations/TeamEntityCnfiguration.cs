using FanDuel.DepthChart.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FanDuel.DepthChart.Infrastructure.Configurations
{
    internal class TeamEntityCnfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Sport)
                .WithMany(x => x.Teams)
                .HasForeignKey(x => x.SportId);

            builder.HasMany(x => x.Players)
                .WithOne(x => x.Team)
                .HasForeignKey(x => x.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.DepthCharts)
                .WithOne(x => x.Team)
                .HasForeignKey(x => x.TeamId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
