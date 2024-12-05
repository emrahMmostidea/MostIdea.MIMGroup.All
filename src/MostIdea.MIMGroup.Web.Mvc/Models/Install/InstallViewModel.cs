using System.Collections.Generic;
using Abp.Localization;
using MostIdea.MIMGroup.Install.Dto;

namespace MostIdea.MIMGroup.Web.Models.Install
{
    public class InstallViewModel
    {
        public List<ApplicationLanguage> Languages { get; set; }

        public AppSettingsJsonDto AppSettingsJson { get; set; }
    }
}
