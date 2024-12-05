using System.Threading.Tasks;
using Abp.Domain.Policies;

namespace MostIdea.MIMGroup.Authorization.Users
{
    public interface IUserPolicy : IPolicy
    {
        Task CheckMaxUserCountAsync(int tenantId);
    }
}
