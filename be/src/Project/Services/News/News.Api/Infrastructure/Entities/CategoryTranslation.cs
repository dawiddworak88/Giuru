
using Foundation.GenericRepository.Entities;
using System.ComponentModel.DataAnnotations;
using System;

namespace News.Api.Infrastructure.Entities
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
