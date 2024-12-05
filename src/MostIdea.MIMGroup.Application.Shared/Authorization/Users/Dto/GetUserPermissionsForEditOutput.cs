using System.Collections.Generic;
using MostIdea.MIMGroup.Authorization.Permissions.Dto;

namespace MostIdea.MIMGroup.Authorization.Users.Dto
{
    public class GetUserPermissionsForEditOutput
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}