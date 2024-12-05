using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class CreateOrEditHospitalDto : EntityDto<Guid?>
    {
        public CreateOrEditHospitalDto()
        {
            Coordinate = "";
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public string TaxAdministration { get; set; }

        [Required]
        public string TaxNumber { get; set; }
         
        public string Coordinate { get; set; }

        public string Website { get; set; }

        public Guid HospitalGroupId { get; set; }

    }
}