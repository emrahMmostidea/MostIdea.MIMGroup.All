using MostIdea.MIMGroup.B2B;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace MostIdea.MIMGroup.B2B
{
    [Table("Districts")]
    public class District : FullAuditedEntity<Guid>
    {

        [Required]
        public virtual string Name { get; set; }

        public virtual Guid CityId { get; set; }

        [ForeignKey("CityId")]
        public City CityFk { get; set; }

    }
}