using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using MostIdea.MIMGroup.Authorization.Users;
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
    //[AbpAuthorize(AppPermissions.Pages_WarehouseVsCouriers)]
    public class WarehouseVsCouriersAppService : MIMGroupAppServiceBase, IWarehouseVsCouriersAppService
    {
        private readonly IRepository<WarehouseVsCourier, Guid> _warehouseVsCourierRepository;
        private readonly IWarehouseVsCouriersExcelExporter _warehouseVsCouriersExcelExporter;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IRepository<Warehouse, Guid> _lookup_warehouseRepository;

        public WarehouseVsCouriersAppService(IRepository<WarehouseVsCourier, Guid> warehouseVsCourierRepository, IWarehouseVsCouriersExcelExporter warehouseVsCouriersExcelExporter, IRepository<User, long> lookup_userRepository, IRepository<Warehouse, Guid> lookup_warehouseRepository)
        {
            _warehouseVsCourierRepository = warehouseVsCourierRepository;
            _warehouseVsCouriersExcelExporter = warehouseVsCouriersExcelExporter;
            _lookup_userRepository = lookup_userRepository;
            _lookup_warehouseRepository = lookup_warehouseRepository;
        }

        public async Task<PagedResultDto<GetWarehouseVsCourierForViewDto>> GetAll(GetAllWarehouseVsCouriersInput input)
        {
            var filteredWarehouseVsCouriers = _warehouseVsCourierRepository.GetAll()
                        .Include(e => e.CourierFk)
                        .Include(e => e.WarehouseFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.CourierFk != null && e.CourierFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.WarehouseNameFilter), e => e.WarehouseFk != null && e.WarehouseFk.Name == input.WarehouseNameFilter);

            var pagedAndFilteredWarehouseVsCouriers = filteredWarehouseVsCouriers
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var warehouseVsCouriers = from o in pagedAndFilteredWarehouseVsCouriers
                                      join o1 in _lookup_userRepository.GetAll() on o.CourierId equals o1.Id into j1
                                      from s1 in j1.DefaultIfEmpty()

                                      join o2 in _lookup_warehouseRepository.GetAll() on o.WarehouseId equals o2.Id into j2
                                      from s2 in j2.DefaultIfEmpty()

                                      select new
                                      {
                                          Id = o.Id,
                                          UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                          WarehouseName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                      };

            var totalCount = await filteredWarehouseVsCouriers.CountAsync();

            var dbList = await warehouseVsCouriers.ToListAsync();
            var results = new List<GetWarehouseVsCourierForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetWarehouseVsCourierForViewDto()
                {
                    WarehouseVsCourier = new WarehouseVsCourierDto
                    {
                        Id = o.Id,
                    },
                    UserName = o.UserName,
                    WarehouseName = o.WarehouseName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetWarehouseVsCourierForViewDto>(
                totalCount,
                results
            );
        }

        //[AbpAuthorize(AppPermissions.Pages_WarehouseVsCouriers_Edit)]
        public async Task<GetWarehouseVsCourierForEditOutput> GetWarehouseVsCourierForEdit(EntityDto<Guid> input)
        {
            var warehouseVsCourier = await _warehouseVsCourierRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetWarehouseVsCourierForEditOutput { WarehouseVsCourier = ObjectMapper.Map<CreateOrEditWarehouseVsCourierDto>(warehouseVsCourier) };

            if (output.WarehouseVsCourier.CourierId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.WarehouseVsCourier.CourierId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            if (output.WarehouseVsCourier.WarehouseId != null)
            {
                var _lookupWarehouse = await _lookup_warehouseRepository.FirstOrDefaultAsync((Guid)output.WarehouseVsCourier.WarehouseId);
                output.WarehouseName = _lookupWarehouse?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditWarehouseVsCourierDto input)
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

        //[AbpAuthorize(AppPermissions.Pages_WarehouseVsCouriers_Create)]
        protected virtual async Task Create(CreateOrEditWarehouseVsCourierDto input)
        {
            var warehouseVsCourier = ObjectMapper.Map<WarehouseVsCourier>(input);

            await _warehouseVsCourierRepository.InsertAsync(warehouseVsCourier);
        }

        //[AbpAuthorize(AppPermissions.Pages_WarehouseVsCouriers_Edit)]
        protected virtual async Task Update(CreateOrEditWarehouseVsCourierDto input)
        {
            var warehouseVsCourier = await _warehouseVsCourierRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, warehouseVsCourier);
        }

        //[AbpAuthorize(AppPermissions.Pages_WarehouseVsCouriers_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            await _warehouseVsCourierRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetWarehouseVsCouriersToExcel(GetAllWarehouseVsCouriersForExcelInput input)
        {
            var filteredWarehouseVsCouriers = _warehouseVsCourierRepository.GetAll()
                        .Include(e => e.CourierFk)
                        .Include(e => e.WarehouseFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.CourierFk != null && e.CourierFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.WarehouseNameFilter), e => e.WarehouseFk != null && e.WarehouseFk.Name == input.WarehouseNameFilter);

            var query = (from o in filteredWarehouseVsCouriers
                         join o1 in _lookup_userRepository.GetAll() on o.CourierId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_warehouseRepository.GetAll() on o.WarehouseId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetWarehouseVsCourierForViewDto()
                         {
                             WarehouseVsCourier = new WarehouseVsCourierDto
                             {
                                 Id = o.Id
                             },
                             UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             WarehouseName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var warehouseVsCourierListDtos = await query.ToListAsync();

            return _warehouseVsCouriersExcelExporter.ExportToFile(warehouseVsCourierListDtos);
        }

        //[AbpAuthorize(AppPermissions.Pages_WarehouseVsCouriers)]
        public async Task<List<WarehouseVsCourierUserLookupTableDto>> GetAllUserForTableDropdown()
        {
            return await _lookup_userRepository.GetAll()
                .Select(user => new WarehouseVsCourierUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user == null || user.Name == null ? "" : user.Name.ToString()
                }).ToListAsync();
        }

        //[AbpAuthorize(AppPermissions.Pages_WarehouseVsCouriers)]
        public async Task<List<WarehouseVsCourierWarehouseLookupTableDto>> GetAllWarehouseForTableDropdown()
        {
            return await _lookup_warehouseRepository.GetAll()
                .Select(warehouse => new WarehouseVsCourierWarehouseLookupTableDto
                {
                    Id = warehouse.Id.ToString(),
                    DisplayName = warehouse == null || warehouse.Name == null ? "" : warehouse.Name.ToString()
                }).ToListAsync();
        }
    }
}