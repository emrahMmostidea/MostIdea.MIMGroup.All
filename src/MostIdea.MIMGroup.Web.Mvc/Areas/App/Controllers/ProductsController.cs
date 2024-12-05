using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MostIdea.MIMGroup.Web.Areas.App.Models.Products;
using MostIdea.MIMGroup.Web.Controllers;
using MostIdea.MIMGroup.Authorization;
using MostIdea.MIMGroup.B2B;
using MostIdea.MIMGroup.B2B.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MostIdea.MIMGroup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Products)]
    public class ProductsController : MIMGroupControllerBase
    {
        private readonly IProductsAppService _productsAppService;

        public ProductsController(IProductsAppService productsAppService)
        {
            _productsAppService = productsAppService;

        }

        public ActionResult Index()
        {
            var model = new ProductsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Products_Create, AppPermissions.Pages_Products_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(Guid? id)
        {
            GetProductForEditOutput getProductForEditOutput;

            if (id.HasValue)
            {
                getProductForEditOutput = await _productsAppService.GetProductForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getProductForEditOutput = new GetProductForEditOutput
                {
                    Product = new CreateOrEditProductDto()
                };
            }

            var viewModel = new CreateOrEditProductModalViewModel()
            {
                Product = getProductForEditOutput.Product,
                ProductCategoryName = getProductForEditOutput.ProductCategoryName,
                TaxRateName = getProductForEditOutput.TaxRateName,
                ProductProductCategoryList = await _productsAppService.GetAllProductCategoryForTableDropdown(),
                ProductTaxRateList = await _productsAppService.GetAllTaxRateForTableDropdown(),

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewProductModal(Guid id)
        {
            var getProductForViewDto = await _productsAppService.GetProductForView(id);

            var model = new ProductViewModel()
            {
                Product = getProductForViewDto.Product
                ,
                ProductCategoryName = getProductForViewDto.ProductCategoryName

                ,
                TaxRateName = getProductForViewDto.TaxRateName

            };

            return PartialView("_ViewProductModal", model);
        }

    }
}