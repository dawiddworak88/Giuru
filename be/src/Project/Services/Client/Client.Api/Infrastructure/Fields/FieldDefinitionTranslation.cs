using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Client.Api.Infrastructure.Fields
{
    public class FieldDefinitionTranslation : EntityTranslation
    {
        [Required]
        public Guid FieldDefinitionId { get; set; }

        [Required]
        public string FieldName { get; set; }
    }
}
