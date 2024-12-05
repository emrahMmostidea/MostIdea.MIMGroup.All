using System.Threading.Tasks;
using Abp.Application.Services;
using MostIdea.MIMGroup.Install.Dto;

namespace MostIdea.MIMGroup.Install
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}