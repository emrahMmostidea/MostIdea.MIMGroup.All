using MostIdea.MIMGroup.B2B.Dtos;
using System.Collections.Generic;

using Abp.Extensions;

namespace MostIdea.MIMGroup.Web.Areas.App.Models.Districts
{
    public class CreateOrEditDistrictModalViewModel
    {
        public CreateOrEditDistrictDto District { get; set; }

        public string CityName { get; set; }

        public List<DistrictCityLookupTableDto> DistrictCityList { get; set; }

        public bool IsEditMode => District.Id.HasValue;
    }
}