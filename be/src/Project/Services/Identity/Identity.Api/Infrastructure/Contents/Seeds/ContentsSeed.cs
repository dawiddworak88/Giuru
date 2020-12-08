using Foundation.GenericRepository.Helpers;
using Identity.Api.Infrastructure.Contents.Definitions;
using Identity.Api.Infrastructure.Contents.Entities;
using Microsoft.Extensions.Configuration;
using System.Web;

namespace Identity.Api.Infrastructure.Contents.Seeds
{
    public static class ContentsSeed
    {
        public static void SeedContents(IdentityContext context, IConfiguration configuration)
        {
            var privacyPolicyContent = new Content
            { 
                Id = ContentConstants.PrivacyPolicyId,
                Language = "pl",
                Text = HttpUtility.UrlDecode(configuration["PrivacyPolicy"])
            };

            context.Contents.Add(EntityHelper.SeedEntity(privacyPolicyContent));

            var regulationsContent = new Content
            {
                Id = ContentConstants.RegulationsId,
                Language = "pl",
                Text = HttpUtility.UrlDecode(configuration["Regulations"])
            };

            context.Contents.Add(EntityHelper.SeedEntity(regulationsContent));

            context.SaveChanges();
        }
    }
}
