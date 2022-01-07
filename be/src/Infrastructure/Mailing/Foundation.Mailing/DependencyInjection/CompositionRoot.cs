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
            services.Configure<MailingConfiguration>(configuration);
            services.AddScoped<ISendGridClient>(_ => new SendGridClient(configuration["SendGridApiKey"]));
            services.AddScoped<IMailingService, MailingService>();
        }
    }
}
