using MostIdea.MIMGroup.B2B.Dtos;
using System.Collections.Generic;

using Abp.Extensions;

namespace MostIdea.MIMGroup.Web.Areas.App.Models.Hospitals
{
    public class CreateOrEditHospitalModalViewModel
    {
        public CreateOrEditHospitalDto Hospital { get; set; }

        public string HospitalGroupName { get; set; }

        public List<HospitalHospitalGroupLookupTableDto> HospitalHospitalGroupList { get; set; }

        public bool IsEditMode => Hospital.Id.HasValue;
    }
}