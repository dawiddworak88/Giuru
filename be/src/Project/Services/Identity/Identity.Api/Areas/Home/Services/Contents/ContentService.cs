using Identity.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography.X509Certificates;
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
            string content = string.Empty;

            var contentEntity = await this.context.Contents.FirstOrDefaultAsync(x => x.Id == id && x.Language == language && x.IsActive);

            if (contentEntity == null)
            { 
                contentEntity = await this.context.Contents.FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
            }

            if (contentEntity != null)
            {
                return contentEntity.Text;
            }

            return content;
        }
    }
}
