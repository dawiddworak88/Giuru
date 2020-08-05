using Foundation.GenericRepository.Entities;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Api.Infrastructure.Translations.Entities
{
    public class Translation  : Entity
    {
        [Required]
        public string Key { get; set; }

        [Required]
        public string Value { get; set; }

        [Required]
        public string Language { get; set; }
    }
}
