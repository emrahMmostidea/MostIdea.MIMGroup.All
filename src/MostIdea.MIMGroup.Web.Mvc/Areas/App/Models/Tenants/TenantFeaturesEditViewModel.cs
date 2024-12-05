using Abp.AutoMapper;
using MostIdea.MIMGroup.MultiTenancy;
using MostIdea.MIMGroup.MultiTenancy.Dto;
using MostIdea.MIMGroup.Web.Areas.App.Models.Common;

namespace MostIdea.MIMGroup.Web.Areas.App.Models.Tenants
{
    [AutoMapFrom(typeof (GetTenantFeaturesEditOutput))]
    public class TenantFeaturesEditViewModel : GetTenantFeaturesEditOutput, IFeatureEditViewModel
    {
        public Tenant Tenant { get; set; }
    }
}