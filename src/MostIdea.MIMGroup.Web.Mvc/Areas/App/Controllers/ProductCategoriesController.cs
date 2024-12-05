using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MostIdea.MIMGroup.Web.Areas.App.Models.ProductCategories;
using MostIdea.MIMGroup.Web.Controllers;
using MostIdea.MIMGroup.Authorization;
using MostIdea.MIMGroup.B2B;
using MostIdea.MIMGroup.B2B.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MostIdea.MIMGroup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_ProductCategories)]
    public class ProductCategoriesController : MIMGroupControllerBase
    {
        private readonly IProductCategoriesAppService _productCategoriesAppService;

        public ProductCategoriesController(IProductCategoriesAppService productCategoriesAppService)
        {
            _productCategoriesAppService = productCategoriesAppService;

        }

        public ActionResult Index()
        {
            var model = new ProductCategoriesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_ProductCategories_Create, AppPermissions.Pages_ProductCategories_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(Guid? id)
        {
            GetProductCategoryForEditOutput getProductCategoryForEditOutput;

            if (id.HasValue)
            {
                getProductCategoryForEditOutput = await _productCategoriesAppService.GetProductCategoryForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getProductCategoryForEditOutput = new GetProductCategoryForEditOutput
                {
                    ProductCategory = new CreateOrEditProductCategoryDto()
                };
            }

            var viewModel = new CreateOrEditProductCategoryModalViewModel()
            {
                ProductCategory = getProductCategoryForEditOutput.ProductCategory,
                ProductCategoryName = getProductCategoryForEditOutput.ProductCategoryName,
                BrandName = getProductCategoryForEditOutput.BrandName,
                ProductCategoryProductCategoryList = await _productCategoriesAppService.GetAllProductCategoryForTableDropdown(),
                ProductCategoryBrandList = await _productCategoriesAppService.GetAllBrandForTableDropdown(),

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

    }
}