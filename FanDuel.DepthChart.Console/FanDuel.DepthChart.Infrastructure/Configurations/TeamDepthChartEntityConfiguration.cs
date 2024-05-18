using FanDuel.DepthChart.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace FanDuel.DepthChart.Infrastructure.Configurations
{
    internal class TeamDepthChartEntityConfiguration : IEntityTypeConfiguration<TeamDepthChart>
    {
        public void Configure(EntityTypeBuilder<TeamDepthChart> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Team)
                .WithMany(x => x.DepthCharts)
                .HasForeignKey(x => x.TeamId);

            var converter = new ValueConverter<Dictionary<string, List<DepthChartEntry>>, string>(
                        v => JsonConvert.SerializeObject(v),
                        v => JsonConvert.DeserializeObject<Dictionary<string, List<DepthChartEntry>>>(v));


            builder.Property(e => e.Entries)
                .HasConversion(converter);
        }
    }
}
