using System.Collections.Generic;
using MostIdea.MIMGroup.Authorization.Permissions.Dto;

namespace MostIdea.MIMGroup.Authorization.Roles.Dto
{
    public class GetRoleForEditOutput
    {
        public RoleEditDto Role { get; set; }

        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}