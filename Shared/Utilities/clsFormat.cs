using System;

namespace Shared.Utilities
{
    public static class clsFormat
    {
        public static string TimeNoLeadingZero(string time)
        {
            if (string.IsNullOrWhiteSpace(time))
                return time;

            string[] parts = time.Trim().Split(':');

            if (parts.Length < 2)
                return time;

            if (!int.TryParse(parts[0], out int hour) ||
                !int.TryParse(parts[1], out int minute))
                return time;

            // Convert to 12-hour format
            hour = hour % 12;
            
            if (hour == 0)
                hour = 12;

            return minute == 0
                ? hour.ToString()
                : $"{hour}:{minute:D2}";
        }
    }
}