using System;
using System.IO;
using Abp.Application.Services.Dto;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class ProductDto : EntityDto<Guid>
    {
        public ProductDto()
        {
            Image = ImageId.HasValue
                ? "/file/get?id=" + ImageId.Value
                : "/Common/Images/default-picture.png";
            
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public Guid ProductCategoryId { get; set; }

        public Guid TaxRateId { get; set; }

        public decimal TaxRate { get; set; }

        public string Image { get; set; }

        public Guid? ImageId { get; set; }

        public string ProductCategoryName { get; set; }

        public string BrandName { get; set; }

        public ProductCategoryDto ProductCategoryFk { get; set; }

    }
}