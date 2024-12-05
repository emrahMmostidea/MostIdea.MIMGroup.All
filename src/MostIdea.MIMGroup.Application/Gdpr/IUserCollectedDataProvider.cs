using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using MostIdea.MIMGroup.Dto;

namespace MostIdea.MIMGroup.Gdpr
{
    public interface IUserCollectedDataProvider
    {
        Task<List<FileDto>> GetFiles(UserIdentifier user);
    }
}
