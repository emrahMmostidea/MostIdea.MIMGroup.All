using System.ComponentModel.DataAnnotations;

namespace MostIdea.MIMGroup.Web.Models.Account
{
    public class SendPasswordResetLinkViewModel
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}