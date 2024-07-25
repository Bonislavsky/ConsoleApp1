using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace ConsoleApp1.Database
{
    public class ApplicationDbContextFactory
    {
        private readonly IHost _host;

        public ApplicationDbContextFactory(string connectionString)
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.ConfigureDbContext(connectionString);
                })
                .Build();
        }

        public ApplicationDbContext CreateDbContext()
        {
            var scope = _host.Services.CreateScope();
            return scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        }
    }

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddLogging(logging =>
            {
                logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
            });
            return services;
        }
    }
}