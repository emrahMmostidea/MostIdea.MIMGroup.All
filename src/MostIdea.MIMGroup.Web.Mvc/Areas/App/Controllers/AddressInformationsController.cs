using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MostIdea.MIMGroup.Web.Areas.App.Models.AddressInformations;
using MostIdea.MIMGroup.Web.Controllers;
using MostIdea.MIMGroup.Authorization;
using MostIdea.MIMGroup.B2B;
using MostIdea.MIMGroup.B2B.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MostIdea.MIMGroup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_AddressInformations)]
    public class AddressInformationsController : MIMGroupControllerBase
    {
        private readonly IAddressInformationsAppService _addressInformationsAppService;

        public AddressInformationsController(IAddressInformationsAppService addressInformationsAppService)
        {
            _addressInformationsAppService = addressInformationsAppService;

        }

        public ActionResult Index(Guid? hospitalId)
        {
            var model = new AddressInformationsViewModel
            {
                FilterText = "",
                HospitalId = hospitalId.Value
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_AddressInformations_Create, AppPermissions.Pages_AddressInformations_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(Guid? id, Guid? hospitalId)
        {
            GetAddressInformationForEditOutput getAddressInformationForEditOutput;

            if (id.HasValue)
            {
                getAddressInformationForEditOutput = await _addressInformationsAppService.GetAddressInformationForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getAddressInformationForEditOutput = new GetAddressInformationForEditOutput
                {
                    AddressInformation = new CreateOrEditAddressInformationDto()
                };
            }

            if (hospitalId.HasValue)
            {
                getAddressInformationForEditOutput.AddressInformation.HospitalId = hospitalId.Value;
            }

            var viewModel = new CreateOrEditAddressInformationModalViewModel()
            {
                AddressInformation = getAddressInformationForEditOutput.AddressInformation,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

    }
}