using ConsoleApp1.Database;
using ConsoleApp1.Database.Models;
using ConsoleApp1.ScheduleHandlers;
using ConsoleApp1.ScheduleHandlers.Intefraces;

namespace ConsoleApp1.ScheduleOperations
{
    public class ScheduleImporter : IScheduleOperation<bool>
    {
        private readonly string _filePath;
        private readonly IScheduleParser _scheduleParser;

        public ScheduleImporter(string filePath, IScheduleParser scheduleParser)
        {
            _filePath = filePath;
            _scheduleParser = scheduleParser;
        }

        public bool Execute(ApplicationDbContext context)
        {
            context.CleanDB();
            var schedules = ImportSchedulesFromFile(_filePath);

            if (!schedules.Any())
            {
                Console.WriteLine("Пустий файл");
                return false;
            }

            var newSchedules = new List<Schedule>();
            foreach (var schedule in schedules)
            {
                var group = context.Groups.SingleOrDefault(g => g.Name == schedule.GroupNumber.ToString());
                if (group == null)
                {
                    group = new Group
                    {
                        Description = "Description",
                        Name = schedule.GroupNumber.ToString()
                    };
                    context.Groups.Add(group);
                    context.SaveChanges();
                }

                foreach (var time in schedule.OffPeriods)
                {
                    newSchedules.Add(new Schedule
                    {
                        Day = "day",
                        StartTime = time.Start,
                        FinishTime = time.End,
                        GroupId = group.Id
                    });
                }
            }

            context.Schedules.AddRange(newSchedules);
            context.SaveChanges();
            return true;
        }

        private List<GroupSchedule> ImportSchedulesFromFile(string filePath)
        {
            var groupSchedules = new List<GroupSchedule>();
            try
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var schedule = _scheduleParser.Parse(line);
                    if (schedule != null)
                    {
                        groupSchedules.Add(schedule);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while reading the file: " + ex.Message);
            }
            return groupSchedules;
        }
    }
}