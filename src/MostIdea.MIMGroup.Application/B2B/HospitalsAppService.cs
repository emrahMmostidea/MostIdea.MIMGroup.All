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
using Abp.Extensions;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Http;

namespace MostIdea.MIMGroup.B2B
{
    //[AbpAuthorize(AppPermissions.Pages_Hospitals)]
    public class HospitalsAppService : MIMGroupAppServiceBase, IHospitalsAppService
    {
        private readonly IRepository<Hospital, Guid> _hospitalRepository;
        private readonly IHospitalsExcelExporter _hospitalsExcelExporter;
        private readonly IRepository<HospitalGroup, Guid> _lookup_hospitalGroupRepository;
        private readonly IRepository<HospitalVsUser, Guid> _hospitalVsUserRepository;
        private readonly ICacheManager _cacheManager;

        public HospitalsAppService(IRepository<Hospital, Guid> hospitalRepository, IHospitalsExcelExporter hospitalsExcelExporter, IRepository<HospitalGroup, Guid> lookupHospitalGroupRepository, IRepository<HospitalVsUser, Guid> hospitalVsUserRepository, ICacheManager cacheManager)
        {
            _hospitalRepository = hospitalRepository;
            _hospitalsExcelExporter = hospitalsExcelExporter;
            _lookup_hospitalGroupRepository = lookupHospitalGroupRepository;
            _hospitalVsUserRepository = hospitalVsUserRepository;
            _cacheManager = cacheManager;
        }

        public async Task<PagedResultDto<GetHospitalForViewDto>> GetAll(GetAllHospitalsInput input)
        {
            var filteredHospitals = _hospitalRepository.GetAll()
                        .Include(e => e.HospitalGroupFk)
                        .WhereIf(input.HospitalTypeEnum == HospitalTypeEnum.Hospital, x => x.HospitalGroupFk.Name != "Klinik" || x.HospitalGroupFk.Name != "Bayi")
                        .WhereIf(input.HospitalTypeEnum == HospitalTypeEnum.Dealer, x => x.HospitalGroupFk.Name == "Bayi")
                        .WhereIf(input.HospitalTypeEnum == HospitalTypeEnum.Clinic, x => x.HospitalGroupFk.Name == "Klinik")
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.TaxAdministration.Contains(input.Filter) || e.TaxNumber.Contains(input.Filter) || e.Coordinate.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HospitalGroupNameFilter), e => e.HospitalGroupFk != null && e.HospitalGroupFk.Name == input.HospitalGroupNameFilter);

            var pagedAndFilteredHospitals = filteredHospitals
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var hospitals = from o in pagedAndFilteredHospitals
                            join o1 in _lookup_hospitalGroupRepository.GetAll() on o.HospitalGroupId equals o1.Id into j1
                            from s1 in j1.DefaultIfEmpty()

                            select new
                            {
                                o.Name,
                                Id = o.Id,
                                HospitalGroupName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                AddressInformation = o.AddresInformations.FirstOrDefault(x => x.IsPrimary)
                            };

            var totalCount = await filteredHospitals.CountAsync();

            var dbList = await hospitals.ToListAsync();
            var results = new List<GetHospitalForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetHospitalForViewDto()
                {
                    Hospital = new HospitalDto
                    {
                        Name = o.Name,
                        Id = o.Id,
                        AddressInformation = ObjectMapper.Map<AddressInformationDto>(o.AddressInformation)
                    },
                    HospitalGroupName = o.HospitalGroupName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetHospitalForViewDto>(
                totalCount,
                results
            );
        }

        public async Task<GetHospitalForViewDto> GetHospitalForView(Guid id)
        {
            var hospital = await _hospitalRepository.GetAsync(id);

            var output = new GetHospitalForViewDto { Hospital = ObjectMapper.Map<HospitalDto>(hospital) };

            if (output.Hospital.HospitalGroupId != null)
            {
                var _lookupHospitalGroup = await _lookup_hospitalGroupRepository.FirstOrDefaultAsync((Guid)output.Hospital.HospitalGroupId);
                output.HospitalGroupName = _lookupHospitalGroup?.Name?.ToString();
            }

            return output;
        }

        //[AbpAuthorize(AppPermissions.Pages_Hospitals_Edit)]
        public async Task<GetHospitalForEditOutput> GetHospitalForEdit(EntityDto<Guid> input)
        {
            var hospital = await _hospitalRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetHospitalForEditOutput { Hospital = ObjectMapper.Map<CreateOrEditHospitalDto>(hospital) };

            if (output.Hospital.HospitalGroupId != null)
            {
                var _lookupHospitalGroup = await _lookup_hospitalGroupRepository.FirstOrDefaultAsync((Guid)output.Hospital.HospitalGroupId);
                output.HospitalGroupName = _lookupHospitalGroup?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditHospitalDto input)
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

        //[AbpAuthorize(AppPermissions.Pages_Hospitals_Create)]
        protected virtual async Task Create(CreateOrEditHospitalDto input)
        {
            var hospital = ObjectMapper.Map<Hospital>(input);

            await _hospitalRepository.InsertAsync(hospital);
        }

        //[AbpAuthorize(AppPermissions.Pages_Hospitals_Edit)]
        protected virtual async Task Update(CreateOrEditHospitalDto input)
        {
            var hospital = await _hospitalRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, hospital);
        }

