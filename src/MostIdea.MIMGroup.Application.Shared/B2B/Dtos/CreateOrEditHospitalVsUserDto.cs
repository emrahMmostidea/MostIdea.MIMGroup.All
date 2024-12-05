using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class CreateOrEditHospitalVsUserDto : EntityDto<Guid?>
    {

        public Guid HospitalId { get; set; }

        public long UserId { get; set; }

    }
}