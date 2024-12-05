using MostIdea.MIMGroup.B2B.Dtos;

using Abp.Extensions;

namespace MostIdea.MIMGroup.Web.Areas.App.Models.TaxRates
{
    public class CreateOrEditTaxRateModalViewModel
    {
        public CreateOrEditTaxRateDto TaxRate { get; set; }

        public bool IsEditMode => TaxRate.Id.HasValue;
    }
}