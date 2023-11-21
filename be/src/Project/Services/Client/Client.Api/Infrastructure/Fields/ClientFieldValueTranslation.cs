using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Client.Api.Infrastructure.Fields
{
    public class ClientFieldValueTranslation : EntityTranslation
    {
        [Required]
        public Guid ClientFieldValueId { get; set; }

        [Required]
        public string FieldValue { get; set; }
    }
}
