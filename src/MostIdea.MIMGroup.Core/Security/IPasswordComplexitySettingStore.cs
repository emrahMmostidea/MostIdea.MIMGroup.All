using System.Threading.Tasks;

namespace MostIdea.MIMGroup.Security
{
    public interface IPasswordComplexitySettingStore
    {
        Task<PasswordComplexitySetting> GetSettingsAsync();
    }
}
