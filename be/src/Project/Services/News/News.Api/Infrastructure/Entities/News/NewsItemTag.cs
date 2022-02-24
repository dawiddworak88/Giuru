using Foundation.GenericRepository.Entities;
using System.Collections.Generic;

namespace News.Api.Infrastructure.Entities.News
{
    public class NewsItemTag : Entity
    {
        public virtual IEnumerable<NewsItemTagTranslation> Translations { get; set; }
    }
}
