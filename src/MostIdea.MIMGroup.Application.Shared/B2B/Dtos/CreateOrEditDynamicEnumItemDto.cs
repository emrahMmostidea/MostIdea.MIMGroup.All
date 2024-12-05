using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class CreateOrEditDynamicEnumItemDto : EntityDto<Guid?>
    {

        [Required]
        public string EnumValue { get; set; }

        public Guid ParentId { get; set; }

        public bool IsAuthRestriction { get; set; }

        [Required]
        public string AuthorizedUsers { get; set; }

        public int Order { get; set; }

        public Guid DynamicEnumId { get; set; }

    }
}