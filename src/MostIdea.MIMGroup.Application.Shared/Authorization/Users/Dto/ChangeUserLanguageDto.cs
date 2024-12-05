using System.ComponentModel.DataAnnotations;

namespace MostIdea.MIMGroup.Authorization.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}
