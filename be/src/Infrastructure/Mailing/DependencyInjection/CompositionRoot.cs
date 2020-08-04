using Foundation.Mailing.Configurations;
using Foundation.Mailing.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SendGrid;

namespace Foundation.Mailing.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterMailingDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            var mailingConfigurationSection = configuration.GetSection("Mailing");
            services.Configure<MailingConfiguration>(mailingConfigurationSection);
            var mailingConfiguration = mailingConfigurationSection.Get<MailingConfiguration>();
            services.AddScoped<ISendGridClient>(_ => new SendGridClient(mailingConfiguration.ApiKey));
            services.AddScoped<IMailingService, MailingService>();
        }
    }
}
