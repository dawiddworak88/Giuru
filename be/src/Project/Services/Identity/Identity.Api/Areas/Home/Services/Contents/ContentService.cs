using Identity.Api.Infrastructure;
using System;
using System.Threading.Tasks;

namespace Identity.Api.Areas.Home.Services.Contents
{
    public class ContentService : IContentService
    {
        private readonly IdentityContext context;

        public ContentService(IdentityContext context)
        {
            this.context = context;
        }

        public async Task<string> GetAsync(Guid id, string language)
        {
            return string.Empty;
        }
    }
}
