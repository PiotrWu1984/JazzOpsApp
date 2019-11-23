using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JazzOpsApp.JsonModels
{
    public partial class GiosJsonData
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("values")]
        public Value[] Values { get; set; }
    }

    public class Value
    {
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("value")]
        public double? PM10Value { get; set; }
    }
}
