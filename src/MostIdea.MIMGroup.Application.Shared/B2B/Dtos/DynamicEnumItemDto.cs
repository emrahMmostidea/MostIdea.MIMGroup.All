using System;
using Abp.Application.Services.Dto;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class DynamicEnumItemDto : EntityDto<Guid>
    {
        public string EnumValue { get; set; }

        public Guid ParentId { get; set; }

        public bool IsAuthRestriction { get; set; }

        public string AuthorizedUsers { get; set; }
        public int Order { get; set; }

        public Guid DynamicEnumId { get; set; }

    }
}