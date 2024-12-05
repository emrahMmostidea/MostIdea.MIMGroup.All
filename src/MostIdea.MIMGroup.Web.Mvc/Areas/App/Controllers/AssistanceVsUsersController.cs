using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MostIdea.MIMGroup.Web.Areas.App.Models.AssistanceVsUsers;
using MostIdea.MIMGroup.Web.Controllers;
using MostIdea.MIMGroup.Authorization;
using MostIdea.MIMGroup.B2B;
using MostIdea.MIMGroup.B2B.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MostIdea.MIMGroup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_AssistanceVsUsers)]
    public class AssistanceVsUsersController : MIMGroupControllerBase
    {
        private readonly IAssistanceVsUsersAppService _assistanceVsUsersAppService;

        public AssistanceVsUsersController(IAssistanceVsUsersAppService assistanceVsUsersAppService)
        {
            _assistanceVsUsersAppService = assistanceVsUsersAppService;

        }

        public ActionResult Index(long doctorId)
        {
            var model = new AssistanceVsUsersViewModel
            {
                FilterText = "",
                DoctorId = doctorId
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_AssistanceVsUsers_Create, AppPermissions.Pages_AssistanceVsUsers_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(Guid? id, long DoctorId)
        {
            GetAssistanceVsUserForEditOutput getAssistanceVsUserForEditOutput;

            if (id.HasValue)
            {
                getAssistanceVsUserForEditOutput = await _assistanceVsUsersAppService.GetAssistanceVsUserForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getAssistanceVsUserForEditOutput = new GetAssistanceVsUserForEditOutput
                {
                    AssistanceVsUser = new CreateOrEditAssistanceVsUserDto()
                };
            }

            getAssistanceVsUserForEditOutput.AssistanceVsUser.DoctorId = DoctorId;

            var viewModel = new CreateOrEditAssistanceVsUserModalViewModel()
            {
                AssistanceVsUser = getAssistanceVsUserForEditOutput.AssistanceVsUser,
                UserName = getAssistanceVsUserForEditOutput.UserName,
                UserName2 = getAssistanceVsUserForEditOutput.UserName2,
                AssistanceVsUserUserList = await _assistanceVsUsersAppService.GetAllUserForTableDropdown(),
                
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_AssistanceVsUsers_Create, AppPermissions.Pages_AssistanceVsUsers_Edit)]
        public PartialViewResult UserLookupTableModal(long? id, string displayName)
        {
            var viewModel = new AssistanceVsUserUserLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_AssistanceVsUserUserLookupTableModal", viewModel);
        }

    }
}