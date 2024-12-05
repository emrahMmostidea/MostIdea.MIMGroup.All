using MostIdea.MIMGroup.B2B;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using MostIdea.MIMGroup.Storage;

namespace MostIdea.MIMGroup.B2B
{
    [Table("Hospitals")]
    public class Hospital : FullAuditedEntity<Guid>
    {
        public Hospital()
        {
            
        }

        [Required]
        public virtual string Name { get; set; }

        public virtual string TaxAdministration { get; set; }

        public virtual string TaxNumber { get; set; }
 
        public virtual string Coordinate { get; set; }

        public virtual Guid HospitalGroupId { get; set; }

        public virtual bool CanBeUseConsigmentOrder { get; set; }
        
        public virtual string  Website { get; set; }

        public virtual Guid? LogoId { get; set; }

        [ForeignKey("LogoId")]
        public BinaryObject Logo { get; set; }

        [ForeignKey("HospitalGroupId")]
        public HospitalGroup HospitalGroupFk { get; set; }

        public List<AddressInformation> AddresInformations { get; set; }

    }
}