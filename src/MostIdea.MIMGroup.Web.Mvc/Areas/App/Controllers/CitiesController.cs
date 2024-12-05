using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MostIdea.MIMGroup.Web.Areas.App.Models.Cities;
using MostIdea.MIMGroup.Web.Controllers;
using MostIdea.MIMGroup.Authorization;
using MostIdea.MIMGroup.B2B;
using MostIdea.MIMGroup.B2B.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MostIdea.MIMGroup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Cities)]
    public class CitiesController : MIMGroupControllerBase
    {
        private readonly ICitiesAppService _citiesAppService;

        public CitiesController(ICitiesAppService citiesAppService)
        {
            _citiesAppService = citiesAppService;

        }

        public ActionResult Index()
        {
            var model = new CitiesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Cities_Create, AppPermissions.Pages_Cities_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(Guid? id)
        {
            GetCityForEditOutput getCityForEditOutput;

            if (id.HasValue)
            {
                getCityForEditOutput = await _citiesAppService.GetCityForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getCityForEditOutput = new GetCityForEditOutput
                {
                    City = new CreateOrEditCityDto()
                };
            }

            var viewModel = new CreateOrEditCityModalViewModel()
            {
                City = getCityForEditOutput.City,
                CountryName = getCityForEditOutput.CountryName,
                CityCountryList = await _citiesAppService.GetAllCountryForTableDropdown(),

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

    }
}