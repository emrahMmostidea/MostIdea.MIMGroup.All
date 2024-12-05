using Abp.Application.Services.Dto;
using System;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class GetAllHospitalsInput : PagedAndSortedResultRequestDto
    {
        public GetAllHospitalsInput()
        {
            HospitalTypeEnum = HospitalTypeEnum.Hospital;
        }

        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string HospitalGroupNameFilter { get; set; }

        public HospitalTypeEnum HospitalTypeEnum { get; set; }
    }
}