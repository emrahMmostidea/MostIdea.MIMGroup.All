using System.Collections.Generic;
using Abp.Application.Services.Dto;
using MostIdea.MIMGroup.Authorization.Permissions.Dto;
using MostIdea.MIMGroup.Web.Areas.App.Models.Common;

namespace MostIdea.MIMGroup.Web.Areas.App.Models.Roles
{
    public class RoleListViewModel : IPermissionsEditViewModel
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}