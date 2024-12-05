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
    //[AbpAuthorize(AppPermissions.Pages_HospitalGroups)]
    public class HospitalGroupsAppService : MIMGroupAppServiceBase, IHospitalGroupsAppService
    {
        private readonly IRepository<HospitalGroup, Guid> _hospitalGroupRepository;
        private readonly IHospitalGroupsExcelExporter _hospitalGroupsExcelExporter;

        public HospitalGroupsAppService(IRepository<HospitalGroup, Guid> hospitalGroupRepository, IHospitalGroupsExcelExporter hospitalGroupsExcelExporter)
        {
            _hospitalGroupRepository = hospitalGroupRepository;
            _hospitalGroupsExcelExporter = hospitalGroupsExcelExporter;
        }

        public async Task<PagedResultDto<GetHospitalGroupForViewDto>> GetAll(GetAllHospitalGroupsInput input)
        {
            var filteredHospitalGroups = _hospitalGroupRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter);

            var pagedAndFilteredHospitalGroups = filteredHospitalGroups
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var hospitalGroups = from o in pagedAndFilteredHospitalGroups
                                 select new
                                 {
                                     o.Name,
                                     Id = o.Id
                                 };

            var totalCount = await filteredHospitalGroups.CountAsync();

            var dbList = await hospitalGroups.ToListAsync();
            var results = new List<GetHospitalGroupForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetHospitalGroupForViewDto()
                {
                    HospitalGroup = new HospitalGroupDto
                    {
                        Name = o.Name,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetHospitalGroupForViewDto>(
                totalCount,
                results
            );
        }

        //[AbpAuthorize(AppPermissions.Pages_HospitalGroups_Edit)]
        public async Task<GetHospitalGroupForEditOutput> GetHospitalGroupForEdit(EntityDto<Guid> input)
        {
            var hospitalGroup = await _hospitalGroupRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetHospitalGroupForEditOutput { HospitalGroup = ObjectMapper.Map<CreateOrEditHospitalGroupDto>(hospitalGroup) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditHospitalGroupDto input)
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

        //[AbpAuthorize(AppPermissions.Pages_HospitalGroups_Create)]
        protected virtual async Task Create(CreateOrEditHospitalGroupDto input)
        {
            var hospitalGroup = ObjectMapper.Map<HospitalGroup>(input);

            await _hospitalGroupRepository.InsertAsync(hospitalGroup);
        }

        //[AbpAuthorize(AppPermissions.Pages_HospitalGroups_Edit)]
        protected virtual async Task Update(CreateOrEditHospitalGroupDto input)
        {
            var hospitalGroup = await _hospitalGroupRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, hospitalGroup);
        }

        //[AbpAuthorize(AppPermissions.Pages_HospitalGroups_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            await _hospitalGroupRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetHospitalGroupsToExcel(GetAllHospitalGroupsForExcelInput input)
        {
            var filteredHospitalGroups = _hospitalGroupRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter);

            var query = (from o in filteredHospitalGroups
                         select new GetHospitalGroupForViewDto()
                         {
                             HospitalGroup = new HospitalGroupDto
                             {
                                 Name = o.Name,
                                 Id = o.Id
                             }
                         });

            var hospitalGroupListDtos = await query.ToListAsync();

            return _hospitalGroupsExcelExporter.ExportToFile(hospitalGroupListDtos);
        }
    }
}