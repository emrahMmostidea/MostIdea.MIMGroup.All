using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class CreateOrEditProductDto : EntityDto<Guid?>
    {

        [Required]
        [StringLength(ProductConsts.MaxNameLength, MinimumLength = ProductConsts.MinNameLength)]
        public string Name { get; set; }

        [StringLength(ProductConsts.MaxDescriptionLength, MinimumLength = ProductConsts.MinDescriptionLength)]
        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public Guid ProductCategoryId { get; set; }

        public Guid TaxRateId { get; set; }

    }
}