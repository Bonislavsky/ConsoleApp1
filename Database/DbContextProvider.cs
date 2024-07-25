using ConsoleApp1.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp1.Database
{
    public static class DbContextProvider
    { 
        private static ApplicationDbContextFactory _dbContextFactory;
        const string CONNECTION_STRING = "Data Source=(Localdb)\\MSSQLLocalDB;Initial Catalog=LocalDBTest;Integrated Security=True";

        public static void Initialize()
        {
            _dbContextFactory = new ApplicationDbContextFactory(CONNECTION_STRING);
        }

        public static ApplicationDbContext GetContext()
        {
            if (_dbContextFactory == null)
                throw new InvalidOperationException("DbContextFactory is not initialized. Call Initialize first.");

            var context = _dbContextFactory.CreateDbContext();

            if (!context.Database.CanConnect())
            {
                context.Database.Migrate();
                Console.WriteLine("Database created and migrations applied.");
            }
            else
            {
                Console.WriteLine("Database already exists.");
            }

            return context;
        }

        public static void CleanDB(this ApplicationDbContext context)
        {
            using var transaction = context.Database.BeginTransaction();
            try
            {
                context.Set<Schedule>().RemoveRange(context.Set<Schedule>());
                context.Set<Address>().RemoveRange(context.Set<Address>());
                context.Set<Group>().RemoveRange(context.Set<Group>());
                context.SaveChanges();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
