using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class CreateOrEditProductCategoryDto : EntityDto<Guid?>
    {

        [Required]
        [StringLength(ProductCategoryConsts.MaxNameLength, MinimumLength = ProductCategoryConsts.MinNameLength)]
        public string Name { get; set; }

        [StringLength(ProductCategoryConsts.MaxDescriptionLength, MinimumLength = ProductCategoryConsts.MinDescriptionLength)]
        public string Description { get; set; }

        public Guid? ProductCategoryId { get; set; }

        public Guid BrandId { get; set; }

    }
}