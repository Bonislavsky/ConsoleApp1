using ConsoleApp1.Database;

namespace ConsoleApp1.ScheduleHandlers
{
    public interface IScheduleOperation<TResult>
    {
        TResult Execute(ApplicationDbContext context);
    }
}
