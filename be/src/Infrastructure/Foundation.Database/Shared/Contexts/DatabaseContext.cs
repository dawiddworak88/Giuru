using Foundation.Database.Areas.Clients.Entities;
using Foundation.Database.Areas.Media.Entities;
using Foundation.Database.Areas.Orders.Entitites;
using Foundation.Database.Areas.Products.Entities;
using Foundation.Database.Areas.Schemas.Entities;
using Foundation.Database.Areas.Accounts.Entities;
using Foundation.Database.Areas.Sellers.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Foundation.Database.Areas.Taxonomies.Entities;
using Foundation.Database.Areas.Translations.Entities;
using Foundation.Database.Shared.Addresses.Entities;
using Foundation.Database.Shared.Secrets.Entities;

namespace Foundation.Database.Shared.Contexts
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
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
        public DbSet<SellerClient> LinkSellersClients { get; set; }
        public DbSet<MediaItemEntity> LinkMediaItemsEntities { get; set; }
        public DbSet<MediaItem> MediaItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderItemState> OrderItemStates { get; set; }
        public DbSet<OrderItemStatus> OrderItemStatusses { get; set; }
        public DbSet<WorkflowState> WorkflowStates { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryProduct> LinkCategoriesProducts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Schema> Schemas { get; set; }
        public DbSet<AddressSeller> LinkAddressesSellers { get; set; }
        public DbSet<AppSecretSeller> LinkAppSecretsSellers { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<Taxonomy> Taxonomies { get; set; }
        public DbSet<Translation> Translations { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<AppSecret> AppSecrets { get; set; }
    }
}
