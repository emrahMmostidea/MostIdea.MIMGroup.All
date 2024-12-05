using System.Collections.Generic;
using MostIdea.MIMGroup.Authorization.Permissions.Dto;

namespace MostIdea.MIMGroup.Web.Areas.App.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }

        List<string> GrantedPermissionNames { get; set; }
    }
}