        //[AbpAuthorize(AppPermissions.Pages_Hospitals_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            await _hospitalRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetHospitalsToExcel(GetAllHospitalsForExcelInput input)
        {
            var filteredHospitals = _hospitalRepository.GetAll()
                        .Include(e => e.HospitalGroupFk)
                        .Where(x => x.HospitalGroupFk.Name != "Klinik" || x.HospitalGroupFk.Name != "Bayi")
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.TaxAdministration.Contains(input.Filter) || e.TaxNumber.Contains(input.Filter) || e.Coordinate.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HospitalGroupNameFilter), e => e.HospitalGroupFk != null && e.HospitalGroupFk.Name == input.HospitalGroupNameFilter);

            var query = (from o in filteredHospitals
                         join o1 in _lookup_hospitalGroupRepository.GetAll() on o.HospitalGroupId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetHospitalForViewDto()
                         {
                             Hospital = new HospitalDto
                             {
                                 Name = o.Name,
                                 Id = o.Id
                             },
                             HospitalGroupName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         });

            var hospitalListDtos = await query.ToListAsync();

            return _hospitalsExcelExporter.ExportToFile(hospitalListDtos);
        }

        //[AbpAuthorize(AppPermissions.Pages_Hospitals)]
        public async Task<List<HospitalHospitalGroupLookupTableDto>> GetAllHospitalGroupForTableDropdown()
        {
            return await _lookup_hospitalGroupRepository.GetAll()
                .Where(x => x.Name != "Klinik" || x.Name != "Bayi")
                .Select(hospitalGroup => new HospitalHospitalGroupLookupTableDto
                {
                    Id = hospitalGroup.Id.ToString(),
                    DisplayName = hospitalGroup == null || hospitalGroup.Name == null ? "" : hospitalGroup.Name.ToString()
                }).ToListAsync();
        }


        public async Task<List<SelectionDto>> Selection(SelectionInput input)
        {
            var userId = AbpSession.UserId;
            if (userId.HasValue)
            {
                var hospitalIds = await _hospitalVsUserRepository.GetAll().Where(x => x.UserId == userId.Value).Select(x => x.HospitalId).ToListAsync();
                var roles = await UserManager.GetRolesAsync(await GetCurrentUserAsync());

                var source = await _hospitalRepository.GetAll()
                    .WhereIf(roles.Any(x => x != "Admin"), x => hospitalIds.Contains(x.Id))
                    .WhereIf(!string.IsNullOrEmpty(input.Keyword), x => x.Name.ToLower().Contains(input.Keyword.ToLower()))
                .Include(x => x.HospitalGroupFk)
                .OrderBy(x => x.HospitalGroupFk.Name).ThenBy(x => x.Name).Select(c => new SelectionDto
                {
                    Id = c.Id.ToString(),
                    Name = $"{c.HospitalGroupFk.Name} - {c.Name}",
                    Color = c.LogoId.HasValue ? c.LogoId.Value.ToString() : "/Common/Images/default-picture.png",
                })
                    .Skip(input.PageSize * input.CurrentPage)
                    .Take(input.PageSize)
                    .ToListAsync();

                return source;
            }
            return new List<SelectionDto>();
        }

        public async Task<bool> SetSelectedHospital(Guid hospitalId)
        {
            try
            {
                if (!AbpSession.UserId.HasValue)
                    return false;

                var cache = _cacheManager.GetCache("SelectedHospital");
                await cache.SetAsync(AbpSession.UserId.Value.ToString(), hospitalId);
                return true;
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                return false;
            }
        }

        public async Task<HospitalDto> GetSelectedHospital()
        {
            try
            {
                if (!AbpSession.UserId.HasValue)
                    return null;

                var cache = _cacheManager.GetCache("SelectedHospital");
                var hospitalCache = await cache.GetOrDefaultAsync(AbpSession.UserId.ToString());
                if (hospitalCache != null)
                {
                    var Id = Guid.Parse(hospitalCache.ToString());
                    var hospital = await _hospitalRepository.GetAll().Include(x => x.HospitalGroupFk).FirstOrDefaultAsync(x => x.Id == Id);
                   var hospitalDto = ObjectMapper.Map<HospitalDto>(hospital);
                   hospitalDto.HospitalGroupName = hospital.HospitalGroupFk.Name;

                    return hospitalDto;
                }

                return null;
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                return null;
            }
        }

        public async Task<bool> ClearSelectedHospital()
        {
            try
            {
                if (!AbpSession.UserId.HasValue)
                    return false;

                var cache = _cacheManager.GetCache("SelectedHospital");
                var hospitalCache = await cache.GetOrDefaultAsync(AbpSession.UserId.ToString());
                if (hospitalCache != null)
                {
                    await cache.RemoveAsync(AbpSession.UserId.ToString());
                    return true;
                }

                return true;
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                return false;
            }
        }
    }
}