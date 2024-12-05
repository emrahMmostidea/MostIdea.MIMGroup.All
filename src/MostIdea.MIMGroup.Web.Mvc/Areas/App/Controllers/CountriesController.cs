using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MostIdea.MIMGroup.Web.Areas.App.Models.Countries;
using MostIdea.MIMGroup.Web.Controllers;
using MostIdea.MIMGroup.Authorization;
using MostIdea.MIMGroup.B2B;
using MostIdea.MIMGroup.B2B.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MostIdea.MIMGroup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Countries)]
    public class CountriesController : MIMGroupControllerBase
    {
        private readonly ICountriesAppService _countriesAppService;

        public CountriesController(ICountriesAppService countriesAppService)
        {
            _countriesAppService = countriesAppService;

        }

        public ActionResult Index()
        {
            var model = new CountriesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Countries_Create, AppPermissions.Pages_Countries_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(Guid? id)
        {
            GetCountryForEditOutput getCountryForEditOutput;

            if (id.HasValue)
            {
                getCountryForEditOutput = await _countriesAppService.GetCountryForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getCountryForEditOutput = new GetCountryForEditOutput
                {
                    Country = new CreateOrEditCountryDto()
                };
            }

            var viewModel = new CreateOrEditCountryModalViewModel()
            {
                Country = getCountryForEditOutput.Country,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

    }
}