using Identity.Api.Infrastructure.Accounts.Entities;
using Identity.Api.Infrastructure.Addresses.Entities;
using Identity.Api.Infrastructure.Clients.Entities;
using Identity.Api.Infrastructure.Organisations.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Api.Infrastructure
{
    public class IdentityContext : IdentityDbContext<ApplicationUser>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        public DbSet<Organisation> Organisations { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<ApplicationUser> Accounts { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<AddressOrganisation> LinkAddressesOrganisations { get; set; }
        public DbSet<AppSecretOrganisation> AppSecretsOrganisations { get; set; }
        public DbSet<Client> Clients { get; set; }
    }
}
