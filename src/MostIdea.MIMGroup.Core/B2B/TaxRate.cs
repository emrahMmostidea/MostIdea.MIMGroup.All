using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace MostIdea.MIMGroup.B2B
{
    [Table("TaxRates")]
    public class TaxRate : FullAuditedEntity<Guid>
    {

        [Required]
        public virtual string Name { get; set; }

        public virtual decimal Rate { get; set; }

    }
}