using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MostIdea.MIMGroup.Web.Areas.App.Models.Orders;
using MostIdea.MIMGroup.Web.Controllers;
using MostIdea.MIMGroup.Authorization;
using MostIdea.MIMGroup.B2B;
using MostIdea.MIMGroup.B2B.Dtos;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Notifications;
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using MostIdea.MIMGroup.Authorization.Users;

namespace MostIdea.MIMGroup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Orders)]
    public class OrdersController : MIMGroupControllerBase
    {
        private readonly IOrdersAppService _ordersAppService;
        private readonly IRepository<Order, Guid> _orderRepository;
        private readonly IHospitalsAppService _hospitalsAppService;
        private readonly INotificationPublisher _notificationPublisher;
        private readonly UserManager _userManager;


        public OrdersController(IOrdersAppService ordersAppService, IRepository<Order, Guid> orderRepository, IHospitalsAppService hospitalsAppService, INotificationPublisher notificationPublisher, UserManager userManager)
        {
            _ordersAppService = ordersAppService;
            _orderRepository = orderRepository;
            _hospitalsAppService = hospitalsAppService;
            _notificationPublisher = notificationPublisher;
            _userManager = userManager;
        }


        public async Task<ActionResult> Index()
        {
            var user =  await _userManager.GetUserAsync(AbpSession.ToUserIdentifier());
            if ((await _hospitalsAppService.GetSelectedHospital()) == null &&  (!await _userManager.IsInRoleAsync(user, "Admin")))
            {
                await _notificationPublisher.PublishAsync("SelectHospital", new MessageNotificationData("Lütfen işlem yapmak istediğiniz hastaneyi seçiniz."), 
                    severity: NotificationSeverity.Warn,
                    userIds: new[] { AbpSession.ToUserIdentifier()  });
                return RedirectToAction("Index", "Home");
            }

            var model = new OrdersViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Orders_Create, AppPermissions.Pages_Orders_Edit)]
        public async Task<ActionResult> Detail(Guid? id)
        {
            var user = _userManager.GetUser(AbpSession.ToUserIdentifier());
            if ((await _hospitalsAppService.GetSelectedHospital()) == null && id.HasValue)
            {
                await _notificationPublisher.PublishAsync("SelectHospital", new MessageNotificationData("Lütfen işlem yapmak istediğiniz hastaneyi seçiniz."),
                    severity: NotificationSeverity.Warn,
                    userIds: new[] { AbpSession.ToUserIdentifier() });
                return RedirectToAction("Index", "Orders");
            }

            var output = new OrderDto();
            if (id.HasValue)
            {
                output = ObjectMapper.Map(await _orderRepository.FirstOrDefaultAsync(id.Value), new OrderDto());
            }
            else
            {
                output.OrderNo = (await _orderRepository.GetAll().CountAsync() + 1).ToString("D5");
            }

            return View("_CreateOrEditModal", output);
        }

    }
}