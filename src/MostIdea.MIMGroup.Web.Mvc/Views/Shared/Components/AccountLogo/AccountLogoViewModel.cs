﻿using MostIdea.MIMGroup.Sessions.Dto;

namespace MostIdea.MIMGroup.Web.Views.Shared.Components.AccountLogo
{
    public class AccountLogoViewModel
    {
        public GetCurrentLoginInformationsOutput LoginInformations { get; }

        private string _skin = "light";

        public AccountLogoViewModel(GetCurrentLoginInformationsOutput loginInformations, string skin)
        {
            LoginInformations = loginInformations;
            _skin = skin;
        }

        public string GetLogoUrl(string appPath)
        {
            if (LoginInformations?.Tenant?.LogoId == null)
            {
                //return appPath + "Common/Images/app-logo-on-" + _skin + ".svg";
                return appPath + "Common/Images/1726521056_mim-logo-white.png";
            }

            return appPath + "TenantCustomization/GetLogo?tenantId=" + LoginInformations?.Tenant?.Id;
        }
    }
}