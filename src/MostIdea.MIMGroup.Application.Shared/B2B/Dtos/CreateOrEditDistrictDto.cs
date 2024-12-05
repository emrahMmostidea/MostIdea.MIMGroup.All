using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class CreateOrEditDistrictDto : EntityDto<Guid?>
    {

        [Required]
        public string Name { get; set; }

        public Guid CityId { get; set; }

    }
}