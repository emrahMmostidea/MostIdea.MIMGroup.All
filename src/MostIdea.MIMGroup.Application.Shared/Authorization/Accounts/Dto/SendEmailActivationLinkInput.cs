using System.ComponentModel.DataAnnotations;

namespace MostIdea.MIMGroup.Authorization.Accounts.Dto
{
    public class SendEmailActivationLinkInput
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}