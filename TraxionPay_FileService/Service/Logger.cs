using System.Diagnostics;
using TraxionPay_FileService.Interface;

namespace TraxionPay_FileService.Service
{
    public class Logger : ILogger
    {
        public readonly string source;
        public Logger(string sourceName)
        {
            source = sourceName;
        }
      
        public void LogInformation(string message)
        {
            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = source;
                eventLog.WriteEntry(message, EventLogEntryType.Information, 101, 1);
            }
        }
       
        public void LogError(string message)
        {
            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = source;
                eventLog.WriteEntry(message, EventLogEntryType.Error, 101, 1);
            }
        }
    }
} 
