using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MostIdea.MIMGroup.Web.Areas.App.Models.SalesConsultants;
using MostIdea.MIMGroup.Web.Controllers;
using MostIdea.MIMGroup.Authorization;
using MostIdea.MIMGroup.B2B;
using MostIdea.MIMGroup.B2B.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MostIdea.MIMGroup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_SalesConsultants)]
    public class SalesConsultantsController : MIMGroupControllerBase
    {
        private readonly ISalesConsultantsAppService _salesConsultantsAppService;

        public SalesConsultantsController(ISalesConsultantsAppService salesConsultantsAppService)
        {
            _salesConsultantsAppService = salesConsultantsAppService;

        }

        public ActionResult Index(long SalesConsultantId)
        {
            var model = new SalesConsultantsViewModel
            {
                FilterText = "",
                SalesConsultantId = SalesConsultantId
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_SalesConsultants_Create, AppPermissions.Pages_SalesConsultants_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(Guid? id, long SalesConsultantId)
        {
            GetSalesConsultantForEditOutput getSalesConsultantForEditOutput;

            if (id.HasValue)
            {
                getSalesConsultantForEditOutput = await _salesConsultantsAppService.GetSalesConsultantForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getSalesConsultantForEditOutput = new GetSalesConsultantForEditOutput
                {
                    SalesConsultant = new CreateOrEditSalesConsultantDto()
                };
            }

            getSalesConsultantForEditOutput.SalesConsultant.SalesConsultantId = SalesConsultantId;

            var viewModel = new CreateOrEditSalesConsultantModalViewModel()
            {
                SalesConsultant = getSalesConsultantForEditOutput.SalesConsultant,
                UserName = getSalesConsultantForEditOutput.UserName,
                UserName2 = getSalesConsultantForEditOutput.UserName2,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewSalesConsultantModal(Guid id)
        {
            var getSalesConsultantForViewDto = await _salesConsultantsAppService.GetSalesConsultantForView(id);

            var model = new SalesConsultantViewModel()
            {
                SalesConsultant = getSalesConsultantForViewDto.SalesConsultant
                ,
                UserName = getSalesConsultantForViewDto.UserName

                ,
                UserName2 = getSalesConsultantForViewDto.UserName2

            };

            return PartialView("_ViewSalesConsultantModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_SalesConsultants_Create, AppPermissions.Pages_SalesConsultants_Edit)]
        public PartialViewResult UserLookupTableModal(long? id, string displayName)
        {
            var viewModel = new SalesConsultantUserLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_SalesConsultantUserLookupTableModal", viewModel);
        }

    }
}