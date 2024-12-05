using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class GetHospitalVsUserForEditOutput
    {
        public CreateOrEditHospitalVsUserDto HospitalVsUser { get; set; }

        public string HospitalName { get; set; }

        public string UserName { get; set; }

    }
}