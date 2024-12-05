using System.Threading.Tasks;
using Abp;
using MostIdea.MIMGroup.Configuration.Dto;
using MostIdea.MIMGroup.UiCustomization;
using MostIdea.MIMGroup.UiCustomization.Dto;

namespace MostIdea.MIMGroup.Test.Base.UiCustomization
{
    public class NullThemeUiCustomizer : IUiCustomizer
    {
        public Task<UiCustomizationSettingsDto> GetUiSettings()
        {
            return Task.FromResult(new UiCustomizationSettingsDto());
        }

        public Task UpdateUserUiManagementSettingsAsync(UserIdentifier user, ThemeSettingsDto settings)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateTenantUiManagementSettingsAsync(int tenantId, ThemeSettingsDto settings)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateApplicationUiManagementSettingsAsync(ThemeSettingsDto settings)
        {
            throw new System.NotImplementedException();
        }

        public Task<ThemeSettingsDto> GetHostUiManagementSettings()
        {
            throw new System.NotImplementedException();
        }

        public Task<ThemeSettingsDto> GetTenantUiCustomizationSettings(int tenantId)
        {
            throw new System.NotImplementedException();
        }
    }
}