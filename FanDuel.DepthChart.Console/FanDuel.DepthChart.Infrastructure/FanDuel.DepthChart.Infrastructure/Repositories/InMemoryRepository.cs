﻿using FanDuel.DepthChart.Application.Interfaces.Repositories;
using FanDuel.DepthChart.Domain.Entities;

namespace FanDuel.DepthChart.Infrastructure.Repositories
{
    public class InMemoryRepository : IRepository
    {
        private readonly ICollection<TeamDepthChart> _depthCharts;

        public InMemoryRepository()
        {
            _depthCharts = new HashSet<TeamDepthChart>();
        }

        public TeamDepthChart GetTeamDepthChart(int week)
        {
            if (week < 0 || week > 32)
            {
                throw new InvalidOperationException("Week must be in range of 0 - 32");
            }

            var depthChart = _depthCharts.FirstOrDefault(x => x.Week == week);
            if (depthChart is null)
            {
                var chart = new TeamDepthChart(week);
                _depthCharts.Add(chart);

                return chart;
            }

            return depthChart;
        }
    }
}
