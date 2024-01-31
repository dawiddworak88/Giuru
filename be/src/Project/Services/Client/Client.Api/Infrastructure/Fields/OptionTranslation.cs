using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Client.Api.Infrastructure.Fields
{
    public class OptionTranslation : EntityTranslation
    {
        [Required]
        public Guid OptionId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
