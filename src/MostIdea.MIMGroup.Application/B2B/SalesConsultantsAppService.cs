using MostIdea.MIMGroup.Authorization.Users;
using MostIdea.MIMGroup.Authorization.Users;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using MostIdea.MIMGroup.B2B.Exporting;
using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.Dto;
using Abp.Application.Services.Dto;
using MostIdea.MIMGroup.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using MostIdea.MIMGroup.Storage;

namespace MostIdea.MIMGroup.B2B
{
    [AbpAuthorize(AppPermissions.Pages_SalesConsultants)]
    public class SalesConsultantsAppService : MIMGroupAppServiceBase, ISalesConsultantsAppService
    {
        private readonly IRepository<SalesConsultant, Guid> _salesConsultantRepository;
        private readonly ISalesConsultantsExcelExporter _salesConsultantsExcelExporter;
        private readonly IRepository<User, long> _lookup_userRepository;

        public SalesConsultantsAppService(IRepository<SalesConsultant, Guid> salesConsultantRepository, ISalesConsultantsExcelExporter salesConsultantsExcelExporter, IRepository<User, long> lookup_userRepository)
        {
            _salesConsultantRepository = salesConsultantRepository;
            _salesConsultantsExcelExporter = salesConsultantsExcelExporter;
            _lookup_userRepository = lookup_userRepository;

        }

        public async Task<PagedResultDto<GetSalesConsultantForViewDto>> GetAll(GetAllSalesConsultantsInput input)
        {

            var filteredSalesConsultants = _salesConsultantRepository.GetAll()
                        .Include(e => e.SalesConsultantFk)
                        .Include(e => e.DoctorFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.SalesConsultantFk != null && e.SalesConsultantFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserName2Filter), e => e.DoctorFk != null && e.DoctorFk.Name == input.UserName2Filter);

            var pagedAndFilteredSalesConsultants = filteredSalesConsultants
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var salesConsultants = from o in pagedAndFilteredSalesConsultants
                                   join o1 in _lookup_userRepository.GetAll() on o.SalesConsultantId equals o1.Id into j1
                                   from s1 in j1.DefaultIfEmpty()

                                   join o2 in _lookup_userRepository.GetAll() on o.DoctorId equals o2.Id into j2
                                   from s2 in j2.DefaultIfEmpty()

                                   select new
                                   {

                                       Id = o.Id,
                                       UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                       UserName2 = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                   };

            var totalCount = await filteredSalesConsultants.CountAsync();

            var dbList = await salesConsultants.ToListAsync();
            var results = new List<GetSalesConsultantForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetSalesConsultantForViewDto()
                {
                    SalesConsultant = new SalesConsultantDto
                    {

                        Id = o.Id,
                    },
                    UserName = o.UserName,
                    UserName2 = o.UserName2
                };

                results.Add(res);
            }

            return new PagedResultDto<GetSalesConsultantForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetSalesConsultantForViewDto> GetSalesConsultantForView(Guid id)
        {
            var salesConsultant = await _salesConsultantRepository.GetAsync(id);

            var output = new GetSalesConsultantForViewDto { SalesConsultant = ObjectMapper.Map<SalesConsultantDto>(salesConsultant) };

            if (output.SalesConsultant.SalesConsultantId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.SalesConsultant.SalesConsultantId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            if (output.SalesConsultant.DoctorId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.SalesConsultant.DoctorId);
                output.UserName2 = _lookupUser?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_SalesConsultants_Edit)]
        public async Task<GetSalesConsultantForEditOutput> GetSalesConsultantForEdit(EntityDto<Guid> input)
        {
            var salesConsultant = await _salesConsultantRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetSalesConsultantForEditOutput { SalesConsultant = ObjectMapper.Map<CreateOrEditSalesConsultantDto>(salesConsultant) };

            if (output.SalesConsultant.SalesConsultantId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.SalesConsultant.SalesConsultantId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            if (output.SalesConsultant.DoctorId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.SalesConsultant.DoctorId);
                output.UserName2 = _lookupUser?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditSalesConsultantDto input)
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

        [AbpAuthorize(AppPermissions.Pages_SalesConsultants_Create)]
        protected virtual async Task Create(CreateOrEditSalesConsultantDto input)
        {
            var salesConsultant = ObjectMapper.Map<SalesConsultant>(input);

            await _salesConsultantRepository.InsertAsync(salesConsultant);

        }

        [AbpAuthorize(AppPermissions.Pages_SalesConsultants_Edit)]
        protected virtual async Task Update(CreateOrEditSalesConsultantDto input)
        {
            var salesConsultant = await _salesConsultantRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, salesConsultant);

        }

        [AbpAuthorize(AppPermissions.Pages_SalesConsultants_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            await _salesConsultantRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetSalesConsultantsToExcel(GetAllSalesConsultantsForExcelInput input)
        {

            var filteredSalesConsultants = _salesConsultantRepository.GetAll()
                        .Include(e => e.SalesConsultantFk)
                        .Include(e => e.DoctorFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.SalesConsultantFk != null && e.SalesConsultantFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserName2Filter), e => e.DoctorFk != null && e.DoctorFk.Name == input.UserName2Filter);

            var query = (from o in filteredSalesConsultants
                         join o1 in _lookup_userRepository.GetAll() on o.SalesConsultantId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_userRepository.GetAll() on o.DoctorId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetSalesConsultantForViewDto()
                         {
                             SalesConsultant = new SalesConsultantDto
                             {
                                 Id = o.Id
                             },
                             UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             UserName2 = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var salesConsultantListDtos = await query.ToListAsync();

            return _salesConsultantsExcelExporter.ExportToFile(salesConsultantListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_SalesConsultants)]
        public async Task<PagedResultDto<SalesConsultantUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_userRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<SalesConsultantUserLookupTableDto>();
            foreach (var user in userList)
            {
                lookupTableDtoList.Add(new SalesConsultantUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user.Name?.ToString()
                });
            }

            return new PagedResultDto<SalesConsultantUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}