using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class CreateOrEditOrderCommentDto : EntityDto<Guid?>
    {

        [Required]
        public string Comment { get; set; }

        public Guid OrderId { get; set; }

    }
}