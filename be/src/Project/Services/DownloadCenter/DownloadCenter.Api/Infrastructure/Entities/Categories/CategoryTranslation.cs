using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace DownloadCenter.Api.Infrastructure.Entities.Categories
{
    public class CategoryTranslation : Entity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Language { get; set; }

        [Required]
        public Guid CategoryId { get; set; }
    }
}
