using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace News.Api.Infrastructure.Entities.News
{
    public class NewsItemFile : EntityMedia
    {
        [Required]
        public Guid NewsItemId { get; set; }
    }
}
