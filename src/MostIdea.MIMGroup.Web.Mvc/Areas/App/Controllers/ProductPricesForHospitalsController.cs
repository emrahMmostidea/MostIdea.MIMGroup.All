using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MostIdea.MIMGroup.Web.Areas.App.Models.ProductPricesForHospitals;
using MostIdea.MIMGroup.Web.Controllers;
using MostIdea.MIMGroup.Authorization;
using MostIdea.MIMGroup.B2B;
using MostIdea.MIMGroup.B2B.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MostIdea.MIMGroup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_ProductPricesForHospitals)]
    public class ProductPricesForHospitalsController : MIMGroupControllerBase
    {
        private readonly IProductPricesForHospitalsAppService _productPricesForHospitalsAppService;

        public ProductPricesForHospitalsController(IProductPricesForHospitalsAppService productPricesForHospitalsAppService)
        {
            _productPricesForHospitalsAppService = productPricesForHospitalsAppService;

        }

        public ActionResult Index()
        {
            var model = new ProductPricesForHospitalsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_ProductPricesForHospitals_Create, AppPermissions.Pages_ProductPricesForHospitals_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(Guid? id)
        {
            GetProductPricesForHospitalForEditOutput getProductPricesForHospitalForEditOutput;

            if (id.HasValue)
            {
                getProductPricesForHospitalForEditOutput = await _productPricesForHospitalsAppService.GetProductPricesForHospitalForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getProductPricesForHospitalForEditOutput = new GetProductPricesForHospitalForEditOutput
                {
                    ProductPricesForHospital = new CreateOrEditProductPricesForHospitalDto()
                };
                getProductPricesForHospitalForEditOutput.ProductPricesForHospital.StartDate = DateTime.Now;
                getProductPricesForHospitalForEditOutput.ProductPricesForHospital.EndDate = DateTime.Now;
            }

            var viewModel = new CreateOrEditProductPricesForHospitalModalViewModel()
            {
                ProductPricesForHospital = getProductPricesForHospitalForEditOutput.ProductPricesForHospital,
                ProductName = getProductPricesForHospitalForEditOutput.ProductName,
                ProductCategoryName = getProductPricesForHospitalForEditOutput.ProductCategoryName,
                ProductPricesForHospitalProductList = await _productPricesForHospitalsAppService.GetAllProductForTableDropdown(),
                ProductPricesForHospitalProductCategoryList = await _productPricesForHospitalsAppService.GetAllProductCategoryForTableDropdown(),

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

    }
}