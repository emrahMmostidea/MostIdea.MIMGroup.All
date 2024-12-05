using MostIdea.MIMGroup.B2B.Dtos;

using Abp.Extensions;

namespace MostIdea.MIMGroup.Web.Areas.App.Models.HospitalGroups
{
    public class CreateOrEditHospitalGroupModalViewModel
    {
        public CreateOrEditHospitalGroupDto HospitalGroup { get; set; }

        public bool IsEditMode => HospitalGroup.Id.HasValue;
    }
}