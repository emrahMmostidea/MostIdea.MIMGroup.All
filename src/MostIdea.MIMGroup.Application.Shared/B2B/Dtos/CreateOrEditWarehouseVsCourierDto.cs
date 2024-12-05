using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class CreateOrEditWarehouseVsCourierDto : EntityDto<Guid?>
    {

        public long CourierId { get; set; }

        public Guid WarehouseId { get; set; }

    }
}