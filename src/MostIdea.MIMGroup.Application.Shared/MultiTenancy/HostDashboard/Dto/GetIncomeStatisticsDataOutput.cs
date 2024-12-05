using System.Collections.Generic;

namespace MostIdea.MIMGroup.MultiTenancy.HostDashboard.Dto
{
    public class GetIncomeStatisticsDataOutput
    {
        public List<IncomeStastistic> IncomeStatistics { get; set; }

        public GetIncomeStatisticsDataOutput(List<IncomeStastistic> incomeStatistics)
        {
            IncomeStatistics = incomeStatistics;
        }
    }
}