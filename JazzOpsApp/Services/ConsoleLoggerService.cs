using JazzOpsApp.Services;

namespace JazzOpsApp.Services
{
    internal class ConsoleLoggerService : ILoggerService
    {
        public void AddLogItem(string text)
        {
            System.Console.WriteLine(text);
        }
    }
}