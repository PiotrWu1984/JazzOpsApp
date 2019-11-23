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
    public class Imgw : SensorModel, ISensor
    {
        public Measurements Measurements { get; set; }
        public override int MinutesBetweenMeasures { get; set; } = 60;

        public Imgw()
        {
            Measurements = new Measurements() { Station = new Station() { iotNet = new IotNet() } };
            this.SetLastMeasure();
        }
        public Imgw(int sensorId) : this()
        {
            this.Measurements.Station.StationId = sensorId;
            this.SetLastMeasure();
        }

        public IEnumerable<Measurements> GetData()
        {
            var sensorList = new List<Measurements>();
            //TimeSpan span = DateTime.Now - LastMeasure;
            //if (MakeMeasure)
            {
                var context = JsonHelpers.GetSensorDataAsync<ImgwJsonData>("https://danepubliczne.imgw.pl/api/data/synop/id/" + this.Measurements.Station.StationId.ToString());
                TimeSpan czas = TimeSpan.Parse(context.godzina_pomiaru.ToString() + ":00");

                var data = new Measurements
                {
                    MeasurementDate = context.data_pomiaru+czas,
                    Temperature = context.temperatura,
                    Humidity = context.wilgotnosc_wzgledna,
                    WindSpeed = context.predkosc_wiatru,
                    WindDeg = context.kierunek_wiatru,
                    Pressure = context.cisnienie,
                    Precipitation = context.suma_opadu,
                    Station=new Station { StationId = Measurements.Station.StationId, iotNet = new IotNet() { Name = this.GetType().Name } }
                };
                sensorList.Add(data);
                this.SetLastMeasure();
            }
            
            return sensorList;
        }             

       
    }
}
