using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class GetWarehouseForEditOutput
    {
        public CreateOrEditWarehouseDto Warehouse { get; set; }

        public string DistrictName { get; set; }

    }
}