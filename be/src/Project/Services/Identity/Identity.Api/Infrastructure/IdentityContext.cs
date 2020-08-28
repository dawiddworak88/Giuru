using Identity.Api.Infrastructure.Accounts.Entities;
using Identity.Api.Infrastructure.Addresses.Entities;
using Identity.Api.Infrastructure.Clients.Entities;
using Identity.Api.Infrastructure.Secrets.Entities;
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

        public DbSet<ApplicationUser> Accounts { get; set; }
        public DbSet<AddressClient> LinkAddressesClients { get; set; }
        public DbSet<AppSecretClient> LinkAppSecretsClients { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<AppSecret> AppSecrets { get; set; }
    }
}
