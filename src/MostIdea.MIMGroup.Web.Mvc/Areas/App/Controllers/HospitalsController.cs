using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MostIdea.MIMGroup.Authorization;
using MostIdea.MIMGroup.B2B;
using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.Web.Areas.App.Models.Hospitals;
using MostIdea.MIMGroup.Web.Controllers;
using MostIdea.MIMGroup.Web.DevExpress;
using System;
using System.Linq;
using System.Threading.Tasks; 

namespace MostIdea.MIMGroup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Hospitals)]
    public class HospitalsController : MIMGroupControllerBase
    {
        private readonly IHospitalsAppService _hospitalsAppService;
        private readonly IRepository<Hospital, Guid> _hospitalRepository;
        private readonly IRepository<HospitalVsUser, Guid> _hospitalVsUserRepository;

        public HospitalsController(IHospitalsAppService hospitalsAppService, IRepository<Hospital, Guid> hospitalRepository, IRepository<HospitalVsUser, Guid> hospitalVsUserRepository)
        {
            _hospitalsAppService = hospitalsAppService;
            _hospitalRepository = hospitalRepository;
            _hospitalVsUserRepository = hospitalVsUserRepository;
        }

        public ActionResult Index()
        {
            var model = new HospitalsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Hospitals_Create, AppPermissions.Pages_Hospitals_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(Guid? id)
        {
            GetHospitalForEditOutput getHospitalForEditOutput;

            if (id.HasValue)
            {
                getHospitalForEditOutput = await _hospitalsAppService.GetHospitalForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getHospitalForEditOutput = new GetHospitalForEditOutput
                {
                    Hospital = new CreateOrEditHospitalDto()
                };
            }

            var viewModel = new CreateOrEditHospitalModalViewModel()
            {
                Hospital = getHospitalForEditOutput.Hospital,
                HospitalGroupName = getHospitalForEditOutput.HospitalGroupName,
                HospitalHospitalGroupList = await _hospitalsAppService.GetAllHospitalGroupForTableDropdown(),
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewHospitalModal(Guid id)
        {
            var getHospitalForViewDto = await _hospitalsAppService.GetHospitalForView(id);

            var model = new HospitalViewModel()
            {
                Hospital = getHospitalForViewDto.Hospital
                ,
                HospitalGroupName = getHospitalForViewDto.HospitalGroupName
            };

            return PartialView("_ViewHospitalModal", model);
        }


        [HttpGet("hospitals/SetId")]
        public void SetHospitalId(Guid hospitalId)
        {
            // TODO: Global HospitalId tanımla
        }
    }
}