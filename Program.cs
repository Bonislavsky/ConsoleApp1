using ConsoleApp1.Database;
using ConsoleApp1.Database.Models;
using ConsoleApp1.ScheduleHandlers.Implements;
using ConsoleApp1.ScheduleOperations;
using ConsoleApp1.Services.Schedules;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DbContextProvider.Initialize();
            var context = DbContextProvider.GetContext();
                
            var scheduleService = new ScheduleService(context);

            while (true)
            {
                switch (PrintMenu())
                {
                    case "1":
                        PrintImporterTxt(scheduleService);
                        break;
                    case "2":
                        PrintStatusCheck(scheduleService);
                        break;
                    case "3":
                        PrintEditor(scheduleService);
                        break;
                    case "4":
                        PrintExportJson(scheduleService);
                        break;
                    case "5":
                        PrintDataViewer(scheduleService);
                        break;
                    case "6":
                        return;
                    default:
                        Console.WriteLine("Неправильний вибір. Спробуйте знову.");
                        break;
                }
            }
        }

        static string PrintMenu()
        {
            Console.WriteLine("\nВиберіть дію:");
            Console.WriteLine("1. Імпорт даних з txt");
            Console.WriteLine("2. перевірити наявність світла у групі");
            Console.WriteLine("3. Редагування даних у групах");
            Console.WriteLine("4. Експорт графіків у JSON");
            Console.WriteLine("5. дивитися графiки");
            Console.WriteLine("6. Вихід");

            Console.Write("Ваш вибiр:");
            return Console.ReadLine();
        }

        static void PrintExportJson(ScheduleService scheduleService)
        {
            Console.Write("Введiть номер группи: ");
            string? groupNumber = Console.ReadLine();

            Console.WriteLine(scheduleService.ExecuteOperation(new ScheduleExport(groupNumber)));
        }

        static void PrintDataViewer(ScheduleService scheduleService)
        {
            var groups = scheduleService.ExecuteOperation(new ScheduleDataViewer());

            foreach (var group in groups)
            {
                Console.WriteLine($"\nгруппа №{group.Name}");
                for (int i = 0; i < 24; i++)
                {
                    string color = IsInOffPeriod(i, group.Schedules) ? "Red" : "Green";
                    Console.ForegroundColor = color == "Red" ? ConsoleColor.Red : ConsoleColor.Green;
                    Console.Write($"{i} ");
                    Console.ResetColor();
                }
            }
            Console.WriteLine();

            bool IsInOffPeriod(int hour, List<Schedule> periods)
            {
                TimeSpan time = new TimeSpan(hour, 0, 0);
                foreach (var period in periods)
                {
                    if (time >= period.StartTime && time <= period.FinishTime)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        
        static void PrintEditor(ScheduleService scheduleService)
        {
            Console.Write("Введiть номер группи: ");
            string groupNumber = Console.ReadLine();

            Console.Write("Введiть час початку вiдключення: ");
            string start = Console.ReadLine();

            Console.Write("Введiть час закiнчення вiдключення: ");
            string end = Console.ReadLine();

            if (int.TryParse(groupNumber, out int id) && TimeSpan.TryParse(start, out TimeSpan startTime) && TimeSpan.TryParse(end, out TimeSpan endTime))
            {
                var editOperation = new ScheduleEditor(id, startTime, endTime);
                scheduleService.ExecuteOperation(editOperation);
            }
            else
            {
                Console.WriteLine("Невірний формат часу чи номеру группи.");
            }
        }

        static void PrintStatusCheck(ScheduleService scheduleService)
        {
            Console.Write("Введiть номер группи: ");
            string groupNumber = Console.ReadLine();

            bool? hasPowerOff = scheduleService.ExecuteOperation(new SchedulePowerStatusCheck(groupNumber));

            Console.WriteLine(hasPowerOff == null ? string.Empty :
                (hasPowerOff == true ? $"в данный момент группа #{groupNumber} без свiтла" : "Лас-Вегас"));
        }

        static void PrintImporterTxt(ScheduleService scheduleService)
        {
            Console.Write("Введіть шлях до файлу txt: ");
            string filePath = Console.ReadLine();

            var scheduleParser = new ScheduleParser();
            var importOperation = new ScheduleImporter(filePath, scheduleParser);
            scheduleService.ExecuteOperation(importOperation);
        }
    }
}
