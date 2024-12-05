using MostIdea.MIMGroup.B2B;
using MostIdea.MIMGroup.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace MostIdea.MIMGroup.B2B
{
    [Table("HospitalVsUsers")]
    public class HospitalVsUser : FullAuditedEntity<Guid>
    {

        public virtual Guid HospitalId { get; set; }

        [ForeignKey("HospitalId")]
        public Hospital HospitalFk { get; set; }

        public virtual long UserId { get; set; }

        [ForeignKey("UserId")]
        public User UserFk { get; set; }

    }
}