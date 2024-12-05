using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class GetProductCategoryForEditOutput
    {
        public CreateOrEditProductCategoryDto ProductCategory { get; set; }

        public string ProductCategoryName { get; set; }

        public string BrandName { get; set; }

    }
}