using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using MostIdea.MIMGroup.Authorization.Users;

namespace MostIdea.MIMGroup.B2B
{
    [Table("DealerVsUsers")]
    public class DealerVsUser : FullAuditedEntity<Guid>
    {

        public virtual Guid DealerId { get; set; }

        [ForeignKey("DealerId")]
        public Hospital DealerFk { get; set; }

        public virtual long UserId { get; set; }

        [ForeignKey("UserId")]
        public User UserFk { get; set; }

    }
}