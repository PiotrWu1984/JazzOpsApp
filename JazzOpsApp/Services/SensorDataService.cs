using JazzOpsApp.DataAccess;
using JazzOpsApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JazzOpsApp.Services
{
    public class SensorDataService : ISensorDataService
    {
        private JazzDbContext _context;
        private List<Measurements> _measurements;


        public SensorDataService(JazzDbContext context)
        {
            _context = context;
        }
        public void AddSensorData(List<Measurements> sensors)
        {
            _measurements = sensors;

            foreach (var item in _measurements)
            {
                _context.Add(item);
            }
            _context.SaveChanges();
        }

        public void UpdateStationInDB(ISensor sensor)
        {
            if (sensor != null)
            {
                _context.Update(sensor.Measurements.Station);
                _context.SaveChanges();
            }
            
        }

        public IEnumerable<Measurements> GetSensorData()
        {
            return Enumerable.Empty<Measurements>();
        }

        public bool IsDataInDB(Measurements measurements)
        {
            return _context.Measurements.Any(s => s.MeasurementDate == measurements.MeasurementDate && s.Station.StationId == measurements.Station.StationId && s.Station.iotNet.Name == measurements.Station.iotNet.Name);
        }

        public Measurements GetLast(ISensor sensor)
        {
            var data = _context.Measurements.Where(s => s.Station.Id == sensor.Measurements.Station.Id).OrderByDescending(d => d.MeasurementDate).FirstOrDefault();
            return data;
        }

        public IEnumerable<ISensor> GetSensorId(IEnumerable<ISensor> sensors)
        {
            foreach (var s in sensors)
            {
                var x = _context.Stations.Where(a => a.StationId == s.Measurements.Station.StationId && a.iotNet.Name == s.GetType().Name).FirstOrDefault();
                s.Measurements.Station = x ?? s.Measurements.Station;
            }
            return sensors;
        }

        public void UpdateSensorList(IEnumerable<ISensor> sensors)
        {
            foreach (var s in sensors)
            {
                if (!_context.Stations.Where(x => x.StationId == s.Measurements.Station.StationId
                        && x.iotNet.Name == s.GetType().Name
                        ).Any())
                {
                    s.Measurements.Station.iotNet = _context.IoTNets.Where(n => n.Name == s.GetType().Name).FirstOrDefault() ?? new IotNet() { Name = s.GetType().Name };
                    _context.Add(s.Measurements.Station);
                }
                _context.SaveChanges();
            }
            
        }


    }
}
