using JazzOpsApp.DataAccess;
using JazzOpsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JazzOpsApp.Services
{
    public class FakeSensorDataService : ISensorDataService
    {
        private JazzDbContext _context;

        public FakeSensorDataService()
        {
            
        }

        List<Measurements> tempList = new List<Measurements>
        {
        new Measurements()
        {
             Pm10=14,
             Station = new Station()
             {
                 StationId = 4576,
                 iotNet = new IotNet
                 {
                     Name="AirQ"
                 }

             }
        } };

    



        public void AddSensorData(List<Measurements> sensors)
        {
            foreach (var item in sensors)
            {
                _context.Add(item);
            }
            _context.SaveChanges();
        }

        public IEnumerable<Measurements> GetSensorData()
        {
            return tempList;
        }

        public bool IsDataInDB(DateTime? dateTime, int? id)
        {
            return false;
        }

        public bool IsDataInDB(Measurements measurements)
        {
            return false;
        }

        public void UpdateStationInDB(ISensor sensor)
        {
            throw new NotImplementedException();
        }

        public Measurements GetLast(ISensor sensor)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ISensor> GetSensorId(IEnumerable<ISensor> sensor)
        {
            return sensor;
        }

        public void UpdateSensorList(IEnumerable<ISensor> sensors)
        {
            throw new NotImplementedException();
        }
    }
}
