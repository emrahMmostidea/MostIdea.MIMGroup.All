using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class CreateOrEditCityDto : EntityDto<Guid?>
    {

        [Required]
        public string Name { get; set; }

        public Guid CountryId { get; set; }

    }
}