using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using MostIdea.MIMGroup.B2B.Dtos;

namespace MostIdea.MIMGroup.B2B
{
    public class MobileAppService : MIMGroupAppServiceBase, IMobileAppService
    {
        private readonly IRepository<Order, Guid> _orderRepository;
        private readonly IRepository<OrderItem, Guid> _orderItemRepository;
        private readonly IRepository<Product, Guid> _productRepository;
        private readonly IRepository<HospitalGroup, Guid> _hospitalGroupRepository;
        private readonly IRepository<Hospital, Guid> _hospitalRepository;
        private readonly IRepository<City, Guid> _cityRepository;

        public MobileAppService(IRepository<Order, Guid> orderRepository, IRepository<OrderItem, Guid> orderItemRepository, IRepository<Product, Guid> productRepository, IRepository<HospitalGroup, Guid> hospitalGroupRepository, IRepository<Hospital, Guid> hospitalRepository, IRepository<City, Guid> cityRepository)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _productRepository = productRepository;
            _hospitalGroupRepository = hospitalGroupRepository;
            _hospitalRepository = hospitalRepository;
            _cityRepository = cityRepository;
        }

        public async Task<MobileDashboardViewModel> GetMobileDashboardViewModel()
        {
            var output = new MobileDashboardViewModel();

            output.Banners = new List<string>()
            {
                "https://mostidea.com.tr/wp-content/uploads/2022/01/Slider1.png",
                "https://mostidea.com.tr/wp-content/uploads/2022/01/Mostidea-Metaverse-nedir.jpg"
            };


            output.MostOrderedProducts =
                ObjectMapper.Map(_productRepository.GetAll().Take(5).ToList(), new List<ProductDto>());

            output.RecentOrders =
                ObjectMapper.Map(_orderRepository.GetAll().Include(x => x.OrderItems).Take(5).ToList(),
                    new List<OrderDto>());

            


            return output;
        }

        public async Task<MobileRegisterViewModel> GetMobileRegisterViewModel()
        {
            var output = new MobileRegisterViewModel();

            output.HospitalGroups = ObjectMapper.Map(await _hospitalGroupRepository.GetAll().Include(x => x.Hospitals.Take(10)).ToListAsync(), new List<HospitalGroupDto>());
            output.Cities = ObjectMapper.Map(await _cityRepository.GetAll().ToListAsync(), new List<CityDto>());

            return output;
        }

    }
}
