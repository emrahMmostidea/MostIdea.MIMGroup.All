using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace MostIdea.MIMGroup.B2B
{
    [Table("AddressInformations")]
    public class AddressInformation : FullAuditedEntity<Guid>
    {

        [Required]
        public virtual string Name { get; set; }

        [Required]
        [StringLength(AddressInformationConsts.MaxAddressLength, MinimumLength = AddressInformationConsts.MinAddressLength)]
        public virtual string Address { get; set; }

        [Required]
        [StringLength(AddressInformationConsts.MaxPhoneLength, MinimumLength = AddressInformationConsts.MinPhoneLength)]
        public virtual string Phone { get; set; }

        public virtual bool IsPrimary { get; set; }

        public virtual Guid HospitalId { get; set; }

        [ForeignKey("HospitalId")]
        public Hospital HospitalFk { get; set; }

    }
}