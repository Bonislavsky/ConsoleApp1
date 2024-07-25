using ConsoleApp1.Database;
using ConsoleApp1.ScheduleHandlers;
using Microsoft.EntityFrameworkCore;
using Group = ConsoleApp1.Database.Models.Group;

namespace ConsoleApp1.Services.Schedules
{
    public class ScheduleDataViewer : IScheduleOperation<List<Group>>
    {
        public List<Group> Execute(ApplicationDbContext context)
        {
            return context.Groups.AsNoTracking().Include(g => g.Schedules).ToList();
        }
    }
}
