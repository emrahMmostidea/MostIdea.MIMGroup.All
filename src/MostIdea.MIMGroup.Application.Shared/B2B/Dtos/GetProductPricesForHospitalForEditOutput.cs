using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class GetProductPricesForHospitalForEditOutput
    {
        public CreateOrEditProductPricesForHospitalDto ProductPricesForHospital { get; set; }

        public string ProductName { get; set; }

        public string ProductCategoryName { get; set; }

    }
}