using Abp.Application.Services.Dto;
using System;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class GetAllHospitalsForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string HospitalGroupNameFilter { get; set; }

    }
}