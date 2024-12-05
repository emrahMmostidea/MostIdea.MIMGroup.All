using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class GetOrderCommentForEditOutput
    {
        public CreateOrEditOrderCommentDto OrderComment { get; set; }

        public string OrderOrderNo { get; set; }

    }
}