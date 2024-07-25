namespace ConsoleApp1.ScheduleHandlers
{
    public class GroupSchedule
    {
        public int GroupNumber { get; set; }
        public List<(TimeSpan Start, TimeSpan End)> OffPeriods { get; set; } = new List<(TimeSpan Start, TimeSpan End)>();
    }
}
