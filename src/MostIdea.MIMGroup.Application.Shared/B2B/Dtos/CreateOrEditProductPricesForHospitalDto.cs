using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class CreateOrEditProductPricesForHospitalDto : EntityDto<Guid?>
    {

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal Price { get; set; }

        public Guid? ProductId { get; set; }

        public Guid? ProductCategoryId { get; set; }

    }
}