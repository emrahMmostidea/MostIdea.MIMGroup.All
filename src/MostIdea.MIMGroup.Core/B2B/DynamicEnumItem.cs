using MostIdea.MIMGroup.B2B;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace MostIdea.MIMGroup.B2B
{
    [Table("DynamicEnumItems")]
    public class DynamicEnumItem : FullAuditedEntity<Guid>
    {

        [Required]
        public virtual string EnumValue { get; set; }

        [Required]
        public virtual string Label { get; set; }

        public virtual Guid? ParentId { get; set; }

        public virtual bool IsAuthRestriction { get; set; }

        public virtual string AuthorizedUsers { get; set; }

        public virtual int Order  { get; set; }

        public virtual Guid DynamicEnumId { get; set; }

        [ForeignKey("DynamicEnumId")]
        public DynamicEnum DynamicEnumFk { get; set; }

    }
}