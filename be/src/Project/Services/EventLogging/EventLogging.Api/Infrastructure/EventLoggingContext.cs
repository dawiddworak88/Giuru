using EventLogging.Api.Infrastructure.Events.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventLogging.Api.Infrastructure
{
    public class EventLoggingContext : DbContext
    {
        public EventLoggingContext(DbContextOptions<EventLoggingContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        public DbSet<EventLog> EventLogs { get; set; }
    }
}
