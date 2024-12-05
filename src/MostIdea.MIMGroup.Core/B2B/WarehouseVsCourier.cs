using MostIdea.MIMGroup.Authorization.Users;
using MostIdea.MIMGroup.B2B;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace MostIdea.MIMGroup.B2B
{
    [Table("WarehouseVsCouriers")]
    public class WarehouseVsCourier : FullAuditedEntity<Guid>
    {

        public virtual long CourierId { get; set; }

        [ForeignKey("CourierId")]
        public User CourierFk { get; set; }

        public virtual Guid WarehouseId { get; set; }

        [ForeignKey("WarehouseId")]
        public Warehouse WarehouseFk { get; set; }

    }
}