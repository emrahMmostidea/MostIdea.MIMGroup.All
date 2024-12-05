using Abp.Auditing;
using MostIdea.MIMGroup.Configuration.Dto;

namespace MostIdea.MIMGroup.Configuration.Tenants.Dto
{
    public class TenantEmailSettingsEditDto : EmailSettingsEditDto
    {
        public bool UseHostDefaultEmailSettings { get; set; }
    }
}