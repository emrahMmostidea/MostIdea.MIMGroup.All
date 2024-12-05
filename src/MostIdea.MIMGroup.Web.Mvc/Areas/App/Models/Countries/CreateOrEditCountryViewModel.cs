using MostIdea.MIMGroup.B2B.Dtos;

using Abp.Extensions;

namespace MostIdea.MIMGroup.Web.Areas.App.Models.Countries
{
    public class CreateOrEditCountryModalViewModel
    {
        public CreateOrEditCountryDto Country { get; set; }

        public bool IsEditMode => Country.Id.HasValue;
    }
}