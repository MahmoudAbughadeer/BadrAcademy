using System.Diagnostics;

namespace Shared.Utilities
{
    public class clsLogger
    {
        private static readonly string SourceName = "BadrAcademy";
        private static readonly string LogName = "Application";

        static clsLogger()
        {
            if (!EventLog.SourceExists(SourceName))
            {
                EventLog.CreateEventSource(SourceName, LogName);
            }
        }

        public static void LogError(string message)
        {
            EventLog.WriteEntry(SourceName, message, EventLogEntryType.Error);
        }

        public static void LogWarning(string message)
        {
            EventLog.WriteEntry(SourceName, message, EventLogEntryType.Warning);
        }

        public static void LogInfo(string message)
        {
            EventLog.WriteEntry(SourceName, message, EventLogEntryType.Information);
        }

        public static void Log(string message, EventLogEntryType type)
        {
            EventLog.WriteEntry(SourceName, message, type);
        }
    }
}
