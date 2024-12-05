using MostIdea.MIMGroup.B2B.Dtos;

using Abp.Extensions;

namespace MostIdea.MIMGroup.Web.Areas.App.Models.SalesConsultants
{
    public class CreateOrEditSalesConsultantModalViewModel
    {
        public CreateOrEditSalesConsultantDto SalesConsultant { get; set; }

        public string UserName { get; set; }

        public string UserName2 { get; set; }

        public bool IsEditMode => SalesConsultant.Id.HasValue;
    }
}