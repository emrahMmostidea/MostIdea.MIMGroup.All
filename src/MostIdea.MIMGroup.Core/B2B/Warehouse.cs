using MostIdea.MIMGroup.B2B;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace MostIdea.MIMGroup.B2B
{
    [Table("Warehouses")]
    public class Warehouse : FullAuditedEntity<Guid>
    {

        [Required]
        [StringLength(WarehouseConsts.MaxNameLength, MinimumLength = WarehouseConsts.MinNameLength)]
        public virtual string Name { get; set; }

        [Required]
        [StringLength(WarehouseConsts.MaxCoordinateLength, MinimumLength = WarehouseConsts.MinCoordinateLength)]
        public virtual string Coordinate { get; set; }

        public virtual Guid DistrictId { get; set; }

        [ForeignKey("DistrictId")]
        public District DistrictFk { get; set; }

    }
}