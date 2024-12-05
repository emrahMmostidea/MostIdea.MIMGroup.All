using MostIdea.MIMGroup.B2B;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace MostIdea.MIMGroup.B2B
{
    [Table("Cities")]
    public class City : FullAuditedEntity<Guid>
    {

        [Required]
        public virtual string Name { get; set; }

        public virtual Guid CountryId { get; set; }

        [ForeignKey("CountryId")]
        public Country CountryFk { get; set; }

    }
}