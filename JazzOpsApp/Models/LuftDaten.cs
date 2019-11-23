using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using CsvHelper;
using JazzOpsApp.Helpers;
using JazzOpsApp.JsonModels;

namespace JazzOpsApp.Models
{
    public partial class LuftDaten : SensorModel, ISensor
    {
        public Measurements Measurements { get; set; }
        //public DateTime LastMeasure { get; private set; }
        public override int MinutesBetweenMeasures { get; set;  } = 0;

        public LuftDaten()
        {
            Measurements = new Measurements() { Station=new Station() {iotNet = new IotNet() } };
            this.SetLastMeasure();
        }
        public LuftDaten(int stationId) : this()
        {
            this.Measurements.Station.StationId = stationId;                        
        }

                
        
        public IEnumerable<Measurements> GetData()
        {
            var sensorList = new List<Measurements>();
                       
            {
                var context = JsonHelpers.GetSensorDataAsync<List<LuftDatenJsonData>>("http://api.luftdaten.info/v1/sensor/" + this.Measurements.Station.StationId.ToString() + "/");
                foreach (var item in context)
                {
                    var data = new Measurements
                    {
                        MeasurementDate = item.Timestamp,
                        Pm25 = item.Sensordatavalues[1].Value,
                        Pm10 = item.Sensordatavalues[0].Value,                        
                        Station = new Station()
                                    { StationId = this.Measurements.Station.StationId,
                                        iotNet =new IotNet {Name=this.GetType().Name } }
                    };
                    sensorList.Add(data);
                    
                }
                this.SetLastMeasure();
            }
           
            return sensorList;
        }
                
    }


}
