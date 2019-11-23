using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace JazzOpsApp.JsonModels
{
    public class LuftDatenJsonData
    {
        [JsonProperty("sensordatavalues")]
        public Sensordatavalue[] Sensordatavalues { get; set; }

        [JsonProperty("sampling_rate")]
        public object SamplingRate { get; set; }

        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("sensor")]
        public Sensor Sensor { get; set; }
        
    }

    public class Location
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("altitude")]
        public double Altitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }
    }

    public class Sensor
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("pin")]       
        public long Pin { get; set; }

        [JsonProperty("sensor_type")]
        public SensorType SensorType { get; set; }
    }

    public class SensorType
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("manufacturer")]
        public string Manufacturer { get; set; }
    }

    public class Sensordatavalue
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("value_type")]
        public string ValueType { get; set; }

        [JsonProperty("value")]
        public double Value { get; set; }
    }

}
