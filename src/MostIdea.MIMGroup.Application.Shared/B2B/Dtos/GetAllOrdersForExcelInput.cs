using Abp.Application.Services.Dto;
using System;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class GetAllOrdersForExcelInput
    {
        public string Filter { get; set; }

        public decimal? MaxTotalFilter { get; set; }
        public decimal? MinTotalFilter { get; set; }

        public decimal? MaxTaxFilter { get; set; }
        public decimal? MinTaxFilter { get; set; }

        public decimal? MaxGrandTotalFilter { get; set; }
        public decimal? MinGrandTotalFilter { get; set; }

        public int? StatusFilter { get; set; }

        public string OrderNoFilter { get; set; }

        public string AddressInformationNameFilter { get; set; }

        public string UserNameFilter { get; set; }

        public string HospitalNameFilter { get; set; }

        public string UserName2Filter { get; set; }

        public string WarehouseNameFilter { get; set; }

    }
}