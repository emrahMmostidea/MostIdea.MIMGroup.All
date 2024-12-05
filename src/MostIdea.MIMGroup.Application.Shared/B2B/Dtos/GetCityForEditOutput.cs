using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class GetCityForEditOutput
    {
        public CreateOrEditCityDto City { get; set; }

        public string CountryName { get; set; }

    }
}