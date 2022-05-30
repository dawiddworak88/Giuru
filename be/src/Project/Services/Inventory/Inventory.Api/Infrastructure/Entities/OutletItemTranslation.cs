using Foundation.GenericRepository.Entities;
using System;

namespace Inventory.Api.Infrastructure.Entities
{
    public class OutletItemTranslation : EntityTranslation
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid OutletItemId { get; set; }
    }
}
