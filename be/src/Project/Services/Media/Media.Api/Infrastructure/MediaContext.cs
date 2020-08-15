using Microsoft.EntityFrameworkCore;

namespace Media.Api.Infrastructure
{
    public class MediaContext : DbContext
    {
        public MediaContext(DbContextOptions<MediaContext> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
