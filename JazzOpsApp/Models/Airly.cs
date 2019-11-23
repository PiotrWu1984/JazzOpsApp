using JazzOpsApp.Helpers;
using JazzOpsApp.JsonModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;

namespace JazzOpsApp.Models
{
    public partial class Airly : SensorModel, ISensor
    {
        private string apiKey = "";
        public Measurements Measurements { get; set; }
        public override int MinutesBetweenMeasures { get; set; } = 60;

        public Airly()
        {
            Measurements = new Measurements() { Station = new Station() { iotNet = new IotNet() } };
            this.SetLastMeasure();
        }
        public Airly(string apiKey) : this()
        {
            this.apiKey = apiKey;            
        }
        public Airly(int stationId, double lat, double lon) : this()
        {            
            Measurements.Station.StationId = stationId;
            Measurements.Station.Latitude = lat;
            Measurements.Station.Longitude = lon;
        }

        public IEnumerable<Measurements> GetData()
        {
            var airlySensorList = new List<Measurements>();            
            {                
                var lat = this.Measurements.Station.Latitude.GetValueOrDefault().ToString(new CultureInfo("en-US"));
                var lon = this.Measurements.Station.Longitude.GetValueOrDefault().ToString(new CultureInfo("en-US"));
                var context = JsonHelpers.GetSensorDataAsync<AirlyJsonData>("https://airapi.airly.eu/v2/measurements/installation?installationId=" + Measurements.Station.StationId+"&apikey=" + apiKey);

                var historyData = context.History;
                if (historyData[0].Values == null)
                    historyData = Array.Empty<Current>();
                foreach (var item in historyData)
                {
                    foreach (var value in item.Values)
                    {
                        if (value.Name == Pollutant.Pm10) Measurements.Pm10 = value.ValueValue;
                        if (value.Name == Pollutant.Pm25) Measurements.Pm25 = value.ValueValue;
                        if (value.Name == Pollutant.Humidity) Measurements.Humidity = value.ValueValue;
                        if (value.Name == Pollutant.Temperature) Measurements.Temperature = value.ValueValue;
                        if (value.Name == Pollutant.Pressure) Measurements.Pressure = value.ValueValue;
                    }
                    var data = new Measurements
                    {
                        Pm10 = Measurements.Pm10,
                        Pm25 = Measurements.Pm25,
                        Humidity = Measurements.Humidity,
                        Temperature = Measurements.Temperature,
                        Pressure = Measurements.Pressure,
                        MeasurementDate = item.FromDateTime.DateTime,
                        Station = new Station { StationId = Measurements.Station.StationId, iotNet = new IotNet() { Name = this.GetType().Name } },
                    };
                    
                    airlySensorList.Add(data);

                }
                this.SetLastMeasure();
            }            
            return airlySensorList;
        }
                

    }


}
