using Foundation.GenericRepository.Entities;
using System;

namespace News.Api.Infrastructure.Entities.News
{
    public class NewsItemFile : EntityMedia
    {
        public Guid NewsItemId { get; set; }
    }
}
