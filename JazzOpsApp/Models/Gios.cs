using JazzOpsApp.Helpers;
using JazzOpsApp.JsonModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace JazzOpsApp.Models
{
    public class Gios : SensorModel, ISensor
    {      
        public Measurements Measurements { get; set; }
        public override int MinutesBetweenMeasures { get; set; } = 60;

        public Gios()
        {
            Measurements = new Measurements() { Station = new Station() { iotNet = new IotNet() } };
        }
        public Gios(int stationId) : this()
        {
            Measurements.Station.StationId = stationId;
            this.SetLastMeasure();
            //MakeMeasure = true;
        }
        public Gios(int stationId, double lat, double lon) : this(stationId)
        {
            Measurements.Station.Latitude = lat;
            Measurements.Station.Longitude = lon;
        }

        public IEnumerable<Measurements> GetData()
        {
            var sensorList = new List<Measurements>();
            //TimeSpan span = DateTime.Now - LastMeasure;
            //Console.WriteLine($"Od ostatniego pomiaru GIOS minęło: {span.Minutes.ToString()}");
            //if (MakeMeasure)
            {                
                var context = JsonHelpers.GetSensorDataAsync<GiosJsonData>("http://api.gios.gov.pl/pjp-api/rest/data/getData/" + this.Measurements.Station.StationId.ToString());
                var historyData = context.Values;
                if (historyData==null)
                    historyData = Array.Empty<Value>();
                foreach (var item in historyData)
                {
                    if (item.PM10Value != null)
                    {
                        var data = new Measurements()
                        {                            
                            MeasurementDate = item.Date,
                            Pm10 = item.PM10Value,
                            Station = new Station { StationId = Measurements.Station.StationId, iotNet = new IotNet() { Name = this.GetType().Name } }
                            
                        };
                        sensorList.Add(data);
                    }
                }
                this.SetLastMeasure();
            }
            return sensorList;        
        }
    }
}
