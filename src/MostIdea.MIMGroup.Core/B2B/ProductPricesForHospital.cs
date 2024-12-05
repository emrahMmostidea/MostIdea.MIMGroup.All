using MostIdea.MIMGroup.B2B;
using MostIdea.MIMGroup.B2B;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace MostIdea.MIMGroup.B2B
{
    [Table("ProductPricesForHospitals")]
    public class ProductPricesForHospital : FullAuditedEntity<Guid>
    {

        public virtual DateTime StartDate { get; set; }

        public virtual DateTime EndDate { get; set; }

        public virtual decimal Price { get; set; }

        public virtual Guid? ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product ProductFk { get; set; }

        public virtual Guid? ProductCategoryId { get; set; }

        [ForeignKey("ProductCategoryId")]
        public ProductCategory ProductCategoryFk { get; set; }

    }
}