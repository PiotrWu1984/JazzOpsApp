using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JazzOpsApp.JsonModels
{
    public partial class AirlyJsonData2
    {
        [JsonProperty("currentMeasurements")]
        public CurrentMeasurementsClass CurrentMeasurements { get; set; }

        [JsonProperty("history")]
        public History[] History { get; set; }

        [JsonProperty("forecast")]
        public Forecast[] Forecast { get; set; }

        public Cords[] cords { get; set; }

        //public static AirlyJsonData FromJson(string json) => JsonConvert.DeserializeObject<AirlyJsonData>(json, Converter.Settings);
    }

    public class Cords
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }

    public partial class CurrentMeasurementsClass
    {
        [JsonProperty("airQualityIndex")]
        public double AirQualityIndex { get; set; }

        [JsonProperty("pm1")]
        public double Pm1 { get; set; }

        [JsonProperty("pm25")]
        public double Pm25 { get; set; }

        [JsonProperty("pm10")]
        public double Pm10 { get; set; }

        [JsonProperty("pressure")]
        public double? Pressure { get; set; }

        [JsonProperty("humidity")]
        public double? Humidity { get; set; }

        [JsonProperty("temperature")]
        public double? Temperature { get; set; }

        [JsonProperty("pollutionLevel")]
        public long PollutionLevel { get; set; }
    }

    public partial class Forecast
    {
        [JsonProperty("fromDateTime")]
        public DateTimeOffset FromDateTime { get; set; }

        [JsonProperty("tillDateTime")]
        public DateTimeOffset TillDateTime { get; set; }

        [JsonProperty("measurements")]
        public ForecastMeasurements Measurements { get; set; }
    }

    public partial class ForecastMeasurements
    {
        [JsonProperty("pollutionLevel")]
        public long PollutionLevel { get; set; }
    }

    public partial class History
    {
        [JsonProperty("fromDateTime")]
        public DateTimeOffset FromDateTime { get; set; }

        [JsonProperty("tillDateTime")]
        public DateTimeOffset TillDateTime { get; set; }

        [JsonProperty("measurements")]
        public CurrentMeasurementsClass Measurements { get; set; }
    }
}
