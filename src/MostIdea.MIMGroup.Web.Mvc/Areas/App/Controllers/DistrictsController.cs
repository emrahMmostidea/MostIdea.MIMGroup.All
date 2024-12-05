using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MostIdea.MIMGroup.Web.Areas.App.Models.Districts;
using MostIdea.MIMGroup.Web.Controllers;
using MostIdea.MIMGroup.Authorization;
using MostIdea.MIMGroup.B2B;
using MostIdea.MIMGroup.B2B.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MostIdea.MIMGroup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Districts)]
    public class DistrictsController : MIMGroupControllerBase
    {
        private readonly IDistrictsAppService _districtsAppService;

        public DistrictsController(IDistrictsAppService districtsAppService)
        {
            _districtsAppService = districtsAppService;

        }

        public ActionResult Index()
        {
            var model = new DistrictsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Districts_Create, AppPermissions.Pages_Districts_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(Guid? id)
        {
            GetDistrictForEditOutput getDistrictForEditOutput;

            if (id.HasValue)
            {
                getDistrictForEditOutput = await _districtsAppService.GetDistrictForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getDistrictForEditOutput = new GetDistrictForEditOutput
                {
                    District = new CreateOrEditDistrictDto()
                };
            }

            var viewModel = new CreateOrEditDistrictModalViewModel()
            {
                District = getDistrictForEditOutput.District,
                CityName = getDistrictForEditOutput.CityName,
                DistrictCityList = await _districtsAppService.GetAllCityForTableDropdown(),

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

    }
}