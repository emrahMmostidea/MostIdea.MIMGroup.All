using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class GetSalesConsultantForEditOutput
    {
        public CreateOrEditSalesConsultantDto SalesConsultant { get; set; }

        public string UserName { get; set; }

        public string UserName2 { get; set; }

    }
}