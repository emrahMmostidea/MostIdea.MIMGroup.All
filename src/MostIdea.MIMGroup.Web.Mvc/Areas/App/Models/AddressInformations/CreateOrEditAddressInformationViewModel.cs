using MostIdea.MIMGroup.B2B.Dtos;

using Abp.Extensions;

namespace MostIdea.MIMGroup.Web.Areas.App.Models.AddressInformations
{
    public class CreateOrEditAddressInformationModalViewModel
    {
        public CreateOrEditAddressInformationDto AddressInformation { get; set; }

        public bool IsEditMode => AddressInformation.Id.HasValue;
    }
}