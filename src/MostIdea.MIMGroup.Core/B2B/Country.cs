using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace MostIdea.MIMGroup.B2B
{
    [Table("Countries")]
    public class Country : Entity<Guid>
    {

        [Required]
        public virtual string Name { get; set; }

    }
}