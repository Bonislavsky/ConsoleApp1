namespace ConsoleApp1.ScheduleHandlers.Intefraces
{
    public interface IScheduleParser
    {
        public GroupSchedule Parse(string line);
    }
}
