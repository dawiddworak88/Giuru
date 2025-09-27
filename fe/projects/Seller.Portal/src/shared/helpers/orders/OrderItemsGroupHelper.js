export default class OrderItemsGrouper {
    static groupOrderItems(items) {
        const grouped = [];

        items.forEach(item => {
            if (item.outletQuantity > 0) {

                const found = grouped.find(g => 
                    g.sku === item.sku && 
                    g.outletQuantity > 0 &&
                    g.moreInfo === item.moreInfo &&
                    g.externalReference === item.externalReference);

                if (found) {
                    found.outletQuantity += item.outletQuantity;
                    found.price = found.price ? (parseFloat(found.price) + parseFloat(item.price)).toFixed(2) : null;
                } else {
                    grouped.push({
                        ...item,
                        quantity: 0,
                        stockQuantity: 0,
                    });
                }
            } else {
                const found = grouped.find(g => 
                    g.sku === item.sku && 
                    (!g.outletQuantity || g.outletQuantity === 0) &&
                    g.moreInfo === item.moreInfo &&
                    g.externalReference === item.externalReference);

                if (found) {
                    found.quantity += item.quantity;
                    found.stockQuantity += item.stockQuantity;
                    found.price = found.price ? (parseFloat(found.price) + parseFloat(item.price)).toFixed(2) : null;
                } else {
                    grouped.push({
                        ...item,
                        outletQuantity: 0
                    });
                }
            }
        });
        
        return grouped;
    }
}
