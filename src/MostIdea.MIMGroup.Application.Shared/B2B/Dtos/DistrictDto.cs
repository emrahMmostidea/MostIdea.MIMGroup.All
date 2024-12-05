using System;
using Abp.Application.Services.Dto;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class DistrictDto : EntityDto<Guid>
    {
        public string Name { get; set; }

        public Guid CityId { get; set; }

    }
}