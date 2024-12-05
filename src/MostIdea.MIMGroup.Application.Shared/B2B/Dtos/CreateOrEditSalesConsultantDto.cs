using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class CreateOrEditSalesConsultantDto : EntityDto<Guid?>
    {

        public long SalesConsultantId { get; set; }

        public long DoctorId { get; set; }

    }
}