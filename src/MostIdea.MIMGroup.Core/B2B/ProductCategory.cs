using MostIdea.MIMGroup.B2B;
using MostIdea.MIMGroup.B2B;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace MostIdea.MIMGroup.B2B
{
    [Table("ProductCategories")]
    public class ProductCategory : FullAuditedEntity<Guid>
    {

        [Required]
        [StringLength(ProductCategoryConsts.MaxNameLength, MinimumLength = ProductCategoryConsts.MinNameLength)]
        public virtual string Name { get; set; }

        [StringLength(ProductCategoryConsts.MaxDescriptionLength, MinimumLength = ProductCategoryConsts.MinDescriptionLength)]
        public virtual string Description { get; set; }

        public virtual Guid? ProductCategoryId { get; set; }

        [ForeignKey("ProductCategoryId")]
        public ProductCategory ProductCategoryFk { get; set; }

        public virtual Guid BrandId { get; set; }

        [ForeignKey("BrandId")]
        public Brand BrandFk { get; set; }

        public virtual List<ProductCategory> ChildCategories { get; set; }

    }
}