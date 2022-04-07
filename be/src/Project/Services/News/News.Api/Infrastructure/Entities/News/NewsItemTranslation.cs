using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace News.Api.Infrastructure.Entities.News
{
    public class NewsItemTranslation : Entity
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string Language { get; set; }

        [Required]
        public Guid NewsItemId { get; set; }
    }
}
