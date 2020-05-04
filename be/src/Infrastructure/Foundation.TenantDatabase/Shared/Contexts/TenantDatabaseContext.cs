using Foundation.TenantDatabase.Areas.Accounts.Entities;
using Foundation.TenantDatabase.Areas.Clients.Entities;
using Foundation.TenantDatabase.Areas.Media.Entities;
using Foundation.TenantDatabase.Areas.Products.Entities;
using Foundation.TenantDatabase.Areas.Schemas.Entities;
using Foundation.TenantDatabase.Areas.Taxonomies.Entities;
using Foundation.TenantDatabase.Areas.Translations.Entities;
using Foundation.TenantDatabase.Shared.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Foundation.TenantDatabase.Shared.Contexts
{
    public class TenantDatabaseContext : IdentityDbContext<ApplicationUser>
    {
        public TenantDatabaseContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ApplicationUser> Accounts { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<EntityType> EntityTypes { get; set; }
        public DbSet<MediaItem> MediaItems { get; set; }
        public DbSet<LinkMediaItem> LinkMediaItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Translation> Translations { get; set; }
        public DbSet<Schema> Schemas { get; set; }
        public DbSet<SchemaField> SchemaFields { get; set; }
        public DbSet<SchemaFieldValue> SchemaFieldValues { get; set; }
        public DbSet<Taxonomy> Taxonomies { get; set; }
    }
}
