using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class HospitalGroupDto : EntityDto<Guid>
    {
        public string Name { get; set; }

        public List<HospitalDto> Hospitals { get; set; }

    }
}