using System;
using Abp.Application.Services.Dto;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class WarehouseDto : EntityDto<Guid>
    {
        public string Name { get; set; }

        public string Coordinate { get; set; }

        public Guid DistrictId { get; set; }

    }
}