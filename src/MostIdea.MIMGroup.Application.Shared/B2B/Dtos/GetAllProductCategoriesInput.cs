using Abp.Application.Services.Dto;
using System;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class GetAllProductCategoriesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public string ProductCategoryNameFilter { get; set; }

        public string BrandNameFilter { get; set; }

        public Guid? ParentId { get; set; }

    }
}