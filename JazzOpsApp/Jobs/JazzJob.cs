using JazzOpsApp.DataAccess;
using JazzOpsApp.Models;
using JazzOpsApp.Services;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazzOpsApp.Jobs
{
    class JazzJob : IJob
    {
        List<ILoggerService> loggerService = new List<ILoggerService>()
        { new FileLoggerService(), new ConsoleLoggerService()};
        StringBuilder sb = new StringBuilder();
        private static int counter = 0;
        static List<Measurements> sensorsData;
        static IEnumerable<ISensor> sensors = new List<ISensor>
            {
                new Airly(stationId:3366, lat:54.32289, lon:18.63318),
                new Airly(stationId:3401, lat:54.34147, lon:18.64776),
                new Airly(stationId:3432, lat:54.34854, lon:18.64966),
                new Airly(stationId:3426, lat:54.37333, lon:18.61919),
                new Airly(stationId:3382, lat:54.34232, lon:18.66199),
                new Airly(stationId:3403, lat:54.35514, lon:18.63292),
                new Airly(stationId:2963, lat:54.34309, lon:18.60807),
                new Airly(stationId:3458, lat:54.38085, lon:18.60137),
                new Airly(stationId:3478, lat:54.40022, lon:18.66909),
                new Airly(stationId:3387, lat:54.40976, lon:18.56401),
                new Airly(stationId:3476, lat:54.52133, lon:18.53865),
                new Airly(stationId:3389, lat:54.54196, lon:18.46555),
                new Airly(stationId:3488, lat:54.55789, lon:18.47214),
                new Airly(stationId:2674, lat:54.48528, lon:18.51831),
                //////new Armaag{SensorId=1, Latitude=54.35333, Longitude=18.63527},
                //////new Armaag{SensorId=2, Latitude=54.36777, Longitude=18.70111},
                //////new Armaag{SensorId=8, Latitude=54.38027, Longitude=18.62027},
                new Gios(stationId:4681, lat:54.353336, lon:18.635283), //4681 - PM10 dla 729 AM1 Gdańsk Śródmieście
                new Gios(stationId:4695, lat:54.367778, lon:18.701111), //4695 - PM10 dla 730 AM2 Gdańsk Stogi
                new Gios(stationId:4706, lat:54.400833, lon:18.657497), //4706 - PM10 dla 731 AM3 Gdańsk Nowy Port
                new Gios(stationId:4715, lat:54.560836, lon:18.493331), //4715 - PM10 dla 732 AM4 Gdynia Pogórze
                new Gios(stationId:4738, lat:54.431667, lon:18.579722), //4738 - PM10 dla 734 AM6 Sopot
                new Gios(stationId:4761, lat:54.380279, lon:18.620274), //4761 - PM10 dla 736 AM8 Gdańsk Wrzeszcz
                new Gios(stationId:4774, lat:54.465758, lon:18.464911), //4774 - PM10 dla 738 AM9 Gdynia Dąbrowa
                new Gios(stationId:4793, lat:54.525274, lon:18.536382), //4793 - PM10 dla 739 AM10 Gdynia Śródmieście

                ////new AirQ{},
                //new LuftDaten(stationId:18667),
                //new LuftDaten(stationId:18070),
                //new LuftDaten(stationId:18873),
                //new LuftDaten(stationId:19105),
                //new LuftDaten(stationId:19731),
                //new LuftDaten(stationId:16725),
                //new LuftDaten(stationId:16729),
                //new LuftDaten(stationId:17582),
                //new LuftDaten(stationId:17684),
                //new LuftDaten(stationId:17161),
                //new LuftDaten(stationId:16715),
                //new LuftDaten(stationId:18255),
                //new OpenWeather(stationId:3099434),
                //new OpenWeather(stationId:7531890),
                //new OpenWeather(stationId:7531002),
                //new Imgw(12155)
            };
        static ISensorDataService sensorDataService;

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                JobDataMap dataMap = context.JobDetail.JobDataMap;
                string conn = dataMap.GetString("connectionString");
                sensorDataService = new SensorDataService(new JazzDbContext(conn));
                sensorDataService.UpdateSensorList(sensors); //sprawdź czy sensor jest w bazie, jeśli nie to dodaj
                sensorsData = new List<Measurements>();
                sensors = sensorDataService.GetSensorId(sensors); //zaktualizuj Id sensora
                counter++;
                
                sb.Append($"Przebieg: {counter.ToString()} \r\n");
                foreach (var s in sensors)
                {
                    var last = sensorDataService.GetLast(s) ?? new Measurements() { Station = new Station() { iotNet = new IotNet() } };
                    var measurements = (s as SensorModel).MakeMeasure() ? s.GetData() : Enumerable.Empty<Measurements>();
                    foreach (var item in measurements)
                    {
                        item.Station.Id = s.Measurements.Station.Id;
                        if (last.Station.Id == 0 || (item.MeasurementDate > last.MeasurementDate && s.Measurements.Station.Id == last.Station.Id))
                        {
                            sensorsData.Add(item);
                            sb.Append($"    Dodaję odczyt {s.GetType().Name} - {s.Measurements.Station.StationId} \r\n");
                        }
                    }
                }

                sb.Append($"Dodano odczytów: {sensorsData.Count} \r\n");
                sb.Append($"Koniec przebiegu: {counter.ToString()} \r\n\n\n");
                sensorDataService.AddSensorData(sensorsData);
            }
            catch (Exception e)
            {
                sb.Append(e.InnerException.Message);
                sb.Append("\r\n");
                sb.Append($"{e.Message} \r\n");
            }

            foreach (var item in loggerService)
            {
                item.AddLogItem(sb.ToString());
            }
        }
    }
}
