using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class CreateOrEditDynamicEnumDto : EntityDto<Guid?>
    {

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public string EnumFile { get; set; }

    }
}