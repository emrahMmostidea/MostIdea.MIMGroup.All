using MostIdea.MIMGroup.Authorization.Users;
using MostIdea.MIMGroup.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace MostIdea.MIMGroup.B2B
{
    [Table("SalesConsultants")]
    public class SalesConsultant : FullAuditedEntity<Guid>
    {

        public virtual long SalesConsultantId { get; set; }

        [ForeignKey("SalesConsultantId")]
        public User SalesConsultantFk { get; set; }

        public virtual long DoctorId { get; set; }

        [ForeignKey("DoctorId")]
        public User DoctorFk { get; set; }

    }
}