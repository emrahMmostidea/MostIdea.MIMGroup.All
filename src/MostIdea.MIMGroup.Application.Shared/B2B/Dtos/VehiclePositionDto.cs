using System;
using System.Collections.Generic;
using System.Text;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class VehiclePositionDto
    {
        public string license_plate { get; set; }
        public int id { get; set; }
        public string timestamp { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int speed { get; set; }
        public int mileage { get; set; }
        public bool ignition_on { get; set; }
        public string address { get; set; }
    }
}
