using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class GetDynamicEnumItemForEditOutput
    {
        public CreateOrEditDynamicEnumItemDto DynamicEnumItem { get; set; }

        public string DynamicEnumName { get; set; }

    }
}