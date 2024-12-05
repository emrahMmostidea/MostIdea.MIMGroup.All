using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MostIdea.MIMGroup.Web.Areas.App.Models.TaxRates;
using MostIdea.MIMGroup.Web.Controllers;
using MostIdea.MIMGroup.Authorization;
using MostIdea.MIMGroup.B2B;
using MostIdea.MIMGroup.B2B.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MostIdea.MIMGroup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_TaxRates)]
    public class TaxRatesController : MIMGroupControllerBase
    {
        private readonly ITaxRatesAppService _taxRatesAppService;

        public TaxRatesController(ITaxRatesAppService taxRatesAppService)
        {
            _taxRatesAppService = taxRatesAppService;

        }

        public ActionResult Index()
        {
            var model = new TaxRatesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_TaxRates_Create, AppPermissions.Pages_TaxRates_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(Guid? id)
        {
            GetTaxRateForEditOutput getTaxRateForEditOutput;

            if (id.HasValue)
            {
                getTaxRateForEditOutput = await _taxRatesAppService.GetTaxRateForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getTaxRateForEditOutput = new GetTaxRateForEditOutput
                {
                    TaxRate = new CreateOrEditTaxRateDto()
                };
            }

            var viewModel = new CreateOrEditTaxRateModalViewModel()
            {
                TaxRate = getTaxRateForEditOutput.TaxRate,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

    }
}