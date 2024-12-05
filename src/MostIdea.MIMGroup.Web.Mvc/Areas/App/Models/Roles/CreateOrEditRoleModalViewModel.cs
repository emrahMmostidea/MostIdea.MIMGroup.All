using Abp.AutoMapper;
using MostIdea.MIMGroup.Authorization.Roles.Dto;
using MostIdea.MIMGroup.Web.Areas.App.Models.Common;

namespace MostIdea.MIMGroup.Web.Areas.App.Models.Roles
{
    [AutoMapFrom(typeof(GetRoleForEditOutput))]
    public class CreateOrEditRoleModalViewModel : GetRoleForEditOutput, IPermissionsEditViewModel
    {
        public bool IsEditMode => Role.Id.HasValue;
    }
}