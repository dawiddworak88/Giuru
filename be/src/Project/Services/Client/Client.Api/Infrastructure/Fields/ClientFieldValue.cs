using Foundation.GenericRepository.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Client.Api.Infrastructure.Fields
{
    public class ClientFieldValue : Entity
    {
        [Required]
        public Guid ClientId { get; set; }

        [Required]
        public Guid FieldDefinitionId { get; set; }

        public virtual IEnumerable<ClientFieldValueTranslation> Translation { get; set; }
        public virtual FieldDefinition FieldDefinition { get; set; }
    }
}
