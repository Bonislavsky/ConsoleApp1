using ConsoleApp1.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp1.Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Schedule> Schedules { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>()
                .HasOne(a => a.Group)
                .WithMany(g => g.Addresses)
                .HasForeignKey(a => a.GroupId);

            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Group)
                .WithMany(g => g.Schedules)
                .HasForeignKey(s => s.GroupId);
        }
    }
}
