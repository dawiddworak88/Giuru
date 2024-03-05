using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Ordering.Api.Infrastructure.Attributes.Entities
{
    public class AttributeOptionTranslation : EntityTranslation
    {
        [Required]
        public Guid AttributeOptionId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
