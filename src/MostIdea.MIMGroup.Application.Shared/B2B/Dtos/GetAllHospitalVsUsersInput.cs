using Abp.Application.Services.Dto;
using System;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class GetAllHospitalVsUsersInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string HospitalNameFilter { get; set; }

        public string UserNameFilter { get; set; }

        public Guid? HospitalId { get; set; }

    }
}