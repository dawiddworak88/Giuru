using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Client.Api.Infrastructure.Fields
{
    public class FieldDefinition : Entity
    {
        [Required]
        public string FieldType { get; set; }

        public bool IsRequired { get; set; }

        public Guid? OptionSetId { get; set; }
    }
}
