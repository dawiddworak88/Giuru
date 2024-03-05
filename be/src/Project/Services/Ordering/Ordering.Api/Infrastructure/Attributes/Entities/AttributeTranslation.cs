using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Ordering.Api.Infrastructure.Attributes.Entities
{
    public class AttributeTranslation : EntityTranslation
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public Guid AttributeId { get; set; }
    }
}
