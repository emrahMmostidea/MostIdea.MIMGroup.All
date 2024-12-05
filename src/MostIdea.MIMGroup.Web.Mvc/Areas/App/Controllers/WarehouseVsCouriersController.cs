using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MostIdea.MIMGroup.Web.Areas.App.Models.WarehouseVsCouriers;
using MostIdea.MIMGroup.Web.Controllers;
using MostIdea.MIMGroup.Authorization;
using MostIdea.MIMGroup.B2B;
using MostIdea.MIMGroup.B2B.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MostIdea.MIMGroup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_WarehouseVsCouriers)]
    public class WarehouseVsCouriersController : MIMGroupControllerBase
    {
        private readonly IWarehouseVsCouriersAppService _warehouseVsCouriersAppService;

        public WarehouseVsCouriersController(IWarehouseVsCouriersAppService warehouseVsCouriersAppService)
        {
            _warehouseVsCouriersAppService = warehouseVsCouriersAppService;

        }

        public ActionResult Index()
        {
            var model = new WarehouseVsCouriersViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_WarehouseVsCouriers_Create, AppPermissions.Pages_WarehouseVsCouriers_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(Guid? id)
        {
            GetWarehouseVsCourierForEditOutput getWarehouseVsCourierForEditOutput;

            if (id.HasValue)
            {
                getWarehouseVsCourierForEditOutput = await _warehouseVsCouriersAppService.GetWarehouseVsCourierForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getWarehouseVsCourierForEditOutput = new GetWarehouseVsCourierForEditOutput
                {
                    WarehouseVsCourier = new CreateOrEditWarehouseVsCourierDto()
                };
            }

            var viewModel = new CreateOrEditWarehouseVsCourierModalViewModel()
            {
                WarehouseVsCourier = getWarehouseVsCourierForEditOutput.WarehouseVsCourier,
                UserName = getWarehouseVsCourierForEditOutput.UserName,
                WarehouseName = getWarehouseVsCourierForEditOutput.WarehouseName,
                WarehouseVsCourierUserList = await _warehouseVsCouriersAppService.GetAllUserForTableDropdown(),
                WarehouseVsCourierWarehouseList = await _warehouseVsCouriersAppService.GetAllWarehouseForTableDropdown(),

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

    }
}