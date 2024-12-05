using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class CreateOrEditWarehouseDto : EntityDto<Guid?>
    {

        [Required]
        [StringLength(WarehouseConsts.MaxNameLength, MinimumLength = WarehouseConsts.MinNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(WarehouseConsts.MaxCoordinateLength, MinimumLength = WarehouseConsts.MinCoordinateLength)]
        public string Coordinate { get; set; }

        public Guid DistrictId { get; set; }

    }
}