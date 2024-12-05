using Abp.AutoMapper;
using MostIdea.MIMGroup.MultiTenancy.Dto;

namespace MostIdea.MIMGroup.Web.Models.TenantRegistration
{
    [AutoMapFrom(typeof(RegisterTenantOutput))]
    public class TenantRegisterResultViewModel : RegisterTenantOutput
    {
        public string TenantLoginAddress { get; set; }
    }
}