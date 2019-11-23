using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JazzOpsApp.Services
{
    public class FileLoggerService : ILoggerService
    {
        public void AddLogItem(string text)
        {
            var today = DateTime.Today.Date.ToShortDateString();
            if (!Directory.Exists("log"))
            {
                Directory.CreateDirectory("log");
            }            
            File.AppendAllText($"log\\log_{today}.txt", text);
        }
    }
}
