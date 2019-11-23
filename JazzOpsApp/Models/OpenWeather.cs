using JazzOpsApp.Helpers;
using JazzOpsApp.JsonModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;

namespace JazzOpsApp.Models
{
    public class OpenWeather : SensorModel, ISensor
    {
        private string apiKey = "";
        public Measurements Measurements { get; set; }
        public override int MinutesBetweenMeasures { get; set; } = 30;

        public OpenWeather()
        {
            Measurements = new Measurements() { Station = new Station() { iotNet = new IotNet() } };
            this.SetLastMeasure();
        }
        public OpenWeather(int stationId) : this()
        {            
            Measurements.Station.StationId = stationId;
            //MakeMeasure = true;
            //LastMeasure = DateTime.Now;
        }

        public IEnumerable<Measurements> GetData()
        {
            //TimeSpan span = DateTime.Now - LastMeasure;
            var sensorList = new List<Measurements>();
           // if (MakeMeasure)
            {                
                var context = JsonHelpers.GetSensorDataAsync<OpenWeatherJsonData>("https://api.openweathermap.org/data/2.5/weather?id=" + this.Measurements.Station.StationId.ToString() + "&appid=" + apiKey);
                var data = new Measurements
                {
                    Temperature = context.main.temp,
                    Humidity = context.main.humidity,
                    Pressure = context.main.pressure,
                    MeasurementDate = UnixTimeStampToDateTime(context.dt),
                    WindSpeed = context.wind.speed,
                    WindDeg = context.wind.deg,
                    Station = new Station {
                        StationId = Measurements.Station.StationId,
                        Latitude = context.coord.lat,
                        Longitude = context.coord.lon,
                        iotNet = new IotNet() { Name = this.GetType().Name } },                                        
                };

                sensorList.Add(data);
                this.SetLastMeasure();
            }
            
            return sensorList;
        }


        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }         

    }
}

