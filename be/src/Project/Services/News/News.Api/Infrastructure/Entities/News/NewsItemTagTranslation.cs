using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace News.Api.Infrastructure.Entities.News
{
    public class NewsItemTagTranslation : Entity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Language { get; set; }

        [Required]
        public Guid NewsItemId { get; set; }
    }
}
