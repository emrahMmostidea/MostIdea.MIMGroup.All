using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class CreateOrEditTaxRateDto : EntityDto<Guid?>
    {

        [Required]
        public string Name { get; set; }

        public decimal Rate { get; set; }

    }
}