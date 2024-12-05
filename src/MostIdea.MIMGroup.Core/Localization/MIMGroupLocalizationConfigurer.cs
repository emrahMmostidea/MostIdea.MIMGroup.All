using System.Reflection;
using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace MostIdea.MIMGroup.Localization
{
    public static class MIMGroupLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    MIMGroupConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(MIMGroupLocalizationConfigurer).GetAssembly(),
                        "MostIdea.MIMGroup.Localization.MIMGroup"
                    )
                )
            );
        }
    }
}