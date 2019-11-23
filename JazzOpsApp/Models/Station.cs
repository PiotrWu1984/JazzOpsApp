using System;
using System.Collections.Generic;
using System.Text;

namespace JazzOpsApp.Models
{
    public class Station
    {
        public int Id { get; set; }
        public IotNet iotNet { get; set; }
        public int StationId { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
