using Client.Api.Infrastructure.Managers.Entities;
using Client.Api.Infrastructure.Clients.Entities;
using Client.Api.Infrastructure.Groups.Entities;
using Client.Api.Infrastructure.Roles.Entities;
using Microsoft.EntityFrameworkCore;
using Client.Api.Infrastructure.Notifications.Entities;

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
        public DbSet<ClientNotificationType> ClientNotificationTypes { get; set; }
        public DbSet<ClientNotificationTypeTranslations> ClientNotificationTypeTranslations { get; set; }
        public DbSet<ClientNotificationTypeApproval> ClientNotificationTypeApprovals { get; set; }
        public DbSet<ClientNotification> ClientNotifications { get; set; }
        public DbSet<ClientNotificationTranslation> ClientNotificationsTranslations { get; set; }
    }
}
