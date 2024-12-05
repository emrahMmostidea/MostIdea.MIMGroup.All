using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace MostIdea.MIMGroup.B2B
{
    [Table("DynamicEnums")]
    public class DynamicEnum : FullAuditedEntity<Guid>
    {

        [Required]
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        [Required]
        public virtual string EnumFile { get; set; }

    }
}