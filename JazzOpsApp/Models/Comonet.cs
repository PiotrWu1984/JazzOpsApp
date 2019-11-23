using JazzOpsApp.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace JazzOpsApp.Models
{
    public class Comonet : SensorModel, ISensor
    {
        public Measurements Measurements { get; set; }
        public override int MinutesBetweenMeasures { get; set; } = 0;
        public Comonet()
        {
            Measurements = new Measurements() { Station = new Station() { iotNet = new IotNet() } };
        }
        public Comonet(int sensorId) : this()
        {            
            this.Measurements.Station.StationId = sensorId;
            this.SetLastMeasure(-1);
        }

        
        public IEnumerable<Measurements> GetData()
        {
            CsvService csvService = new CsvService();
            Comonet comonet = new Comonet();
            var data = comonet.FeedMeasurementsFromCSV(csvService);
            return data;
        }
        public IEnumerable<Measurements> FeedMeasurementsFromCSV(CsvService csvService)
        {
            var csvFile = csvService.loadCsvFileFromFile(@"./comonet/data.csv");
            var list = csvService.SplitCSVLines(csvFile);
            var measurements = CreateListOfMeasurementsFromStringTable(list);

            return measurements;
        }
        public List<Measurements> CreateListOfMeasurementsFromStringTable(List<string[]> list)
        {
            var SensorList = new List<Measurements>();
            List<string> allValues = new List<string>();

            foreach (var item in list)
            {
                DateTime myDate = DateTime.ParseExact(item[0].Substring(0, 16), "yyyy-MM-dd HH:mm",
                                       CultureInfo.InvariantCulture);
                var Data = new Measurements
                {
                    MeasurementDate = myDate,
                    Pm25 = double.Parse(item[2]),
                    Pm10 = double.Parse(item[3]),
                    Temperature = double.Parse(item[4]),
                    Humidity = double.Parse(item[5]),
                    Station = new Station { StationId = Measurements.Station.StationId, iotNet = new IotNet() { Name = this.GetType().Name } }
                };
                SensorList.Add(Data);

            }

            return SensorList;
        }
    }
}
