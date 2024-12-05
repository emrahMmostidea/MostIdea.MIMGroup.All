using Abp.AutoMapper;
using MostIdea.MIMGroup.Authorization.Users;
using MostIdea.MIMGroup.Authorization.Users.Dto;
using MostIdea.MIMGroup.Web.Areas.App.Models.Common;

namespace MostIdea.MIMGroup.Web.Areas.App.Models.Users
{
    [AutoMapFrom(typeof(GetUserPermissionsForEditOutput))]
    public class UserPermissionsEditViewModel : GetUserPermissionsForEditOutput, IPermissionsEditViewModel
    {
        public User User { get; set; }
    }
}