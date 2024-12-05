using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class CreateOrEditBrandDto : EntityDto<Guid?>
    {

        [Required]
        [StringLength(BrandConsts.MaxNameLength, MinimumLength = BrandConsts.MinNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(BrandConsts.MaxDescriptionLength, MinimumLength = BrandConsts.MinDescriptionLength)]
        public string Description { get; set; }

    }
}