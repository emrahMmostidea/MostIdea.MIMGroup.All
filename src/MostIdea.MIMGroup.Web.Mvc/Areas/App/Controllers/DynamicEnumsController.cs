using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MostIdea.MIMGroup.Web.Areas.App.Models.DynamicEnums;
using MostIdea.MIMGroup.Web.Controllers;
using MostIdea.MIMGroup.Authorization;
using MostIdea.MIMGroup.B2B;
using MostIdea.MIMGroup.B2B.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MostIdea.MIMGroup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_DynamicEnums)]
    public class DynamicEnumsController : MIMGroupControllerBase
    {
        private readonly IDynamicEnumsAppService _dynamicEnumsAppService;

        public DynamicEnumsController(IDynamicEnumsAppService dynamicEnumsAppService)
        {
            _dynamicEnumsAppService = dynamicEnumsAppService;

        }

        public ActionResult Index()
        {
            var model = new DynamicEnumsViewModel
            {
                FilterText = "",
                
                
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_DynamicEnums_Create, AppPermissions.Pages_DynamicEnums_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(Guid? id)
        {
            GetDynamicEnumForEditOutput getDynamicEnumForEditOutput;

            if (id.HasValue)
            {
                getDynamicEnumForEditOutput = await _dynamicEnumsAppService.GetDynamicEnumForEdit(new EntityDto<Guid> { Id = (Guid)id });
                getDynamicEnumForEditOutput.Enums = _dynamicEnumsAppService.GetEnumFiles();
            }
            else
            {
                getDynamicEnumForEditOutput = new GetDynamicEnumForEditOutput
                {
                    DynamicEnum = new CreateOrEditDynamicEnumDto(),
                    Enums = _dynamicEnumsAppService.GetEnumFiles()
                };
            }

            var viewModel = new CreateOrEditDynamicEnumModalViewModel()
            {
                DynamicEnum = getDynamicEnumForEditOutput.DynamicEnum,
                Enums = getDynamicEnumForEditOutput.Enums
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

    }
}