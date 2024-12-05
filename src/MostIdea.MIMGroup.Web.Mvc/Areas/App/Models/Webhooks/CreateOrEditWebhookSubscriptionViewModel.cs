﻿using Abp.Application.Services.Dto;
using Abp.Webhooks;
using MostIdea.MIMGroup.WebHooks.Dto;

namespace MostIdea.MIMGroup.Web.Areas.App.Models.Webhooks
{
    public class CreateOrEditWebhookSubscriptionViewModel
    {
        public WebhookSubscription WebhookSubscription { get; set; }

        public ListResultDto<GetAllAvailableWebhooksOutput> AvailableWebhookEvents { get; set; }
    }
}
