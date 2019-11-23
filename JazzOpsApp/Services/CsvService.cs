using JazzOpsApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;

namespace JazzOpsApp.Services
{
    public class CsvService
    {
        public List<string> loadCsvFileFromUrl(string strURL)
        {
            try
            {
                WebResponse objResponse;
                WebRequest objRequest = HttpWebRequest.Create(strURL);
                objResponse = objRequest.GetResponse();
                using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                {
                    List<string> lines = new List<string>();
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine();
                        lines.Add(line);
                    }
                    return lines;
                }
            }
            catch (Exception)
            {

                return new List<string>() { "" };
            }


        }
        public List<string> loadCsvFileFromFile(string path)
        {
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    List<string> lines = new List<string>();
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine();
                        lines.Add(line);
                    }
                    return lines;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<string>() { "" };
            }


        }
        public List<string[]> SplitCSVLines(List<string> lines)
        {
            List<string[]> csvData = new List<string[]>();
            for (int i = 1; i < lines.Count; i++)
            {
                var data = lines[i].Split(";");
                csvData.Add(data);

            }
            return csvData;
        }
        
    }
}
