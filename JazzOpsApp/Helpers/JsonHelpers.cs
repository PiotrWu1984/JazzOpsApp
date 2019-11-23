using JazzOpsApp.JsonModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;

namespace JazzOpsApp.Helpers
{
    public static class JsonHelpers
    {
        public static T GetSensorDataAsync<T>(string uri) where T : new()
        {
            using (var webClient = new WebClient())
            {
                var response = string.Empty;
                try
                {
                    response = webClient.DownloadString(uri);
                }
                catch (WebException e)
                {
                    Console.WriteLine(e.ToString());                    
                }
                return !string.IsNullOrEmpty(response) ? JsonConvert.DeserializeObject<T>(response) : new T();
            }
        }

        public static string SerializeToJson(this AirlyJsonData self) => JsonConvert.SerializeObject(self, Settings);

        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
