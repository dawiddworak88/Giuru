using Foundation.GenericRepository.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ordering.Api.Infrastructure.Attributes.Entities
{
    public class Attribute : Entity
    {
        [Required]
        public string Type { get; set; }

        public bool IsRequired { get; set; }

        public bool IsOrderItemAttribute { get; set; }

        public Guid? AttributeOptionSetId { get; set; }

        public virtual IEnumerable<AttributeTranslation> AttributeTranslations { get; set; }
    }
}
