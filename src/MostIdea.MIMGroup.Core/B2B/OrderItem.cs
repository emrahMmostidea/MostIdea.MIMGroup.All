using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MostIdea.MIMGroup.B2B
{
    [Table("OrderItems")]
    public class OrderItem : FullAuditedEntity<Guid>
    {
        public virtual decimal Price { get; set; }

        public virtual int Amount { get; set; }

        public virtual OrderItemStatusEnum Status { get; set; }

        public virtual Guid ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product ProductFk { get; set; }

        public virtual Guid OrderId { get; set; }

        [ForeignKey("OrderId")]
        public Order OrderFk { get; set; }
    }
}