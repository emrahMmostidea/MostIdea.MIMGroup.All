using Abp.Application.Services.Dto;
using System;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class GetAllDistrictsForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string CityNameFilter { get; set; }

    }
}