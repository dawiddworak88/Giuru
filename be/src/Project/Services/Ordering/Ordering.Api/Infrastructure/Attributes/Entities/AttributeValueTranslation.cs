using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Ordering.Api.Infrastructure.Attributes.Entities
{
    public class AttributeValueTranslation : EntityTranslation
    {
        [Required]
        public Guid AttributeValueId { get; set; }

        [Required]
        public string Value { get; set; }
    }
}
