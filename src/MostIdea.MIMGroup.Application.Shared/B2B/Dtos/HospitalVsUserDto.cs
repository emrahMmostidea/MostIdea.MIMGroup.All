using System;
using Abp.Application.Services.Dto;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class HospitalVsUserDto : EntityDto<Guid>
    {

        public Guid HospitalId { get; set; }

        public long UserId { get; set; }

    }
}