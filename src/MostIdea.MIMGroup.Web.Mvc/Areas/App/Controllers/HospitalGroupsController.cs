using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MostIdea.MIMGroup.Web.Areas.App.Models.HospitalGroups;
using MostIdea.MIMGroup.Web.Controllers;
using MostIdea.MIMGroup.Authorization;
using MostIdea.MIMGroup.B2B;
using MostIdea.MIMGroup.B2B.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MostIdea.MIMGroup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_HospitalGroups)]
    public class HospitalGroupsController : MIMGroupControllerBase
    {
        private readonly IHospitalGroupsAppService _hospitalGroupsAppService;

        public HospitalGroupsController(IHospitalGroupsAppService hospitalGroupsAppService)
        {
            _hospitalGroupsAppService = hospitalGroupsAppService;

        }

        public ActionResult Index()
        {
            var model = new HospitalGroupsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_HospitalGroups_Create, AppPermissions.Pages_HospitalGroups_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(Guid? id)
        {
            GetHospitalGroupForEditOutput getHospitalGroupForEditOutput;

            if (id.HasValue)
            {
                getHospitalGroupForEditOutput = await _hospitalGroupsAppService.GetHospitalGroupForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getHospitalGroupForEditOutput = new GetHospitalGroupForEditOutput
                {
                    HospitalGroup = new CreateOrEditHospitalGroupDto()
                };
            }

            var viewModel = new CreateOrEditHospitalGroupModalViewModel()
            {
                HospitalGroup = getHospitalGroupForEditOutput.HospitalGroup,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

    }
}