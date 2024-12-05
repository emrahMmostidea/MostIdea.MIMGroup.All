using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MostIdea.MIMGroup.Authorization.Permissions.Dto;

namespace MostIdea.MIMGroup.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
