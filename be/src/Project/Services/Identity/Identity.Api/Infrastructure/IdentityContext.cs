using Identity.Api.Infrastructure.Accounts.Entities;
using Identity.Api.Infrastructure.Addresses.Entities;
using Identity.Api.Infrastructure.Approvals.Entities;
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
        public DbSet<OrganisationTranslation> OrganisationTranslations { get; set; }
        public DbSet<OrganisationImage> OrganisationImages { get; set; }
        public DbSet<OrganisationVideo> OrganisationVideos { get; set; }
        public DbSet<OrganisationFile> OrganisationFiles { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<ApplicationUser> Accounts { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<OrganisationAddress> OrganisationAddreses { get; set; }
        public DbSet<OrganisationAppSecret> OrganisationAppSecrets { get; set; }
        public DbSet<Approval> Approvals { get; set; }
        public DbSet<ApprovalTranslation> ApprovalTranslations { get; set; }
        public DbSet<UserApproval> UserApprovals { get; set; }
    }
}
