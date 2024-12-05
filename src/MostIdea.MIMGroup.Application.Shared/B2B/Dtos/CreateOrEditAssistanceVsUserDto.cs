using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class CreateOrEditAssistanceVsUserDto : EntityDto<Guid?>
    {

        public long AssistanceId { get; set; }

        public long DoctorId { get; set; }

    }
}