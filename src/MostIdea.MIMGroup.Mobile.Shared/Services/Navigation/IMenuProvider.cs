using System.Collections.Generic;
using MvvmHelpers;
using MostIdea.MIMGroup.Models.NavigationMenu;

namespace MostIdea.MIMGroup.Services.Navigation
{
    public interface IMenuProvider
    {
        ObservableRangeCollection<NavigationMenuItem> GetAuthorizedMenuItems(Dictionary<string, string> grantedPermissions);
    }
}