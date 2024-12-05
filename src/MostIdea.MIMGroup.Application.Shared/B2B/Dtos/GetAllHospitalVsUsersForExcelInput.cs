using Abp.Application.Services.Dto;
using System;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class GetAllHospitalVsUsersForExcelInput
    {
        public string Filter { get; set; }

        public string HospitalNameFilter { get; set; }

        public string UserNameFilter { get; set; }

    }
}