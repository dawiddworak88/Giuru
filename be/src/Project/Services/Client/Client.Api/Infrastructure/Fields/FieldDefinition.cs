using Foundation.GenericRepository.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Client.Api.Infrastructure.Fields
{
    public class FieldDefinition : Entity
    {
        [Required]
        public string FieldType { get; set; }

        public bool IsRequired { get; set; }

        public Guid? OptionSetId { get; set; }

        public virtual IEnumerable<FieldDefinitionTranslation> FieldDefinitionTranslations { get; set; }
    }
}
