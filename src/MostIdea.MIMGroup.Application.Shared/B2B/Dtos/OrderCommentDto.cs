using System;
using Abp.Application.Services.Dto;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class OrderCommentDto : EntityDto<Guid>
    {
        public string Comment { get; set; }

        public Guid OrderId { get; set; }

        public DateTime? CreationTime { get; set; }

    }
}