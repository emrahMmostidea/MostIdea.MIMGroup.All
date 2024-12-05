using System;
using Abp.Application.Services.Dto;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class CountryDto : EntityDto<Guid>
    {
        public string Name { get; set; }

    }
}