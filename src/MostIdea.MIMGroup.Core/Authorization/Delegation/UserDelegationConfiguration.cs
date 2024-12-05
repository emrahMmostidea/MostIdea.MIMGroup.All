namespace MostIdea.MIMGroup.Authorization.Delegation
{
    public class UserDelegationConfiguration : IUserDelegationConfiguration
    {
        public bool IsEnabled { get; set; }

        public UserDelegationConfiguration()
        {
            IsEnabled = true;
        }
    }
}
