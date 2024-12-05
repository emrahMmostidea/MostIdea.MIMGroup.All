using System.Collections.Generic;
using MostIdea.MIMGroup.Authorization.Delegation;
using MostIdea.MIMGroup.Authorization.Users.Delegation.Dto;

namespace MostIdea.MIMGroup.Web.Areas.App.Models.Layout
{
    public class ActiveUserDelegationsComboboxViewModel
    {
        public IUserDelegationConfiguration UserDelegationConfiguration { get; set; }
        
        public List<UserDelegationDto> UserDelegations { get; set; }
    }
}
