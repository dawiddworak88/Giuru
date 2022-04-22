using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Api.Infrastructure.Entities
{
    public class OutletItemTranslations : Entity
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Language { get; set; }

        [Required]
        public Guid OutletItemId { get; set; }
    }
}
