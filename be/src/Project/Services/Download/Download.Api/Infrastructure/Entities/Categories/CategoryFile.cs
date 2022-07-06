using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Download.Api.Infrastructure.Entities.Categories
{
    public class CategoryFile : EntityMedia
    {
        [Required]
        public Guid CategoryId { get; set; }
    }
}
