using Foundation.GenericRepository.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ordering.Api.Infrastructure.Attributes.Entities
{
    public class AttributeOption : Entity
    {
        [Required]
        public Guid AttributeOptionSetId { get; set; }

        public virtual AttributeOptionSet AttributeOptionSet { get; set; }

        public virtual IEnumerable<AttributeOptionTranslation> AttributeOptionTranslations { get; set; }
    }
}
