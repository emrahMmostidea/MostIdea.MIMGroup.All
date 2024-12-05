using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class GetDynamicEnumForEditOutput
    {
        public CreateOrEditDynamicEnumDto DynamicEnum { get; set; }
        public List<SelectionDto> Enums { get; set; }
    }
}