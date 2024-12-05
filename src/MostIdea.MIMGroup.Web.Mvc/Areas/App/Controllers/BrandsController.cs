using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MostIdea.MIMGroup.Web.Areas.App.Models.Brands;
using MostIdea.MIMGroup.Web.Controllers;
using MostIdea.MIMGroup.Authorization;
using MostIdea.MIMGroup.B2B;
using MostIdea.MIMGroup.B2B.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MostIdea.MIMGroup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Brands)]
    public class BrandsController : MIMGroupControllerBase
    {
        private readonly IBrandsAppService _brandsAppService;

        public BrandsController(IBrandsAppService brandsAppService)
        {
            _brandsAppService = brandsAppService;

        }

        public ActionResult Index()
        {
            var model = new BrandsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Brands_Create, AppPermissions.Pages_Brands_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(Guid? id)
        {
            GetBrandForEditOutput getBrandForEditOutput;

            if (id.HasValue)
            {
                getBrandForEditOutput = await _brandsAppService.GetBrandForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getBrandForEditOutput = new GetBrandForEditOutput
                {
                    Brand = new CreateOrEditBrandDto()
                };
            }

            var viewModel = new CreateOrEditBrandModalViewModel()
            {
                Brand = getBrandForEditOutput.Brand,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

    }
}