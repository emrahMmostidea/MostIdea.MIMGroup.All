using System;
using System.Collections.Generic;
using System.Text;

namespace MostIdea.MIMGroup.B2B.Dtos
{ 
    public class OrderProductDto
    {
        public OrderProductDto()
        {
            ProductImage = ProductImageId.HasValue
                ? "/file/get?id=" + ProductImageId.Value
                : "/Common/Images/default-picture.png";
        }

        public Guid OrderItemId { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public Guid TaxRateId { get; set; }
        public string TaxRateName { get; set; }
        public decimal TaxRate { get; set; }
        public OrderItemStatusEnum Status { get; set; }
        public int Amount { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductImage { get; set; }
        public Guid? ProductImageId { get; set; }
        public Guid ProductCategoryId { get; set; }
        public string ProductCategory { get; set; }
        public Guid ProductBrandId { get; set; }
        public string ProductBrandName { get; set; }
    }
}
