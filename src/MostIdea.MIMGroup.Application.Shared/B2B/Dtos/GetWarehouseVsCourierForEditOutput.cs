using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class GetWarehouseVsCourierForEditOutput
    {
        public CreateOrEditWarehouseVsCourierDto WarehouseVsCourier { get; set; }

        public string UserName { get; set; }

        public string WarehouseName { get; set; }

    }
}