using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MostIdea.MIMGroup.B2B.Dtos;
using Newtonsoft.Json;

namespace MostIdea.MIMGroup.B2B
{
    public class VehiclesAppService : MIMGroupAppServiceBase, IVehiclesAppService
    {

        public async Task<List<VehiclePositionDto>> GetAllPositions()
        {
            using (WebClient wc = new WebClient())
            {
                return  JsonConvert.DeserializeObject<List<VehiclePositionDto>>(wc.DownloadString("http://takip.triomobil.com/soap/GetAllPositions?user=mim_api&pass=Most2015!"));
            }
        }

        public async Task<List<string>> GetVehiclesPlate()
        {
            using (WebClient wc = new WebClient())
            {
                return JsonConvert.DeserializeObject<List<VehiclePositionDto>>(wc.DownloadString("http://takip.triomobil.com/soap/GetAllPositions?user=mim_api&pass=Most2015!"))
                    .Select(x => x.license_plate)
                    .ToList();
            }
        }
    }
}
