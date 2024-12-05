using MostIdea.MIMGroup.Authorization.Users;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
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
    [AbpAuthorize(AppPermissions.Pages_AssistanceVsUsers)]
    public class AssistanceVsUsersAppService : MIMGroupAppServiceBase, IAssistanceVsUsersAppService
    {
        private readonly IRepository<AssistanceVsUser, Guid> _assistanceVsUserRepository;
        private readonly IRepository<User, long> _lookup_userRepository;

        public AssistanceVsUsersAppService(IRepository<AssistanceVsUser, Guid> assistanceVsUserRepository, IRepository<User, long> lookup_userRepository)
        {
            _assistanceVsUserRepository = assistanceVsUserRepository;
            _lookup_userRepository = lookup_userRepository;

        }

        public async Task<PagedResultDto<GetAssistanceVsUserForViewDto>> GetAll(GetAllAssistanceVsUsersInput input)
        {

            var filteredAssistanceVsUsers = _assistanceVsUserRepository.GetAll()
                        .Include(e => e.AssistanceFk)
                        .Include(e => e.DoctorFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.AssistanceFk != null && e.AssistanceFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserName2Filter), e => e.DoctorFk != null && e.DoctorFk.Name == input.UserName2Filter);

            var pagedAndFilteredAssistanceVsUsers = filteredAssistanceVsUsers
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var assistanceVsUsers = from o in pagedAndFilteredAssistanceVsUsers
                                    join o1 in _lookup_userRepository.GetAll() on o.AssistanceId equals o1.Id into j1
                                    from s1 in j1.DefaultIfEmpty()

                                    join o2 in _lookup_userRepository.GetAll() on o.DoctorId equals o2.Id into j2
                                    from s2 in j2.DefaultIfEmpty()

                                    select new
                                    {

                                        Id = o.Id,
                                        UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString() + " " + s1.Surname.ToString(),
                                        UserName2 = s2 == null || s2.Name == null ? "" : s2.Name.ToString() + " " + s2.Surname.ToString(),
                                    };

            var totalCount = await filteredAssistanceVsUsers.CountAsync();

            var dbList = await assistanceVsUsers.ToListAsync();
            var results = new List<GetAssistanceVsUserForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetAssistanceVsUserForViewDto()
                {
                    AssistanceVsUser = new AssistanceVsUserDto
                    {

                        Id = o.Id,
                    },
                    UserName = o.UserName,
                    UserName2 = o.UserName2,
                };

                results.Add(res);
            }

            return new PagedResultDto<GetAssistanceVsUserForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_AssistanceVsUsers_Edit)]
        public async Task<GetAssistanceVsUserForEditOutput> GetAssistanceVsUserForEdit(EntityDto<Guid> input)
        {
            var assistanceVsUser = await _assistanceVsUserRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetAssistanceVsUserForEditOutput { AssistanceVsUser = ObjectMapper.Map<CreateOrEditAssistanceVsUserDto>(assistanceVsUser) };

            if (output.AssistanceVsUser.AssistanceId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.AssistanceVsUser.AssistanceId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            if (output.AssistanceVsUser.DoctorId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.AssistanceVsUser.DoctorId);
                output.UserName2 = _lookupUser?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditAssistanceVsUserDto input)
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

        [AbpAuthorize(AppPermissions.Pages_AssistanceVsUsers_Create)]
        protected virtual async Task Create(CreateOrEditAssistanceVsUserDto input)
        {
            var assistanceVsUser = ObjectMapper.Map<AssistanceVsUser>(input);

            await _assistanceVsUserRepository.InsertAsync(assistanceVsUser);

        }

        [AbpAuthorize(AppPermissions.Pages_AssistanceVsUsers_Edit)]
        protected virtual async Task Update(CreateOrEditAssistanceVsUserDto input)
        {
            var assistanceVsUser = await _assistanceVsUserRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, assistanceVsUser);

        }

        [AbpAuthorize(AppPermissions.Pages_AssistanceVsUsers_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            await _assistanceVsUserRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_AssistanceVsUsers)]
        public async Task<PagedResultDto<AssistanceVsUserUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_userRepository.GetAll()
                    .Where(x =>  x.UserType == UserTypeEnum.Assistance)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => e.DiplomaNo != null && (e.DiplomaNo).Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => e.Name != null && (e.Name + " " +e.Surname).Contains(input.Filter))
                ;

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<AssistanceVsUserUserLookupTableDto>();
            foreach (var user in userList)
            {
                lookupTableDtoList.Add(new AssistanceVsUserUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user.DiplomaNo + " | " + user.Name.ToString() + " " + user.Surname.ToString()
                });
            }

            return new PagedResultDto<AssistanceVsUserUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
        [AbpAuthorize(AppPermissions.Pages_AssistanceVsUsers)]
        public async Task<List<AssistanceVsUserUserLookupTableDto>> GetAllUserForTableDropdown()
        {
            return await _lookup_userRepository.GetAll()
                .Select(user => new AssistanceVsUserUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user == null || user.Name == null ? "" : user.DiplomaNo + " | " + user.Name.ToString() + " " + user.Surname.ToString()
                }).ToListAsync();
        }

    }
}