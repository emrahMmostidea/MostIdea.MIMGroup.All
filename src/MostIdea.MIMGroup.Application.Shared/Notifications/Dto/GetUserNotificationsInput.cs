using System;
using Abp.Notifications;
using MostIdea.MIMGroup.Dto;

namespace MostIdea.MIMGroup.Notifications.Dto
{
    public class GetUserNotificationsInput : PagedInputDto
    {
        public UserNotificationState? State { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}