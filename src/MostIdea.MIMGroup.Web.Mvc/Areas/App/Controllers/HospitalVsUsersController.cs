using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MostIdea.MIMGroup.Web.Areas.App.Models.HospitalVsUsers;
using MostIdea.MIMGroup.Web.Controllers;
using MostIdea.MIMGroup.Authorization;
using MostIdea.MIMGroup.B2B;
using MostIdea.MIMGroup.B2B.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MostIdea.MIMGroup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_HospitalVsUsers)]
    public class HospitalVsUsersController : MIMGroupControllerBase
    {
        private readonly IHospitalVsUsersAppService _hospitalVsUsersAppService;

        public HospitalVsUsersController(IHospitalVsUsersAppService hospitalVsUsersAppService)
        {
            _hospitalVsUsersAppService = hospitalVsUsersAppService;

        }

        public ActionResult Index(Guid? HospitalId)
        {
            var model = new HospitalVsUsersViewModel
            {
                FilterText = "",
                HospitalId = HospitalId.Value
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_HospitalVsUsers_Create, AppPermissions.Pages_HospitalVsUsers_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(Guid? id)
        {
            GetHospitalVsUserForEditOutput getHospitalVsUserForEditOutput;

            if (id.HasValue)
            {
                getHospitalVsUserForEditOutput = await _hospitalVsUsersAppService.GetHospitalVsUserForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getHospitalVsUserForEditOutput = new GetHospitalVsUserForEditOutput
                {
                    HospitalVsUser = new CreateOrEditHospitalVsUserDto()
                };
            }

            var viewModel = new CreateOrEditHospitalVsUserModalViewModel()
            {
                HospitalVsUser = getHospitalVsUserForEditOutput.HospitalVsUser,
                HospitalName = getHospitalVsUserForEditOutput.HospitalName,
                UserName = getHospitalVsUserForEditOutput.UserName,
                HospitalVsUserHospitalList = await _hospitalVsUsersAppService.GetAllHospitalForTableDropdown(),
                HospitalVsUserUserList = await _hospitalVsUsersAppService.GetAllUserForTableDropdown(),

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

    }
}