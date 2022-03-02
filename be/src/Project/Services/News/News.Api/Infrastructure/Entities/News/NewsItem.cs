using Foundation.GenericRepository.Entities;
using System;
using System.Collections.Generic;

namespace News.Api.Infrastructure.Entities.News
{
    public class NewsItem : Entity
    {
        public Guid ThumbImageId { get; set; }
        public Guid HeroImageId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid OrganisationId { get; set; }
        public bool IsPublished { get; set; }
        public virtual IEnumerable<NewsItemTranslation> Translations { get; set; }
    }
}
