using System.Threading.Tasks;
using MostIdea.MIMGroup.Sessions.Dto;

namespace MostIdea.MIMGroup.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
