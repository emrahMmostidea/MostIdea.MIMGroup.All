using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using MostIdea.MIMGroup.Authorization;
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
    //[AbpAuthorize(AppPermissions.Pages_AddressInformations)]
    public class AddressInformationsAppService : MIMGroupAppServiceBase, IAddressInformationsAppService
    {
        private readonly IRepository<AddressInformation, Guid> _addressInformationRepository;
        private readonly IAddressInformationsExcelExporter _addressInformationsExcelExporter;
        private readonly IHospitalsAppService _hospitalsAppService;
        private readonly IRepository<HospitalVsUser, Guid> _hospitalVsUserRepository;

        public AddressInformationsAppService(IRepository<AddressInformation, Guid> addressInformationRepository, IAddressInformationsExcelExporter addressInformationsExcelExporter, IHospitalsAppService hospitalsAppService, IRepository<HospitalVsUser, Guid> hospitalVsUserRepository)
        {
            _addressInformationRepository = addressInformationRepository;
            _addressInformationsExcelExporter = addressInformationsExcelExporter;
            _hospitalsAppService = hospitalsAppService;
            _hospitalVsUserRepository = hospitalVsUserRepository;
        }

        public async Task<PagedResultDto<GetAddressInformationForViewDto>> GetAll(GetAllAddressInformationsInput input)
        {


            var filteredAddressInformations = _addressInformationRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.Name.Contains(input.Filter) || e.Address.Contains(input.Filter) ||
                         e.Phone.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
               .WhereIf(input.HospitalId.HasValue, x => x.HospitalId == input.HospitalId.Value);
            var pagedAndFilteredAddressInformations = filteredAddressInformations
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var addressInformations = from o in pagedAndFilteredAddressInformations
                                      select new
                                      {
                                          Name = o.Name,
                                          Id = o.Id,
                                          Address = o.Address,
                                          Phone = o.Phone,
                                          IsPrimary = o.IsPrimary
                                      };

            var totalCount = await filteredAddressInformations.CountAsync();

            var dbList = await addressInformations.ToListAsync();
            var results = new List<GetAddressInformationForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetAddressInformationForViewDto()
                {
                    AddressInformation = new AddressInformationDto
                    {
                        Name = o.Name,
                        Id = o.Id,
                        Address = o.Address,
                        Phone = o.Phone,
                        IsPrimary = o.IsPrimary
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetAddressInformationForViewDto>(
                totalCount,
                results
            );
        }

        //[AbpAuthorize(AppPermissions.Pages_AddressInformations_Edit)]
        public async Task<GetAddressInformationForEditOutput> GetAddressInformationForEdit(EntityDto<Guid> input)
        {
            var addressInformation = await _addressInformationRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetAddressInformationForEditOutput { AddressInformation = ObjectMapper.Map<CreateOrEditAddressInformationDto>(addressInformation) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditAddressInformationDto input)
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

        //[AbpAuthorize(AppPermissions.Pages_AddressInformations_Create)]
        protected virtual async Task Create(CreateOrEditAddressInformationDto input)
        {
            if (!input.HospitalId.HasValue)
            {
                input.HospitalId = (await _hospitalsAppService.GetSelectedHospital()).Id;
            }
            


            var addressInformation = ObjectMapper.Map<AddressInformation>(input);

            await _addressInformationRepository.InsertAsync(addressInformation);
        }

        //[AbpAuthorize(AppPermissions.Pages_AddressInformations_Edit)]
        protected virtual async Task Update(CreateOrEditAddressInformationDto input)
        {
            var addressInformation = await _addressInformationRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, addressInformation);
        }

        //[AbpAuthorize(AppPermissions.Pages_AddressInformations_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            await _addressInformationRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetAddressInformationsToExcel(GetAllAddressInformationsForExcelInput input)
        {
            var filteredAddressInformations = _addressInformationRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Address.Contains(input.Filter) || e.Phone.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter);

            var query = (from o in filteredAddressInformations
                         select new GetAddressInformationForViewDto()
                         {
                             AddressInformation = new AddressInformationDto
                             {
                                 Name = o.Name,
                                 Id = o.Id
                             }
                         });

            var addressInformationListDtos = await query.ToListAsync();

            return _addressInformationsExcelExporter.ExportToFile(addressInformationListDtos);
        }
    }
}