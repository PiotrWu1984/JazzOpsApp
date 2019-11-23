using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Globalization;

namespace JazzOpsApp.JsonModels
{
    public class AirlyJsonData
    {
        [JsonProperty("current")]
        public Current Current { get; set; }

        [JsonProperty("history")]
        public Current[] History { get; set; }

        [JsonProperty("forecast")]
        public Current[] Forecast { get; set; }
    }

    public class Current
    {
        [JsonProperty("fromDateTime")]
        public DateTimeOffset FromDateTime { get; set; }

        [JsonProperty("tillDateTime")]
        public DateTimeOffset TillDateTime { get; set; }

        [JsonProperty("values")]
        public AirlyValue[] Values { get; set; }

        [JsonProperty("indexes")]
        public Index[] Indexes { get; set; }

        [JsonProperty("standards")]
        public Standard[] Standards { get; set; }
    }

    public class Index
    {
        [JsonIgnore]
        [JsonProperty("name")]
        public IndexName Name { get; set; }

        [JsonIgnore]
        [JsonProperty("value2")]
        public double Value { get; set; }

        [JsonIgnore]
        [JsonProperty("level")]
        public Level Level { get; set; }

        [JsonIgnore]
        [JsonProperty("description")]
        public Description Description { get; set; }

        [JsonIgnore]
        [JsonProperty("advice")]
        public string Advice { get; set; }

        [JsonIgnore]
        [JsonProperty("color")]
        public Color Color { get; set; }
    }

    public class Standard
    {
        [JsonIgnore]
        [JsonProperty("name")]
        public StandardName Name { get; set; }

        [JsonProperty("pollutant")]
        public Pollutant Pollutant { get; set; }

        [JsonProperty("limit")]
        public long Limit { get; set; }

        [JsonProperty("percent")]
        public double Percent { get; set; }
    }

    public class AirlyValue
    {
        [JsonProperty("name")]
        public Pollutant Name { get; set; }

        [JsonProperty("value")]
        public double ValueValue { get; set; }
    }

    public enum Color { D1Cf1E, The6Bc926 };

    public enum Description { AirIsQuiteGood, GreatAirHereToday };

    public enum Level { Low, VeryLow };

    public enum IndexName { AirlyCaqi };

    public enum StandardName { Who };

    public enum Pollutant { Humidity, Pm1, Pm10, Pm25, Pressure, Temperature };

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                ColorConverter.Singleton,
                DescriptionConverter.Singleton,
                LevelConverter.Singleton,
                IndexNameConverter.Singleton,
                StandardNameConverter.Singleton,
                PollutantConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class ColorConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Color) || t == typeof(Color?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "#6BC926":
                    return Color.The6Bc926;
                case "#D1CF1E":
                    return Color.D1Cf1E;
            }
            throw new Exception("Cannot unmarshal type Color");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Color)untypedValue;
            switch (value)
            {
                case Color.The6Bc926:
                    serializer.Serialize(writer, "#6BC926");
                    return;
                case Color.D1Cf1E:
                    serializer.Serialize(writer, "#D1CF1E");
                    return;
            }
            throw new Exception("Cannot marshal type Color");
        }

        public static readonly ColorConverter Singleton = new ColorConverter();
    }

    internal class DescriptionConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Description) || t == typeof(Description?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Air is quite good.":
                    return Description.AirIsQuiteGood;
                case "Great air here today!":
                    return Description.GreatAirHereToday;
            }
            throw new Exception("Cannot unmarshal type Description");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Description)untypedValue;
            switch (value)
            {
                case Description.AirIsQuiteGood:
                    serializer.Serialize(writer, "Air is quite good.");
                    return;
                case Description.GreatAirHereToday:
                    serializer.Serialize(writer, "Great air here today!");
                    return;
            }
            throw new Exception("Cannot marshal type Description");
        }

        public static readonly DescriptionConverter Singleton = new DescriptionConverter();
    }

    internal class LevelConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Level) || t == typeof(Level?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "LOW":
                    return Level.Low;
                case "VERY_LOW":
                    return Level.VeryLow;
            }
            throw new Exception("Cannot unmarshal type Level");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Level)untypedValue;
            switch (value)
            {
                case Level.Low:
                    serializer.Serialize(writer, "LOW");
                    return;
                case Level.VeryLow:
                    serializer.Serialize(writer, "VERY_LOW");
                    return;
            }
            throw new Exception("Cannot marshal type Level");
        }

        public static readonly LevelConverter Singleton = new LevelConverter();
    }

    internal class IndexNameConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(IndexName) || t == typeof(IndexName?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "AIRLY_CAQI")
            {
                return IndexName.AirlyCaqi;
            }
            throw new Exception("Cannot unmarshal type IndexName");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (IndexName)untypedValue;
            if (value == IndexName.AirlyCaqi)
            {
                serializer.Serialize(writer, "AIRLY_CAQI");
                return;
            }
            throw new Exception("Cannot marshal type IndexName");
        }

        public static readonly IndexNameConverter Singleton = new IndexNameConverter();
    }

    internal class StandardNameConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(StandardName) || t == typeof(StandardName?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "WHO")
            {
                return StandardName.Who;
            }
            throw new Exception("Cannot unmarshal type StandardName");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (StandardName)untypedValue;
            if (value == StandardName.Who)
            {
                serializer.Serialize(writer, "WHO");
                return;
            }
            throw new Exception("Cannot marshal type StandardName");
        }

        public static readonly StandardNameConverter Singleton = new StandardNameConverter();
    }

    internal class PollutantConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Pollutant) || t == typeof(Pollutant?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "HUMIDITY":
                    return Pollutant.Humidity;
                case "PM1":
                    return Pollutant.Pm1;
                case "PM10":
                    return Pollutant.Pm10;
                case "PM25":
                    return Pollutant.Pm25;
                case "PRESSURE":
                    return Pollutant.Pressure;
                case "TEMPERATURE":
                    return Pollutant.Temperature;
            }
            throw new Exception("Cannot unmarshal type Pollutant");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Pollutant)untypedValue;
            switch (value)
            {
                case Pollutant.Humidity:
                    serializer.Serialize(writer, "HUMIDITY");
                    return;
                case Pollutant.Pm1:
                    serializer.Serialize(writer, "PM1");
                    return;
                case Pollutant.Pm10:
                    serializer.Serialize(writer, "PM10");
                    return;
                case Pollutant.Pm25:
                    serializer.Serialize(writer, "PM25");
                    return;
                case Pollutant.Pressure:
                    serializer.Serialize(writer, "PRESSURE");
                    return;
                case Pollutant.Temperature:
                    serializer.Serialize(writer, "TEMPERATURE");
                    return;
            }
            throw new Exception("Cannot marshal type Pollutant");
        }

        public static readonly PollutantConverter Singleton = new PollutantConverter();
    }
}
