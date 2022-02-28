using Foundation.GenericRepository.Entities;
using System;
using System.Collections.Generic;

namespace News.Api.Infrastructure.Entities.News
{
    public class NewsItemTag : Entity
    {
        public Guid TagId { get; set; }
        public Guid NewsItemId { get; set; }
    }
}
