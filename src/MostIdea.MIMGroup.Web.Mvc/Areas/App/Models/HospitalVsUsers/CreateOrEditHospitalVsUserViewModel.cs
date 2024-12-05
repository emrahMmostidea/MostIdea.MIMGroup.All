using MostIdea.MIMGroup.B2B.Dtos;
using System.Collections.Generic;
using System.Collections.Generic;

using Abp.Extensions;

namespace MostIdea.MIMGroup.Web.Areas.App.Models.HospitalVsUsers
{
    public class CreateOrEditHospitalVsUserModalViewModel
    {
        public CreateOrEditHospitalVsUserDto HospitalVsUser { get; set; }

        public string HospitalName { get; set; }

        public string UserName { get; set; }

        public List<HospitalVsUserHospitalLookupTableDto> HospitalVsUserHospitalList { get; set; }

        public List<HospitalVsUserUserLookupTableDto> HospitalVsUserUserList { get; set; }

        public bool IsEditMode => HospitalVsUser.Id.HasValue;
    }
}