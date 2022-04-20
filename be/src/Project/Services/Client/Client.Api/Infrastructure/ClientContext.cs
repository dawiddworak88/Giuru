using Client.Api.Infrastructure.Clients.Entities;
using Client.Api.Infrastructure.Groups.Entities;
using Microsoft.EntityFrameworkCore;

namespace Client.Api.Infrastructure
{
    public class ClientContext : DbContext
    {
        public ClientContext(DbContextOptions<ClientContext> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        public DbSet<Clients.Entities.Client> Clients { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupTranslation> GroupTranslations { get; set; }
        public DbSet<ClientsGroups> ClientsGroups { get; set; }
    }
}
