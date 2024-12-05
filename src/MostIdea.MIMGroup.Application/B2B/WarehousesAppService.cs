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
    //[AbpAuthorize(AppPermissions.Pages_Warehouses)]
    public class WarehousesAppService : MIMGroupAppServiceBase, IWarehousesAppService
    {
        private readonly IRepository<Warehouse, Guid> _warehouseRepository;
        private readonly IWarehousesExcelExporter _warehousesExcelExporter;
        private readonly IRepository<District, Guid> _lookup_districtRepository;

        public WarehousesAppService(IRepository<Warehouse, Guid> warehouseRepository, IWarehousesExcelExporter warehousesExcelExporter, IRepository<District, Guid> lookup_districtRepository)
        {
            _warehouseRepository = warehouseRepository;
            _warehousesExcelExporter = warehousesExcelExporter;
            _lookup_districtRepository = lookup_districtRepository;
        }

        public async Task<PagedResultDto<GetWarehouseForViewDto>> GetAll(GetAllWarehousesInput input)
        {
            var filteredWarehouses = _warehouseRepository.GetAll()
                        .Include(e => e.DistrictFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Coordinate.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CoordinateFilter), e => e.Coordinate == input.CoordinateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DistrictNameFilter), e => e.DistrictFk != null && e.DistrictFk.Name == input.DistrictNameFilter);

            var pagedAndFilteredWarehouses = filteredWarehouses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var warehouses = from o in pagedAndFilteredWarehouses
                             join o1 in _lookup_districtRepository.GetAll() on o.DistrictId equals o1.Id into j1
                             from s1 in j1.DefaultIfEmpty()

                             select new
                             {
                                 o.Name,
                                 o.Coordinate,
                                 Id = o.Id,
                                 DistrictName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                             };

            var totalCount = await filteredWarehouses.CountAsync();

            var dbList = await warehouses.ToListAsync();
            var results = new List<GetWarehouseForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetWarehouseForViewDto()
                {
                    Warehouse = new WarehouseDto
                    {
                        Name = o.Name,
                        Coordinate = o.Coordinate,
                        Id = o.Id,
                    },
                    DistrictName = o.DistrictName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetWarehouseForViewDto>(
                totalCount,
                results
            );
        }

        //[AbpAuthorize(AppPermissions.Pages_Warehouses_Edit)]
        public async Task<GetWarehouseForEditOutput> GetWarehouseForEdit(EntityDto<Guid> input)
        {
            var warehouse = await _warehouseRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetWarehouseForEditOutput { Warehouse = ObjectMapper.Map<CreateOrEditWarehouseDto>(warehouse) };

            if (output.Warehouse.DistrictId != null)
            {
                var _lookupDistrict = await _lookup_districtRepository.FirstOrDefaultAsync((Guid)output.Warehouse.DistrictId);
                output.DistrictName = _lookupDistrict?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditWarehouseDto input)
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

        //[AbpAuthorize(AppPermissions.Pages_Warehouses_Create)]
        protected virtual async Task Create(CreateOrEditWarehouseDto input)
        {
            var warehouse = ObjectMapper.Map<Warehouse>(input);

            await _warehouseRepository.InsertAsync(warehouse);
        }

        //[AbpAuthorize(AppPermissions.Pages_Warehouses_Edit)]
        protected virtual async Task Update(CreateOrEditWarehouseDto input)
        {
            var warehouse = await _warehouseRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, warehouse);
        }

        //[AbpAuthorize(AppPermissions.Pages_Warehouses_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            await _warehouseRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetWarehousesToExcel(GetAllWarehousesForExcelInput input)
        {
            var filteredWarehouses = _warehouseRepository.GetAll()
                        .Include(e => e.DistrictFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Coordinate.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CoordinateFilter), e => e.Coordinate == input.CoordinateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DistrictNameFilter), e => e.DistrictFk != null && e.DistrictFk.Name == input.DistrictNameFilter);

            var query = (from o in filteredWarehouses
                         join o1 in _lookup_districtRepository.GetAll() on o.DistrictId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetWarehouseForViewDto()
                         {
                             Warehouse = new WarehouseDto
                             {
                                 Name = o.Name,
                                 Coordinate = o.Coordinate,
                                 Id = o.Id
                             },
                             DistrictName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         });

            var warehouseListDtos = await query.ToListAsync();

            return _warehousesExcelExporter.ExportToFile(warehouseListDtos);
        }

        //[AbpAuthorize(AppPermissions.Pages_Warehouses)]
        public async Task<List<WarehouseDistrictLookupTableDto>> GetAllDistrictForTableDropdown()
        {
            return await _lookup_districtRepository.GetAll()
                .Select(district => new WarehouseDistrictLookupTableDto
                {
                    Id = district.Id.ToString(),
                    DisplayName = district == null || district.Name == null ? "" : district.Name.ToString()
                }).ToListAsync();
        }
    }
}