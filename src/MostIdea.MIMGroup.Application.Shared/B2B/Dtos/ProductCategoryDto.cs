using System;
using Abp.Application.Services.Dto;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class ProductCategoryDto : EntityDto<Guid>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public Guid? ProductCategoryId { get; set; }

        public Guid BrandId { get; set; }

        public int ChildCount { get; set; }

    }
}