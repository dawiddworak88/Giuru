using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Client.Api.Infrastructure.Fields
{
    public class OptionSetTranslation : EntityTranslation
    {
        [Required]
        public Guid OptionSetId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
