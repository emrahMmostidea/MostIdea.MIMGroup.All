using Abp.Application.Services.Dto;
using System;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class GetAllOrderCommentsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string CommentFilter { get; set; }

        public string OrderOrderNoFilter { get; set; }

        public Guid? OrderId { get; set; }

    }
}