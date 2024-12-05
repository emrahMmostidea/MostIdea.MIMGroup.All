using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using Abp.Web.Models;
using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Mvc;
using MostIdea.MIMGroup.Authorization;
using MostIdea.MIMGroup.B2B;
using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.Web.Areas.App.Models.DynamicEnumItems;
using MostIdea.MIMGroup.Web.Controllers;
using MostIdea.MIMGroup.Web.DevExpress;
using Newtonsoft.Json;

namespace MostIdea.MIMGroup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_DynamicEnumItems)]
    public class DynamicEnumItemsController : MIMGroupControllerBase
    {
        private readonly IDynamicEnumItemsAppService _dynamicEnumItemsAppService;
        private readonly IRepository<DynamicEnumItem, Guid> _dynamicEnumItemRepository;

        public DynamicEnumItemsController(IDynamicEnumItemsAppService dynamicEnumItemsAppService, IRepository<DynamicEnumItem, Guid> dynamicEnumItemRepository)
        {
            _dynamicEnumItemsAppService = dynamicEnumItemsAppService;
            _dynamicEnumItemRepository = dynamicEnumItemRepository;
        }
        
        public ActionResult Index()
        {
            var model = new DynamicEnumItemsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_DynamicEnumItems_Create, AppPermissions.Pages_DynamicEnumItems_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(Guid? id)
        {
            GetDynamicEnumItemForEditOutput getDynamicEnumItemForEditOutput;

            if (id.HasValue)
            {
                getDynamicEnumItemForEditOutput = await _dynamicEnumItemsAppService.GetDynamicEnumItemForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getDynamicEnumItemForEditOutput = new GetDynamicEnumItemForEditOutput
                {
                    DynamicEnumItem = new CreateOrEditDynamicEnumItemDto()
                };
            }

            var viewModel = new CreateOrEditDynamicEnumItemModalViewModel
            {
                DynamicEnumItem = getDynamicEnumItemForEditOutput.DynamicEnumItem,
                DynamicEnumName = getDynamicEnumItemForEditOutput.DynamicEnumName,
                DynamicEnumItemDynamicEnumList = await _dynamicEnumItemsAppService.GetAllDynamicEnumForTableDropdown(),
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewDynamicEnumItemModal(Guid id)
        {
            var getDynamicEnumItemForViewDto = await _dynamicEnumItemsAppService.GetDynamicEnumItemForView(id);

            var model = new DynamicEnumItemViewModel
            {
                DynamicEnumItem = getDynamicEnumItemForViewDto.DynamicEnumItem,
                DynamicEnumName = getDynamicEnumItemForViewDto.DynamicEnumName
            };

            return PartialView("_ViewDynamicEnumItemModal", model);
        }


         [DontWrapResult]
        public async Task<IActionResult> GetHierarchical(DataSourceLoadOptions loadOptions)
        {
            var source = _dynamicEnumItemRepository.GetAll().Select(x => new
            {
                x.Id,
                x.ParentId,
                x.Label,
                x.DynamicEnumId,
                DynamicEnumName = x.DynamicEnumFk.Name,
                x.Order,
                x.AuthorizedUsers,
                x.EnumValue,
                x.IsAuthRestriction,
            });

            loadOptions.PrimaryKey = new[] { "Id" };
            loadOptions.PaginateViaPrimaryKey = true;
 

            return Json(await DataSourceLoader.LoadAsync(source, loadOptions));
        }

        [HttpPut]
        public async Task<IActionResult> Update(Guid key, string values)
        {
            var dynamicEnumItem = await _dynamicEnumItemRepository.GetAsync(key);
            if (dynamicEnumItem == null)
            {
                return StatusCode(409, "Record not found");
            } 

            JsonConvert.PopulateObject(values, dynamicEnumItem);

            if (!TryValidateModel(dynamicEnumItem))
                return BadRequest(ModelState.ValidationState);

            await _dynamicEnumItemRepository.UpdateAsync(dynamicEnumItem);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Insert(string values)
        {
            var data = new DynamicEnumItem();
            JsonConvert.PopulateObject(values, data);

            if (!TryValidateModel(data))
                return BadRequest(ModelState.ValidationState);

            await _dynamicEnumItemRepository.InsertAsync(data);

            return Json(data);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid key)
        {
            var data = await _dynamicEnumItemRepository.GetAsync(key);
            if (data == null)
                return StatusCode(409, "Record not found");

            await _dynamicEnumItemRepository.DeleteAsync(data);
            return Ok();
        }

    }
}