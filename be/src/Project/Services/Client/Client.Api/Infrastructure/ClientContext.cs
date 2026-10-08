using Client.Api.Infrastructure.Managers.Entities;
using Client.Api.Infrastructure.Clients.Entities;
using Client.Api.Infrastructure.DiscountCodes.Entities;
using Client.Api.Infrastructure.Groups.Entities;
using Client.Api.Infrastructure.Roles.Entities;
using Microsoft.EntityFrameworkCore;
using Client.Api.Infrastructure.Fields;

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
        public DbSet<ClientsApplication> ClientsApplications { get; set; }
        public DbSet<ClientsApplicationAddress> ClientsApplicationAddresses { get; set; }
        public DbSet<ClientGroup> ClientGroups { get; set; }
        public DbSet<ClientGroupTranslation> ClientGroupTranslations { get; set; }
        public DbSet<ClientsGroup> ClientsGroups { get; set; }
        public DbSet<ClientRole> ClientRoles { get; set; }
        public DbSet<ClientAccountManager> ClientAccountManagers { get; set; }
        public DbSet<ClientsAccountManagers> ClientsAccountManagers { get; set; }
        public DbSet<ClientFieldValue> ClientFieldValues { get; set; }
        public DbSet<ClientFieldValueTranslation> ClientFieldValueTranslations { get; set; }
        public DbSet<FieldDefinition> FieldDefinitions { get; set; }
        public DbSet<FieldDefinitionTranslation> FieldDefinitionTranslations { get; set; }
        public DbSet<Option> FieldOptions { get; set; }
        public DbSet<OptionTranslation> FieldOptionTranslations { get; set; }
        public DbSet<OptionSet> FieldOptionSets { get; set; }
        public DbSet<DiscountCode> DiscountCodes { get; set; }
        public DbSet<ClientsDiscountCode> ClientsDiscountCodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Case-insensitive, accent-sensitive, so that matching a code does not depend on the database default collation.
            modelBuilder.Entity<DiscountCode>()
                .Property(x => x.Code)
                .UseCollation("Latin1_General_100_CI_AS");

            modelBuilder.Entity<DiscountCode>()
                .HasIndex(x => new { x.SellerId, x.Code })
                .IsUnique()
                .HasFilter("[IsActive] = 1");

            modelBuilder.Entity<ClientsDiscountCode>()
                .HasIndex(x => new { x.ClientId, x.DiscountCodeId })
                .IsUnique()
                .HasFilter("[IsActive] = 1");

            modelBuilder.Entity<ClientsDiscountCode>()
                .HasOne<Clients.Entities.Client>()
                .WithMany()
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClientsDiscountCode>()
                .HasOne<DiscountCode>()
                .WithMany()
                .HasForeignKey(x => x.DiscountCodeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
