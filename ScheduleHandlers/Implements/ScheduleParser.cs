using ConsoleApp1.ScheduleHandlers.Intefraces;
using System.Globalization;

namespace ConsoleApp1.ScheduleHandlers.Implements
{
    public class ScheduleParser : IScheduleParser
    {
        public GroupSchedule Parse(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                return null;
            }

            var parts = line.Split('.', 2);
            if (parts.Length != 2 || !int.TryParse(parts[0], out int groupNumber))
            {
                Console.WriteLine("Invalid format: " + line);
                return null;
            }

            var groupSchedule = new GroupSchedule { GroupNumber = groupNumber };
            var periods = parts[1].Split(';');

            foreach (var period in periods)
            {
                var times = period.Split('-', 2);
                if (times.Length != 2 ||
                    !TimeSpan.TryParseExact(times[0].Trim(), "hh\\:mm", CultureInfo.InvariantCulture, out TimeSpan startTime) ||
                    !TimeSpan.TryParseExact(times[1].Trim(), "hh\\:mm", CultureInfo.InvariantCulture, out TimeSpan endTime))
                {
                    Console.WriteLine("Invalid format: " + line);
                    return null;
                }

                groupSchedule.OffPeriods.Add((startTime, endTime));
            }

            return groupSchedule;
        }
    }
}
