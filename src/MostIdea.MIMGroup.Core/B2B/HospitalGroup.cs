using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace MostIdea.MIMGroup.B2B
{
    [Table("HospitalGroups")]
    public class HospitalGroup : FullAuditedEntity<Guid>
    {

        [Required]
        public virtual string Name { get; set; }

        public virtual List<Hospital> Hospitals { get; set; }

    }
}