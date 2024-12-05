using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using MostIdea.MIMGroup.Authorization;
using MostIdea.MIMGroup.B2B.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace MostIdea.MIMGroup.B2B
{
    //[AbpAuthorize(AppPermissions.Pages_DynamicEnums)]
    public class DynamicEnumsAppService : MIMGroupAppServiceBase, IDynamicEnumsAppService
    {
        private readonly IRepository<DynamicEnum, Guid> _dynamicEnumRepository;

        public DynamicEnumsAppService(IRepository<DynamicEnum, Guid> dynamicEnumRepository)
        {
            _dynamicEnumRepository = dynamicEnumRepository;
        }

        public async Task<PagedResultDto<GetDynamicEnumForViewDto>> GetAll(GetAllDynamicEnumsInput input)
        {
            var filteredDynamicEnums = _dynamicEnumRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.EnumFile.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter);

            var pagedAndFilteredDynamicEnums = filteredDynamicEnums
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var dynamicEnums = from o in pagedAndFilteredDynamicEnums
                               select new
                               {
                                   o.Name,
                                   o.Description,
                                   o.EnumFile,
                                   Id = o.Id
                               };

            var totalCount = await filteredDynamicEnums.CountAsync();

            var dbList = await dynamicEnums.ToListAsync();
            var results = new List<GetDynamicEnumForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetDynamicEnumForViewDto()
                {
                    DynamicEnum = new DynamicEnumDto
                    {
                        Name = o.Name,
                        Description = o.Description,
                        EnumFile = o.EnumFile,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetDynamicEnumForViewDto>(
                totalCount,
                results
            );
        }

        //[AbpAuthorize(AppPermissions.Pages_DynamicEnums_Edit)]
        public async Task<GetDynamicEnumForEditOutput> GetDynamicEnumForEdit(EntityDto<Guid> input)
        {
            var dynamicEnum = await _dynamicEnumRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDynamicEnumForEditOutput
            {
                DynamicEnum = ObjectMapper.Map<CreateOrEditDynamicEnumDto>(dynamicEnum)
            };

            return output;
        }

        public List<SelectionDto> GetEnumFiles()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            return assemblies.FirstOrDefault(x => x.GetName().Name.Contains("Core.Shared")).GetTypes()
                      .Where(t => t.Name.EndsWith("Enum")).Select(e => new SelectionDto { Name = e.Name, Id = e.Name }).ToList();
        }

        public async Task<List<SelectionDto>> GetEnumValues(Guid enumId)
        { 
            var enumObj = await _dynamicEnumRepository.FirstOrDefaultAsync(x => x.Id == enumId);

            if (enumObj != null)
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                var enumType = assemblies.FirstOrDefault(x => x.GetName().Name.Contains("Core.Shared")).GetTypes().FirstOrDefault(x => x.Name == enumObj.EnumFile);
                return Enum.GetValues(enumType).Cast<Enum>().Select(e => new SelectionDto { Name = e.ToString(), Id = e.ToString() }).ToList();
            }

            return new List<SelectionDto>();
        }   

        public async Task CreateOrEdit(CreateOrEditDynamicEnumDto input)
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

        //[AbpAuthorize(AppPermissions.Pages_DynamicEnums_Create)]
        protected virtual async Task Create(CreateOrEditDynamicEnumDto input)
        {
            var dynamicEnum = ObjectMapper.Map<DynamicEnum>(input);

            await _dynamicEnumRepository.InsertAsync(dynamicEnum);
        }

        //[AbpAuthorize(AppPermissions.Pages_DynamicEnums_Edit)]
        protected virtual async Task Update(CreateOrEditDynamicEnumDto input)
        {
            var dynamicEnum = await _dynamicEnumRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, dynamicEnum);
        }

        //[AbpAuthorize(AppPermissions.Pages_DynamicEnums_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            await _dynamicEnumRepository.DeleteAsync(input.Id);
        }
    }
}