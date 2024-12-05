using System;
using Abp.Application.Services.Dto;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class AddressInformationDto : EntityDto<Guid>
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public bool IsPrimary { get; set; }

    }
}