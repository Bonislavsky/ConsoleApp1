using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Database.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string AddressValue { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}
