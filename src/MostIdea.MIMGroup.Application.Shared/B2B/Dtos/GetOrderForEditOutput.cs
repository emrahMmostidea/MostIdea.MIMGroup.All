using Abp.Application.Services.Dto;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class GetOrderForEditOutput
    {
        public CreateOrEditOrderDto Order { get; set; }

        public string AddressInformationName { get; set; }

        public string UserName { get; set; }

        public string HospitalName { get; set; }

        public string UserName2 { get; set; }

        public string WarehouseName { get; set; }

    }
}