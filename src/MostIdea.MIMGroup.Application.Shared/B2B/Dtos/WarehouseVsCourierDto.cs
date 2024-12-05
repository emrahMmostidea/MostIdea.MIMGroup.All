using System;
using Abp.Application.Services.Dto;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class WarehouseVsCourierDto : EntityDto<Guid>
    {

        public long CourierId { get; set; }

        public Guid WarehouseId { get; set; }

    }
}