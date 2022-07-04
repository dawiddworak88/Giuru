using Microsoft.EntityFrameworkCore;
using System;

namespace Download.Api.Infrastructure
{
    public class DownloadContext : DbContext
    {
        public DownloadContext(DbContextOptions<DownloadContext> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
