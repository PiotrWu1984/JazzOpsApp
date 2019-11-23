using System;
using System.Collections.Generic;
using System.Text;

namespace JazzOpsApp.Models
{
    public class Measurements
    {
        public int Id { get; set; }
        public Station Station { get; set; }
        public DateTime MeasurementDate { get; set; }
        public double? Temperature { get; set; }
        public double? Humidity { get; set; }
        public double? Pressure { get; set; }
        public double? WindSpeed { get; set; }
        public int? WindDeg { get; set; }
        public double? Precipitation { get; set; }
        public double? Pm25 { get; set; }
        public double? Pm10 { get; set; }        
    }
}
