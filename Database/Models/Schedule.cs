using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Database.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public string Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan FinishTime { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}
