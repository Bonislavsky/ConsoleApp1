using ConsoleApp1.Database;
using ConsoleApp1.Database.Models;
using ConsoleApp1.ScheduleHandlers;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp1.Services.Schedules
{
    public class ScheduleEditor : IScheduleOperation<bool>
    {
        private readonly int _groupNumber;
        private readonly TimeSpan _newStartTime;
        private readonly TimeSpan _newFinishTime;

        public ScheduleEditor(int groupNumber, TimeSpan newStartTime, TimeSpan newFinishTime)
        {
            _groupNumber = groupNumber;
            _newStartTime = newStartTime;
            _newFinishTime = newFinishTime;
        }

        public bool Execute(ApplicationDbContext context)
        {
            var group = context.Groups
                .Include(g => g.Schedules)
                .SingleOrDefault(g => g.Name.Equals(_groupNumber.ToString()));

            if (group == null || group.Schedules == null)
            {
                Console.WriteLine("Група з таким номером відсутня");
                return false;
            }

            var newSchedule = new Schedule
            {
                Day = "day",
                StartTime = _newStartTime,
                FinishTime = _newFinishTime,
                GroupId = group.Id,
            };

            var toRemove = group.Schedules.Where(s => s.StartTime.Hours < _newFinishTime.Hours && s.FinishTime.Hours > _newStartTime.Hours);
            context.Schedules.RemoveRange(toRemove);
            context.Schedules.Add(newSchedule);

            context.SaveChanges();
            return true;
        }
    }
}
