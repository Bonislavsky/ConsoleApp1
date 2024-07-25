using ConsoleApp1.Database;
using ConsoleApp1.ScheduleHandlers;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp1.Services.Schedules
{
    public class SchedulePowerStatusCheck : IScheduleOperation<bool?>
    {
        private readonly string _groupName;

        public SchedulePowerStatusCheck(string groupName)
        {
            _groupName = groupName;
        }

        public bool? Execute(ApplicationDbContext context)
        {
            var timeNow = DateTime.UtcNow.AddHours(3).TimeOfDay;
            var group = context.Groups
                .AsNoTracking()
                .Include(g => g.Schedules)
                .SingleOrDefault(g => g.Name.Equals(_groupName));

            if (group == null)
            {
                Console.WriteLine("Група з таким номером відсутня");
                return null;
            }

            return group.Schedules.Any(s => s.StartTime <= timeNow && s.FinishTime >= timeNow);
        }
    }
}
