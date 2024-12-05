using Abp.Application.Services.Dto;
using System;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class GetAllSalesConsultantsForExcelInput
    {
        public string Filter { get; set; }

        public string UserNameFilter { get; set; }

        public string UserName2Filter { get; set; }

    }
}