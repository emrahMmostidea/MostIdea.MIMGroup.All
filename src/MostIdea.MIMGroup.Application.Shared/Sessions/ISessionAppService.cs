using System.Threading.Tasks;
using Abp.Application.Services;
using MostIdea.MIMGroup.Sessions.Dto;

namespace MostIdea.MIMGroup.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}
