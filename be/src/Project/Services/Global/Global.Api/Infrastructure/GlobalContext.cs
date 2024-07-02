using Global.Api.Infrastructure.Entities.Countries;
using Global.Api.Infrastructure.Entities.Currencies;
using Global.Api.Infrastructure.Entities.Settings;
using Microsoft.EntityFrameworkCore;

namespace Global.Api.Infrastructure
{
    public class GlobalContext : DbContext
    {
        public GlobalContext(DbContextOptions<GlobalContext> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<CountryTranslation> CountryTranslations { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<CurrencyTranslation> CurrenciesTranslations { get; set; }
        public DbSet<Setting> Settings { get; set; }
    }
}
