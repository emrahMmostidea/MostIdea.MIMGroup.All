using Abp.Application.Services.Dto;
using System;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class GetAllWarehousesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string CoordinateFilter { get; set; }

        public string DistrictNameFilter { get; set; }

    }
}