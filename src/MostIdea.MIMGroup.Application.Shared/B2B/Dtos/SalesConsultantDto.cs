using System;
using Abp.Application.Services.Dto;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class SalesConsultantDto : EntityDto<Guid>
    {

        public long SalesConsultantId { get; set; }

        public long DoctorId { get; set; }

    }
}