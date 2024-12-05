using MostIdea.MIMGroup.B2B.Dtos;

using Abp.Extensions;

namespace MostIdea.MIMGroup.Web.Areas.App.Models.Brands
{
    public class CreateOrEditBrandModalViewModel
    {
        public CreateOrEditBrandDto Brand { get; set; }

        public bool IsEditMode => Brand.Id.HasValue;
    }
}