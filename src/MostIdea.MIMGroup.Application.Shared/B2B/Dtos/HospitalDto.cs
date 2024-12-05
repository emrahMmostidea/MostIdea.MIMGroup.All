using System;
using Abp.Application.Services.Dto;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class HospitalDto : EntityDto<Guid>
    {
        public string Name { get; set; }

        public Guid HospitalGroupId { get; set; }

        public Guid? CityId { get; set; }

        public Guid? DistrictId { get; set; }

        public string HospitalGroupName { get; set; }    

        public AddressInformationDto AddressInformation { get; set; }

    }
}