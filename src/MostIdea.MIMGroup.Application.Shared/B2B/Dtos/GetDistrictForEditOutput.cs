using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class GetDistrictForEditOutput
    {
        public CreateOrEditDistrictDto District { get; set; }

        public string CityName { get; set; }

    }
}