export default class QuantityCalculationHelper {
    static calculateMaxQuantity = (orderItems, quantityType, availableQuantity, sku) => {
        if (orderItems && orderItems.length > 0) {
            const actualQuantity = QuantityCalculationHelper.getCurrentQuantity(orderItems, quantityType, sku);

            return Math.max(availableQuantity - actualQuantity, 0);
        }
        
        return availableQuantity;
    };

    static getCurrentQuantity = (orderItems, quantityType, sku) => {
        if (!orderItems || orderItems.length === 0) {
            return 0;
        }

        if (!sku) {
            return 0;
        }

        const orderItem = orderItems.filter(x => x.sku === sku);

        if (orderItem.length > 0) {
            return orderItem.reduce((sum, item) => sum + (item[quantityType] || 0), 0);
        }
        
        return 0;
    }
}