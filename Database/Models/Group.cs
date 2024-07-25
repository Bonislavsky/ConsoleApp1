namespace ConsoleApp1.Database.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Address> Addresses { get; set; }
        public List<Schedule> Schedules { get; set; }
    }
}
