using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class GetAssistanceVsUserForEditOutput
    {
        public CreateOrEditAssistanceVsUserDto AssistanceVsUser { get; set; }

        public string UserName { get; set; }

        public string UserName2 { get; set; }

    }
}