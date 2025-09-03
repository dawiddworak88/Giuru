class QuantityCalculatorService {

    static calculateOrderItem(
        product, 
        requestedQuantity, 
        isOutletOrder,
        existingItems = []
    ) {
        let quantity = 0;
        let stockQuantity = 0;
        let outletQuantity = 0;

        if (isOutletOrder) {
            outletQuantity = requestedQuantity;
        } 
        else if (product.stockQuantity > 0) {
            const currentStockInBasket = existingItems
                .filter((item) => item.sku === product.sku)
                .reduce((sum, item) => sum + (item.stockQuantity || 0), 0);

             const currentAvailableStockQuantity = Math.max(0, product.stockQuantity - currentStockInBasket);

            if (requestedQuantity > currentAvailableStockQuantity) {
                stockQuantity = currentAvailableStockQuantity;
                quantity = requestedQuantity - currentAvailableStockQuantity;
            } 
            else {
                stockQuantity = requestedQuantity;
            }
        } 
        else {
            quantity = requestedQuantity;
        }

        return { 
            quantity, 
            stockQuantity, 
            outletQuantity 
        };
    }

    static validateQuantity(
        requestedQuantity,
        maxAllowedOrderQuantity = null, 
        maxAllowedOrderQuantityErrorMessage,
        minOrderQuantityErrorMessage
    ) {
        if (requestedQuantity < 1) {
            return { 
                isValid: false, 
                errorMessage: minOrderQuantityErrorMessage 
            };
        }

        if (maxAllowedOrderQuantity && requestedQuantity > maxAllowedOrderQuantity) {
            return {
                isValid: false,
                errorMessage: maxAllowedOrderQuantityErrorMessage,
                maxAllowed: maxAllowedOrderQuantity
            };
        }

        return { 
            isValid: true 
        };
    }

    static calculateMaxQuantity(
        orderItems, 
        quantityType, 
        availableQuantity, 
        sku
    ) {
        if (!orderItems || orderItems.length === 0 || !sku) {
            return availableQuantity;
        }

        const actualQuantity = orderItems
            .filter(item => item.sku === sku)
            .reduce((sum, item) => sum + (item[quantityType] || 0), 0);

        return Math.max(availableQuantity - actualQuantity, 0);
    }

}

export default QuantityCalculatorService;