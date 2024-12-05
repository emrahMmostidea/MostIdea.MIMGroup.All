using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using MostIdea.MIMGroup.Authorization.Users;
using MostIdea.MIMGroup.MultiTenancy;

namespace MostIdea.MIMGroup.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
        }
    }
}