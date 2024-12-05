using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MostIdea.MIMGroup.Web.Areas.App.Models.OrderItems;
using MostIdea.MIMGroup.Web.Controllers;
using MostIdea.MIMGroup.Authorization;
using MostIdea.MIMGroup.B2B;
using MostIdea.MIMGroup.B2B.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MostIdea.MIMGroup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_OrderItems)]
    public class OrderItemsController : MIMGroupControllerBase
    {
        private readonly IOrderItemsAppService _orderItemsAppService;

        public OrderItemsController(IOrderItemsAppService orderItemsAppService)
        {
            _orderItemsAppService = orderItemsAppService;

        }

        public ActionResult Index()
        {
            var model = new OrderItemsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_OrderItems_Create, AppPermissions.Pages_OrderItems_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(Guid? id)
        {
            GetOrderItemForEditOutput getOrderItemForEditOutput;

            if (id.HasValue)
            {
                getOrderItemForEditOutput = await _orderItemsAppService.GetOrderItemForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getOrderItemForEditOutput = new GetOrderItemForEditOutput
                {
                    OrderItem = new CreateOrEditOrderItemDto()
                };
            }

            var viewModel = new CreateOrEditOrderItemModalViewModel()
            {
                OrderItem = getOrderItemForEditOutput.OrderItem,
                ProductName = getOrderItemForEditOutput.ProductName,
                OrderOrderNo = getOrderItemForEditOutput.OrderOrderNo,
                OrderItemProductList = await _orderItemsAppService.GetAllProductForTableDropdown(),
                OrderItemOrderList = await _orderItemsAppService.GetAllOrderForTableDropdown(),

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

    }
}