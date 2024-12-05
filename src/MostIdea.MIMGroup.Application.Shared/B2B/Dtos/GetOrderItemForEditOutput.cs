using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class GetOrderItemForEditOutput
    {
        public CreateOrEditOrderItemDto OrderItem { get; set; }

        public string ProductName { get; set; }

        public string OrderOrderNo { get; set; }

    }
}