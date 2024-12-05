using System;
using Abp.Application.Services.Dto;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class DynamicEnumDto : EntityDto<Guid>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string EnumFile { get; set; }

    }
}