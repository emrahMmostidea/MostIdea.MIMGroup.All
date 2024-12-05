using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.B2B.Exporting;
using MostIdea.MIMGroup.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace MostIdea.MIMGroup.B2B
{
    //[AbpAuthorize(AppPermissions.Pages_DynamicEnumItems)]
    public class DynamicEnumItemsAppService : MIMGroupAppServiceBase, IDynamicEnumItemsAppService
    {
        private readonly IRepository<DynamicEnumItem, Guid> _dynamicEnumItemRepository;
        private readonly IDynamicEnumItemsExcelExporter _dynamicEnumItemsExcelExporter;
        private readonly IRepository<DynamicEnum, Guid> _lookupDynamicEnumRepository;

        public DynamicEnumItemsAppService(IRepository<DynamicEnumItem, Guid> dynamicEnumItemRepository, IDynamicEnumItemsExcelExporter dynamicEnumItemsExcelExporter, IRepository<DynamicEnum, Guid> lookupDynamicEnumRepository)
        {
            _dynamicEnumItemRepository = dynamicEnumItemRepository;
            _dynamicEnumItemsExcelExporter = dynamicEnumItemsExcelExporter;
            _lookupDynamicEnumRepository = lookupDynamicEnumRepository;
        }

        public async Task<PagedResultDto<GetDynamicEnumItemForViewDto>> GetAll(GetAllDynamicEnumItemsInput input)
        {
            var filteredDynamicEnumItems = _dynamicEnumItemRepository.GetAll()
                        .Include(e => e.DynamicEnumFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.EnumValue.Contains(input.Filter) || e.AuthorizedUsers.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EnumValueFilter), e => e.EnumValue == input.EnumValueFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ParentIdFilter.ToString()), e => e.ParentId.ToString() == input.ParentIdFilter.ToString())
                        .WhereIf(input.IsAuthRestrictionFilter.HasValue && input.IsAuthRestrictionFilter > -1, e => (input.IsAuthRestrictionFilter == 1 && e.IsAuthRestriction) || (input.IsAuthRestrictionFilter == 0 && !e.IsAuthRestriction))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AuthorizedUsersFilter), e => e.AuthorizedUsers == input.AuthorizedUsersFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DynamicEnumNameFilter), e => e.DynamicEnumFk != null && e.DynamicEnumFk.Name == input.DynamicEnumNameFilter);

            var pagedAndFilteredDynamicEnumItems = filteredDynamicEnumItems
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var dynamicEnumItems = from o in pagedAndFilteredDynamicEnumItems
                                   join o1 in _lookupDynamicEnumRepository.GetAll() on o.DynamicEnumId equals o1.Id into j1
                                   from s1 in j1.DefaultIfEmpty()

                                   select new
                                   {
                                       o.EnumValue,
                                       o.ParentId,
                                       o.IsAuthRestriction,
                                       o.AuthorizedUsers,
                                       Id = o.Id,
                                       DynamicEnumName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                                   };

            var totalCount = await filteredDynamicEnumItems.CountAsync();

            var dbList = await dynamicEnumItems.ToListAsync();
            var results = new List<GetDynamicEnumItemForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetDynamicEnumItemForViewDto()
                {
                    DynamicEnumItem = new DynamicEnumItemDto
                    {
                        EnumValue = o.EnumValue,
                        ParentId = o.ParentId.Value,
                        IsAuthRestriction = o.IsAuthRestriction,
                        AuthorizedUsers = o.AuthorizedUsers,
                        Id = o.Id,
                    },
                    DynamicEnumName = o.DynamicEnumName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetDynamicEnumItemForViewDto>(
                totalCount,
                results
            );
        }

        public async Task<GetDynamicEnumItemForViewDto> GetDynamicEnumItemForView(Guid id)
        {
            var dynamicEnumItem = await _dynamicEnumItemRepository.GetAsync(id);

            var output = new GetDynamicEnumItemForViewDto { DynamicEnumItem = ObjectMapper.Map<DynamicEnumItemDto>(dynamicEnumItem) };

            if (output.DynamicEnumItem.DynamicEnumId != null)
            {
                var _lookupDynamicEnum = await _lookupDynamicEnumRepository.FirstOrDefaultAsync((Guid)output.DynamicEnumItem.DynamicEnumId);
                output.DynamicEnumName = _lookupDynamicEnum?.Name?.ToString();
            }

            return output;
        }

        //[AbpAuthorize(AppPermissions.Pages_DynamicEnumItems_Edit)]
        public async Task<GetDynamicEnumItemForEditOutput> GetDynamicEnumItemForEdit(EntityDto<Guid> input)
        {
            var dynamicEnumItem = await _dynamicEnumItemRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDynamicEnumItemForEditOutput { DynamicEnumItem = ObjectMapper.Map<CreateOrEditDynamicEnumItemDto>(dynamicEnumItem) };

            if (output.DynamicEnumItem.DynamicEnumId != null)
            {
                var _lookupDynamicEnum = await _lookupDynamicEnumRepository.FirstOrDefaultAsync((Guid)output.DynamicEnumItem.DynamicEnumId);
                output.DynamicEnumName = _lookupDynamicEnum?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditDynamicEnumItemDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        //[AbpAuthorize(AppPermissions.Pages_DynamicEnumItems_Create)]
        protected virtual async Task Create(CreateOrEditDynamicEnumItemDto input)
        {
            var dynamicEnumItem = ObjectMapper.Map<DynamicEnumItem>(input);

            await _dynamicEnumItemRepository.InsertAsync(dynamicEnumItem);
        }

        //[AbpAuthorize(AppPermissions.Pages_DynamicEnumItems_Edit)]
        protected virtual async Task Update(CreateOrEditDynamicEnumItemDto input)
        {
            var dynamicEnumItem = await _dynamicEnumItemRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, dynamicEnumItem);
        }

        //[AbpAuthorize(AppPermissions.Pages_DynamicEnumItems_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            await _dynamicEnumItemRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetDynamicEnumItemsToExcel(GetAllDynamicEnumItemsForExcelInput input)
        {
            var filteredDynamicEnumItems = _dynamicEnumItemRepository.GetAll()
                        .Include(e => e.DynamicEnumFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.EnumValue.Contains(input.Filter) || e.AuthorizedUsers.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EnumValueFilter), e => e.EnumValue == input.EnumValueFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ParentIdFilter.ToString()), e => e.ParentId.ToString() == input.ParentIdFilter.ToString())
                        .WhereIf(input.IsAuthRestrictionFilter.HasValue && input.IsAuthRestrictionFilter > -1, e => (input.IsAuthRestrictionFilter == 1 && e.IsAuthRestriction) || (input.IsAuthRestrictionFilter == 0 && !e.IsAuthRestriction))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AuthorizedUsersFilter), e => e.AuthorizedUsers == input.AuthorizedUsersFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DynamicEnumNameFilter), e => e.DynamicEnumFk != null && e.DynamicEnumFk.Name == input.DynamicEnumNameFilter);

            var query = (from o in filteredDynamicEnumItems
                         join o1 in _lookupDynamicEnumRepository.GetAll() on o.DynamicEnumId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetDynamicEnumItemForViewDto()
                         {
                             DynamicEnumItem = new DynamicEnumItemDto
                             {
                                 EnumValue = o.EnumValue,
                                 ParentId = o.ParentId.Value,
                                 IsAuthRestriction = o.IsAuthRestriction,
                                 AuthorizedUsers = o.AuthorizedUsers,
                                 Id = o.Id
                             },
                             DynamicEnumName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         });

            var dynamicEnumItemListDtos = await query.ToListAsync();

            return _dynamicEnumItemsExcelExporter.ExportToFile(dynamicEnumItemListDtos);
        }

        //[AbpAuthorize(AppPermissions.Pages_DynamicEnumItems)]

        public async Task<List<DynamicEnumItemDynamicEnumLookupTableDto>> GetAllDynamicEnumForTableDropdown()
        {
            return await _lookupDynamicEnumRepository.GetAll()
                .Select(dynamicEnum => new DynamicEnumItemDynamicEnumLookupTableDto
                {
                    Id = dynamicEnum.Id.ToString(),
                    DisplayName = dynamicEnum == null || dynamicEnum.Name == null ? "" : dynamicEnum.Name.ToString()
                }).ToListAsync();
        }

        //public async Task<List<HierarchicalDto>> GetHierarchical(Guid enumId)
        //{
        //    var enumItems = await _dynamicEnumItemRepository.GetAll()
        //        .Where(x => x.DynamicEnumId == enumId)
        //        .ToListAsync();

        //    var hierarchicals = new List<HierarchicalDto>();

        //    foreach (var item in enumItems)
        //    {
        //        var hierarchical = new HierarchicalDto
        //        {
        //            Id = item.Id,
        //            Label = item.Label,
        //            AuthorizedUsers = item.AuthorizedUsers,
        //            EnumValue = item.EnumValue,
        //            IsAuthRestriction = item.IsAuthRestriction,
        //            ParentId = item.ParentId.HasValue ? item.ParentId.ToString() : "-1",
        //        };

        //        hierarchicals.Add(hierarchical);
        //    }
        //    return hierarchicals;
        //}

        //public object GetHierarchical(DataSourceLoadOptions loadOptions)
        //{
        //    var output = _dynamicEnumItemRepository.GetAll();

        //    return DataSourceLoader.Load(output, loadOptions);
        //}
    }
}