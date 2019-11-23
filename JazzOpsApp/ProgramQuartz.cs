using JazzOpsApp.DataAccess;
using JazzOpsApp.Jobs;
using JazzOpsApp.Models;
using Microsoft.Extensions.Configuration;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace JazzOpsApp
{
    class ProgramQuartz
    {
        static void Main(string[] args)
        {       
            string server;
            if (args.Length == 0)
            {
                server = "Default";
            }
            else server = args[0];

            IConfiguration configuration = new ConfigurationBuilder()
                 .AddJsonFile("appsettings.json", true, true)
                 .Build();
            string connectionString = configuration.GetConnectionString(server);
            if (connectionString == null)
                throw new ArgumentNullException("Database connection string not found");
            Console.WriteLine("Connecting to {0}", server);
           
                SchedulerMethod(connectionString).GetAwaiter().GetResult();
           
        }
        


        private static async Task SchedulerMethod(string connectionString)
        {            
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            IScheduler scheduler = await schedulerFactory.GetScheduler();
            scheduler.Start().Wait();

            IJobDetail job = JobBuilder.Create<JazzJob>()
               .WithIdentity("myJob", "group1")
               .UsingJobData("connectionString",connectionString)
               .Build();

            ITrigger trigger = TriggerBuilder.Create()
             .WithIdentity("myTrigger", "group1")
             .StartNow()
             .WithSimpleSchedule(x => x
             .WithIntervalInMinutes(5)
             .RepeatForever())
             .Build();

            await scheduler.ScheduleJob(job, trigger);
            string key;
            do
            {
                Console.WriteLine("Wciśnij x aby wyjść.");
            } while ((key = Console.ReadLine()) != "x");


        }
    }
    
}
