using JazzOpsApp.DataAccess;
using System;
using System.Collections.Generic;

namespace JazzOpsApp.Models
{
    public partial class AirQ : SensorModel, ISensor
    {      

        public override int Id { get; set; }
        public override DateTime? MeasurementDate { get; set; }
        public override int SensorId { get; set; }
        public override double? Pm10 { get; set; }
        

        public IEnumerable<SensorModel> GetData()
        {
            var list = new List<SensorModel>();
            AirQConnect dBConnect = new AirQConnect();
            var fromDate = DateTime.Today.Date - TimeSpan.FromDays(1);
            var select = "SELECT date, id, case when id = 1 then 3401 when id = 2 then 3432 when id = 3 then 3336 when id = 4 then 3426 end as AirlyId, left(time, 2) as Hour, avg(pm10) as PM10_avg FROM `pw_data` WHERE date >= '" + fromDate.ToShortDateString() + "' group by id, date, left(time, 2)";
            var data = dBConnect.Select(select);
            
            for (int i = 0; i < data.Count; i++)
            {                
                list.Add(new AirQ()
                {
                    //TODO: połączenie daty z godziną
                    MeasurementDate = DateTime.Parse(data[i][0]),
                    SensorId = Int32.Parse(data[i][1]),
                    //AirlyId = Int32.Parse(data[i][2]),
                    // Hour = Int32.Parse(data[i][3]),
                    //PM10Min = double.Parse(data[i][4]),
                    Pm10 = double.Parse(data[i][3]),
                    //PM10Max = double.Parse(data[i][6])
                });               
            }
            return list;
        }

        public int GetSensorId()
        {
            return this.SensorId;
        }
    }
}
