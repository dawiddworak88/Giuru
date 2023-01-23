using Foundation.GenericRepository.Entities;
using News.Api.Infrastructure.Entities.Categories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace News.Api.Infrastructure.Entities.News
{
    public class NewsItem : Entity
    {
        public Guid? ThumbnailImageId { get; set; }

        public Guid? PreviewImageId { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        public Guid OrganisationId { get; set; }

        [Required]
        public bool IsPublished { get; set; }

        public virtual Category Category { get; set; }

        public virtual IEnumerable<NewsItemTranslation> Translations { get; set; }

        public virtual IEnumerable<NewsItemFile> Files { get; set; }
    }
}
