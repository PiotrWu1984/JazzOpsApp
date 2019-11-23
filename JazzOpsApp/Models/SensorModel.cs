using JazzOpsApp.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace JazzOpsApp.Models
{
    public abstract class SensorModel
    {
        //public virtual bool MakeMeasure { get; set; }
        private DateTime LastMeasure { get; set; }
        public virtual int MinutesBetweenMeasures { get; set; } = 15;
        private TimeSpan span { get; set; }

        public virtual void SetLastMeasure()
        {
            LastMeasure = DateTime.Now;
        }
        public virtual void SetLastMeasure(int minutes)
        {
            LastMeasure = DateTime.Now.AddMinutes(minutes);
        }

        public virtual bool MakeMeasure()
        {
            span = DateTime.Now - LastMeasure;

            Console.WriteLine($"{DateTime.Now} : {this.GetType().Name} - ");
            int nextMeasure = CalculateNextMeasureTime();
            if (span.TotalMinutes >= MinutesBetweenMeasures)
            {
                Console.WriteLine($"{DateTime.Now.ToShortTimeString()} - Pomiar {this.GetType().Name}");
            }
            else
            {

                Console.Write($" kolejny pomiar za {nextMeasure} minut");
            }
            //Console.WriteLine($" LastMeasure: {LastMeasure}");            
            Console.WriteLine("\r\n");
            return (span.TotalMinutes >= MinutesBetweenMeasures);
        }

        public int CalculateNextMeasureTime()
        {
            double difference = MinutesBetweenMeasures - span.TotalMinutes < 0
                                        ? 0 : MinutesBetweenMeasures - span.TotalMinutes;
            int nextMeasure = difference > 5 ? (int)Math.Round(difference / 5) * 5 : 5;
            double scale = Math.Pow(10, (int)Math.Log10(difference));
            return nextMeasure;
        }

        //public override string ToString()
        //{
        //    return String.Format($"Id: {this.GetType().GetProperty("Measurements").GetValue("Station")} - Pm10: {this.Pm10} - Date: {this.MeasurementDate} - SensorID: {this.SensorId} - {this.GetType().Name}");
        //}
    }
}
