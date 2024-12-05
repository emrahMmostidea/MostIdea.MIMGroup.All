using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MostIdea.MIMGroup.Web.Areas.App.Models.Warehouses;
using MostIdea.MIMGroup.Web.Controllers;
using MostIdea.MIMGroup.Authorization;
using MostIdea.MIMGroup.B2B;
using MostIdea.MIMGroup.B2B.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MostIdea.MIMGroup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Warehouses)]
    public class WarehousesController : MIMGroupControllerBase
    {
        private readonly IWarehousesAppService _warehousesAppService;

        public WarehousesController(IWarehousesAppService warehousesAppService)
        {
            _warehousesAppService = warehousesAppService;

        }

        public ActionResult Index()
        {
            var model = new WarehousesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Warehouses_Create, AppPermissions.Pages_Warehouses_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(Guid? id)
        {
            GetWarehouseForEditOutput getWarehouseForEditOutput;

            if (id.HasValue)
            {
                getWarehouseForEditOutput = await _warehousesAppService.GetWarehouseForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getWarehouseForEditOutput = new GetWarehouseForEditOutput
                {
                    Warehouse = new CreateOrEditWarehouseDto()
                };
            }

            var viewModel = new CreateOrEditWarehouseModalViewModel()
            {
                Warehouse = getWarehouseForEditOutput.Warehouse,
                DistrictName = getWarehouseForEditOutput.DistrictName,
                WarehouseDistrictList = await _warehousesAppService.GetAllDistrictForTableDropdown(),

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

    }
}