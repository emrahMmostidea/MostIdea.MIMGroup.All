using System.ComponentModel.DataAnnotations;

namespace MostIdea.MIMGroup.Localization.Dto
{
    public class CreateOrUpdateLanguageInput
    {
        [Required]
        public ApplicationLanguageEditDto Language { get; set; }
    }
}