using MostIdea.MIMGroup.Authorization.Users;
using MostIdea.MIMGroup.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace MostIdea.MIMGroup.B2B
{
    [Table("AssistanceVsUsers")]
    public class AssistanceVsUser : FullAuditedEntity<Guid>
    {

        public virtual long AssistanceId { get; set; }

        [ForeignKey("AssistanceId")]
        public User AssistanceFk { get; set; }

        public virtual long DoctorId { get; set; }

        [ForeignKey("DoctorId")]
        public User DoctorFk { get; set; }

    }
}