using ConsoleApp1.Database;
using ConsoleApp1.Database.Models;
using ConsoleApp1.ScheduleHandlers;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;

namespace ConsoleApp1.Services.Schedules
{
    public class ScheduleExport : IScheduleOperation<string>
    {
        private readonly string? _groupName;

        public ScheduleExport(string? groupName)
        {
            _groupName = groupName;
        }

        public string Execute(ApplicationDbContext context)
        {
            var groups = context.Groups
                .AsNoTracking()
                .Include(g => g.Schedules)
                .Where(g => _groupName.IsNullOrEmpty() || g.Name.Equals(_groupName))
                .Select(g => new GroupDTO(g))
                .ToList();

            string path = @"D:\Group_in_json.json";
            string jsonString = JsonSerializer.Serialize(groups);
            File.WriteAllText(path, jsonString);

            return path;
        }
    }

    public class GroupDTO
    {
        public GroupDTO(Group group)
        {
            GroupNumber = group.Name;
            Schedules = group.Schedules.Select(s => new ScheduleDTO(s)).ToList();
        }
        public string GroupNumber { get; set; }
        public List<ScheduleDTO> Schedules { get; set; }
    }

    public class ScheduleDTO
    {
        public ScheduleDTO(Schedule schedule)
        {
            StartTime = schedule.StartTime;
            FinishTime = schedule.FinishTime;
        }
        public TimeSpan StartTime { get; set; }
        public TimeSpan FinishTime { get; set; }
    }
}
