using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class GetHospitalForEditOutput
    {
        public CreateOrEditHospitalDto Hospital { get; set; }

        public string HospitalGroupName { get; set; }

    }
}