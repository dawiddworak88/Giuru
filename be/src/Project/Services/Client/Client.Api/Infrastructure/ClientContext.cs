using Client.Api.Infrastructure.Managers.Entities;
using Client.Api.Infrastructure.Clients.Entities;
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
        public DbSet<ClientGroup> ClientGroups { get; set; }
        public DbSet<ClientGroupTranslation> ClientGroupTranslations { get; set; }
        public DbSet<ClientsGroup> ClientsGroups { get; set; }
        public DbSet<ClientRole> ClientRoles { get; set; }
        public DbSet<ClientAccountManager> ClientAccountManagers { get; set; }
        public DbSet<ClientsAccountManagers> ClientsAccountManagers { get; set; }
        public DbSet<ClientFieldValue> ClientFieldValues { get; set; }
        public DbSet<ClientFieldValueTranslation> ClientFieldValuesTranslation { get; set; }
        public DbSet<FieldDefinition> FieldDefinitions { get; set; }
        public DbSet<FieldDefinitionTranslation> FieldDefinitionTranslations { get; set; }
        public DbSet<Option> FieldOptions { get; set; }
        public DbSet<OptionSet> FieldOptionSets { get; set; }
    }
}
