using MostIdea.MIMGroup.B2B;
using MostIdea.MIMGroup.B2B;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using MostIdea.MIMGroup.Storage;

namespace MostIdea.MIMGroup.B2B
{
    [Table("Products")]
    public class Product : FullAuditedEntity<Guid>
    {

        [Required]
        [StringLength(ProductConsts.MaxNameLength, MinimumLength = ProductConsts.MinNameLength)]
        public virtual string Name { get; set; }

        [StringLength(ProductConsts.MaxDescriptionLength, MinimumLength = ProductConsts.MinDescriptionLength)]
        public virtual string Description { get; set; }

        public virtual decimal Price { get; set; }

        public virtual int Quantity { get; set; }

        public virtual Guid ProductCategoryId { get; set; }

        [ForeignKey("ProductCategoryId")]
        public ProductCategory ProductCategoryFk { get; set; }

        public virtual Guid TaxRateId { get; set; }

        [ForeignKey("TaxRateId")]
        public TaxRate TaxRateFk { get; set; }

        public virtual Guid? ImageId { get; set; }

        [ForeignKey("ImageId")]
        public BinaryObject Image { get; set; }

    }
}