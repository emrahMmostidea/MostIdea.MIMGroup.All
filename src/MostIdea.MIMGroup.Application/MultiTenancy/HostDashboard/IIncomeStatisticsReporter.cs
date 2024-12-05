using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MostIdea.MIMGroup.MultiTenancy.HostDashboard.Dto;

namespace MostIdea.MIMGroup.MultiTenancy.HostDashboard
{
    public interface IIncomeStatisticsService
    {
        Task<List<IncomeStastistic>> GetIncomeStatisticsData(DateTime startDate, DateTime endDate,
            ChartDateInterval dateInterval);
    }
}