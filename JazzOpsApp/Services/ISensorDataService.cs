using JazzOpsApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JazzOpsApp.Services
{
    public interface ISensorDataService
    {
        IEnumerable<Measurements> GetSensorData();
        void AddSensorData(List<Measurements> sensors);
        void UpdateStationInDB(ISensor sensor);
        bool IsDataInDB(Measurements measurements);
        Measurements GetLast(ISensor sensor);
        IEnumerable<ISensor> GetSensorId(IEnumerable<ISensor> sensor);
        void UpdateSensorList(IEnumerable<ISensor> sensors);
        
    }
}
