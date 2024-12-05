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
    //[AbpAuthorize(AppPermissions.Pages_HospitalVsUsers)]
    public class HospitalVsUsersAppService : MIMGroupAppServiceBase, IHospitalVsUsersAppService
    {
        private readonly IRepository<HospitalVsUser, Guid> _hospitalVsUserRepository;
        private readonly IHospitalVsUsersExcelExporter _hospitalVsUsersExcelExporter;
        private readonly IRepository<Hospital, Guid> _lookup_hospitalRepository;
        private readonly IRepository<User, long> _lookup_userRepository;

        public HospitalVsUsersAppService(IRepository<HospitalVsUser, Guid> hospitalVsUserRepository, IHospitalVsUsersExcelExporter hospitalVsUsersExcelExporter, IRepository<Hospital, Guid> lookup_hospitalRepository, IRepository<User, long> lookup_userRepository)
        {
            _hospitalVsUserRepository = hospitalVsUserRepository;
            _hospitalVsUsersExcelExporter = hospitalVsUsersExcelExporter;
            _lookup_hospitalRepository = lookup_hospitalRepository;
            _lookup_userRepository = lookup_userRepository;
        }

        public async Task<PagedResultDto<GetHospitalVsUserForViewDto>> GetAll(GetAllHospitalVsUsersInput input)
        {
            var filteredHospitalVsUsers = _hospitalVsUserRepository.GetAll()
                        .Include(e => e.HospitalFk)
                        .Include(e => e.UserFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HospitalNameFilter), e => e.HospitalFk != null && e.HospitalFk.Name == input.HospitalNameFilter)
                        .WhereIf(input.HospitalId.HasValue, x => x.HospitalId == input.HospitalId)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

            var pagedAndFilteredHospitalVsUsers = filteredHospitalVsUsers
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var hospitalVsUsers = from o in pagedAndFilteredHospitalVsUsers
                                  join o1 in _lookup_hospitalRepository.GetAll() on o.HospitalId equals o1.Id into j1
                                  from s1 in j1.DefaultIfEmpty()

                                  join o2 in _lookup_userRepository.GetAll() on o.UserId equals o2.Id into j2
                                  from s2 in j2.DefaultIfEmpty()

                                  select new
                                  {
                                      Id = o.Id,
                                      HospitalName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                      UserName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                  };

            var totalCount = await filteredHospitalVsUsers.CountAsync();

            var dbList = await hospitalVsUsers.ToListAsync();
            var results = new List<GetHospitalVsUserForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetHospitalVsUserForViewDto()
                {
                    HospitalVsUser = new HospitalVsUserDto
                    {
                        Id = o.Id,
                    },
                    HospitalName = o.HospitalName,
                    UserName = o.UserName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetHospitalVsUserForViewDto>(
                totalCount,
                results
            );
        }

        //[AbpAuthorize(AppPermissions.Pages_HospitalVsUsers_Edit)]
        public async Task<GetHospitalVsUserForEditOutput> GetHospitalVsUserForEdit(EntityDto<Guid> input)
        {
            var hospitalVsUser = await _hospitalVsUserRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetHospitalVsUserForEditOutput { HospitalVsUser = ObjectMapper.Map<CreateOrEditHospitalVsUserDto>(hospitalVsUser) };

            if (output.HospitalVsUser.HospitalId != null)
            {
                var _lookupHospital = await _lookup_hospitalRepository.FirstOrDefaultAsync((Guid)output.HospitalVsUser.HospitalId);
                output.HospitalName = _lookupHospital?.Name?.ToString();
            }

            if (output.HospitalVsUser.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.HospitalVsUser.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditHospitalVsUserDto input)
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

        //[AbpAuthorize(AppPermissions.Pages_HospitalVsUsers_Create)]
        protected virtual async Task Create(CreateOrEditHospitalVsUserDto input)
        {
            var hospitalVsUser = ObjectMapper.Map<HospitalVsUser>(input);

            await _hospitalVsUserRepository.InsertAsync(hospitalVsUser);
        }

        //[AbpAuthorize(AppPermissions.Pages_HospitalVsUsers_Edit)]
        protected virtual async Task Update(CreateOrEditHospitalVsUserDto input)
        {
            var hospitalVsUser = await _hospitalVsUserRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, hospitalVsUser);
        }

        //[AbpAuthorize(AppPermissions.Pages_HospitalVsUsers_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            await _hospitalVsUserRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetHospitalVsUsersToExcel(GetAllHospitalVsUsersForExcelInput input)
        {
            var filteredHospitalVsUsers = _hospitalVsUserRepository.GetAll()
                        .Include(e => e.HospitalFk)
                        .Include(e => e.UserFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HospitalNameFilter), e => e.HospitalFk != null && e.HospitalFk.Name == input.HospitalNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

            var query = (from o in filteredHospitalVsUsers
                         join o1 in _lookup_hospitalRepository.GetAll() on o.HospitalId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_userRepository.GetAll() on o.UserId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetHospitalVsUserForViewDto()
                         {
                             HospitalVsUser = new HospitalVsUserDto
                             {
                                 Id = o.Id
                             },
                             HospitalName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             UserName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var hospitalVsUserListDtos = await query.ToListAsync();

            return _hospitalVsUsersExcelExporter.ExportToFile(hospitalVsUserListDtos);
        }

        //[AbpAuthorize(AppPermissions.Pages_HospitalVsUsers)]
        public async Task<List<HospitalVsUserHospitalLookupTableDto>> GetAllHospitalForTableDropdown()
        {
            return await _lookup_hospitalRepository.GetAll()
                .Select(hospital => new HospitalVsUserHospitalLookupTableDto
                {
                    Id = hospital.Id.ToString(),
                    DisplayName = hospital == null || hospital.Name == null ? "" : hospital.Name.ToString()
                }).ToListAsync();
        }

        //[AbpAuthorize(AppPermissions.Pages_HospitalVsUsers)]
        public async Task<List<HospitalVsUserUserLookupTableDto>> GetAllUserForTableDropdown()
        {
            return await _lookup_userRepository.GetAll()
                .Select(user => new HospitalVsUserUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user == null || user.Name == null ? "" : user.Name.ToString()
                }).ToListAsync();
        }
    }
}