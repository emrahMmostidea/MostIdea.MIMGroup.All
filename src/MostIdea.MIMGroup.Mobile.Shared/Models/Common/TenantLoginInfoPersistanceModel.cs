using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using MostIdea.MIMGroup.Sessions.Dto;

namespace MostIdea.MIMGroup.Models.Common
{
    [AutoMapFrom(typeof(TenantLoginInfoDto)),
     AutoMapTo(typeof(TenantLoginInfoDto))]
    public class TenantLoginInfoPersistanceModel : EntityDto
    {
        public string TenancyName { get; set; }

        public string Name { get; set; }
    }
}