using System.Collections.Generic;
using Abp.Application.Services.Dto;
using MostIdea.MIMGroup.Editions.Dto;

namespace MostIdea.MIMGroup.MultiTenancy.Dto
{
    public class GetTenantFeaturesEditOutput
    {
        public List<NameValueDto> FeatureValues { get; set; }

        public List<FlatFeatureDto> Features { get; set; }
    }
}