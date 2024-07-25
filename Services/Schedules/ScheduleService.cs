using ConsoleApp1.Database;
using ConsoleApp1.ScheduleHandlers;

namespace ConsoleApp1.Services.Schedules
{
    public class ScheduleService
    {
        private readonly ApplicationDbContext _context;

        public ScheduleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public TResult ExecuteOperation<TResult>(IScheduleOperation<TResult> operation)
        {
            return operation.Execute(_context);
        }
    }
}
