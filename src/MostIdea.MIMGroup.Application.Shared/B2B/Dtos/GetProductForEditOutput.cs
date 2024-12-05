using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class GetProductForEditOutput
    {
        public CreateOrEditProductDto Product { get; set; }

        public string ProductCategoryName { get; set; }

        public string TaxRateName { get; set; }

    }
}