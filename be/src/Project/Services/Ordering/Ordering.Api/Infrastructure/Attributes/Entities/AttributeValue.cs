using Foundation.GenericRepository.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ordering.Api.Infrastructure.Attributes.Entities
{
    public class AttributeValue : Entity
    {
        [Required]
        public Guid AttributeId { get; set; }

        [Required]
        public Guid OrderId { get; set; }

        public Guid? OrderItemId { get; set; }

        public virtual IEnumerable<AttributeValueTranslation> AttributeValueTranslations { get; set; }
    }
}
