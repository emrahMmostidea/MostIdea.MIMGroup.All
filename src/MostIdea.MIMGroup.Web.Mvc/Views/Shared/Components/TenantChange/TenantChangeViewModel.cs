using Abp.AutoMapper;
using MostIdea.MIMGroup.Sessions.Dto;

namespace MostIdea.MIMGroup.Web.Views.Shared.Components.TenantChange
{
    [AutoMapFrom(typeof(GetCurrentLoginInformationsOutput))]
    public class TenantChangeViewModel
    {
        public TenantLoginInfoDto Tenant { get; set; }
    }
}