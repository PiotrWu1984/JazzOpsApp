using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace JazzOpsApp.Models
{
    public partial class Armaag : SensorModel, ISensor
    {
        public override int Id { get; set; }
        public override DateTime? MeasurementDate { get; set; }
        public override double? Pm10 { get; set; }
        public override int SensorId { get; set; }
        public override double Latitude { get; set; }
        public override double Longitude { get; set; }
        

        public Armaag()
        {
            //LastMeasure = DateTime.Now;
        }
        public Armaag(DateTime date, double pm10, int armaagId)
        {
            this.MeasurementDate = date;
            this.Pm10 = pm10;
            this.SensorId = armaagId;
        }
        public IEnumerable<Measurements> GetData()
        {
            var measurements = new List<SensorModel>();
            for (int days = 1; days >= 0; days--)
            {                
                WebClient webClient = new WebClient();
                string station = SensorId.ToString();
                DateTime dateOfMeasurement = DateTime.Now.AddDays(-days);
                string month = (dateOfMeasurement.Month.ToString().Length == 1) ? ("0" + dateOfMeasurement.Month.ToString()) : dateOfMeasurement.Month.ToString();
                string day = (dateOfMeasurement.Day.ToString().Length == 1) ? ("0" + dateOfMeasurement.Day.ToString()) : dateOfMeasurement.Day.ToString();
                string dateForUrl = dateOfMeasurement.Year.ToString() + "-" + month + "-" + day;
                string page = webClient.DownloadString("http://armaag.gda.pl/komunikat1h.htm?station=" + station + "&date=" + dateForUrl + "&go=poka%BF");

                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(page);
                var tableHeaders = doc.DocumentNode.SelectSingleNode("//table[@class='meteotable']")
                            .Descendants("tr")                           
                            .Where(tr => tr.Elements("th").Count() > 1)
                            .Select(tr => tr.Elements("th").Select(td => td.InnerText.Trim()
                            ).ToList())
                            .ToList();
                var table = doc.DocumentNode.SelectSingleNode("//table[@class='meteotable']")
                            .Descendants("tr")
                            .Skip(1)
                            .Where(tr => tr.Elements("td").Count() > 1)
                            .Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim().Replace(":00", "")
                            ).ToList())
                            .ToList();
                int hourIndex = tableHeaders[0].IndexOf("godzina");
                int pm10Index = tableHeaders[0].IndexOf("PM10");
                for (int i = 0; i < table.Count; i++)
                {
                    int hour = Int32.Parse(table[i][hourIndex].ToString()) - 1;
                    if (string.IsNullOrEmpty(table[i][pm10Index]))
                        continue;
                    measurements.Add(new Armaag(new DateTime(dateOfMeasurement.Year, dateOfMeasurement.Month, Int32.Parse(dateOfMeasurement.Day.ToString()), hour, 0, 0), Convert.ToDouble(table[i][pm10Index].Replace(".", ",")), SensorId)); //aby zadziałało lokalnie należy zamienić kropkę na przecinek, w tej wersji działa na serwerze Azure

                }
            }
            return measurements;
        }

        public int GetSensorId()
        {
            return this.SensorId;
        }
    }
}
