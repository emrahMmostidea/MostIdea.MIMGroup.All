using System;
using Abp.Application.Services.Dto;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class AssistanceVsUserDto : EntityDto<Guid>
    {

        public long AssistanceId { get; set; }

        public long DoctorId { get; set; }

    }
}