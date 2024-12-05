using MostIdea.MIMGroup.B2B;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace MostIdea.MIMGroup.B2B
{
    [Table("OrderComments")]
    public class OrderComment : FullAuditedEntity<Guid>
    {

        [Required]
        public virtual string Comment { get; set; }

        public virtual Guid OrderId { get; set; }

        [ForeignKey("OrderId")]
        public Order OrderFk { get; set; }

    }
}