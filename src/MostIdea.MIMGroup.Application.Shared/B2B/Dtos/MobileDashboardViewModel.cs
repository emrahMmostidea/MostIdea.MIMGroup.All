using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class MobileDashboardViewModel
    {
        public List<string> Banners { get; set; }

        public List<ProductDto> MostOrderedProducts { get; set; }

        public List<OrderDto> RecentOrders { get; set; } 


    }

    public class MobileRegisterViewModel
    {
        public List<HospitalGroupDto> HospitalGroups { get; set; }

        public List<CityDto> Cities { get; set; }
    }
}
