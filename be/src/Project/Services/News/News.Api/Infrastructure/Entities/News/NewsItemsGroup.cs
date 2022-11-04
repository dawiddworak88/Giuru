using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace News.Api.Infrastructure.Entities.News
{
    public class NewsItemsGroup : Entity
    {
        [Required]
        public Guid NewsItemId { get; set; }

        [Required]
        public Guid GroupId { get; set; }
    }
}
