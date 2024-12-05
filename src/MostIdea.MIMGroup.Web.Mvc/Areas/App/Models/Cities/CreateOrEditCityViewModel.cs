using MostIdea.MIMGroup.B2B.Dtos;
using System.Collections.Generic;

using Abp.Extensions;

namespace MostIdea.MIMGroup.Web.Areas.App.Models.Cities
{
    public class CreateOrEditCityModalViewModel
    {
        public CreateOrEditCityDto City { get; set; }

        public string CountryName { get; set; }

        public List<CityCountryLookupTableDto> CityCountryList { get; set; }

        public bool IsEditMode => City.Id.HasValue;
    }
}