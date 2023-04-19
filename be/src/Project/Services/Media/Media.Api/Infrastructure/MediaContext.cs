using Media.Api.Infrastructure.Media.Entities;
using Microsoft.EntityFrameworkCore;

namespace Media.Api.Infrastructure
{
    public class MediaContext : DbContext
    {
        public MediaContext(DbContextOptions<MediaContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        public DbSet<MediaItem> MediaItems { get; set; }
        public DbSet<MediaItemsGroup> MediaItemsGroups { get; set; }
        public DbSet<MediaItemTranslation> MediaItemTranslations { get; set; }
        public DbSet<MediaItemVersion> MediaItemVersions { get; set; }
    }
}
