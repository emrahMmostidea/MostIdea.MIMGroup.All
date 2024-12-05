using Abp.AutoMapper;
using MostIdea.MIMGroup.MultiTenancy.Dto;

namespace MostIdea.MIMGroup.Web.Models.TenantRegistration
{
    [AutoMapFrom(typeof(EditionsSelectOutput))]
    public class EditionsSelectViewModel : EditionsSelectOutput
    {
    }
}
