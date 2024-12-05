using System;
using Abp.Application.Services.Dto;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class TaxRateDto : EntityDto<Guid>
    {
        public string Name { get; set; }

        public decimal Rate { get; set; }

    }
}