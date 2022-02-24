using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace News.Api.Infrastructure.Entities.News
{
    public class NewsItemImage : EntityMedia
    {
        public Guid NewsItemId { get; set; }

        [Required]
        public bool IsHero { get; set; }
    }
}
