using System.Globalization;

namespace MostIdea.MIMGroup.Localization
{
    public interface IApplicationCulturesProvider
    {
        CultureInfo[] GetAllCultures();
    }
}