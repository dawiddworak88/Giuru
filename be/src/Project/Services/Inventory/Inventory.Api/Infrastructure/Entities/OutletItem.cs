using System.Collections.Generic;

namespace Inventory.Api.Infrastructure.Entities
{
    public class OutletItem : InventoryProduct
    {
        public virtual IEnumerable<OutletItemTranslations> Translations { get; set; }
    }
}
