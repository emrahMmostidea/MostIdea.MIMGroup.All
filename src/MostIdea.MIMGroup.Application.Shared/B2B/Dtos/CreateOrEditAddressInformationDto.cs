using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class CreateOrEditAddressInformationDto : EntityDto<Guid?>
    {

        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(AddressInformationConsts.MaxAddressLength, MinimumLength = AddressInformationConsts.MinAddressLength)]
        public string Address { get; set; }

        [Required]
        [StringLength(AddressInformationConsts.MaxPhoneLength, MinimumLength = AddressInformationConsts.MinPhoneLength)]
        public string Phone { get; set; }

        public Guid?  HospitalId { get; set; }

    }
}