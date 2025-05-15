export const addGoogleAnalyticsEventToDataLayer = (
    event,
    items
) => {
    if (typeof window !== 'undefined' && window.dataLayer) {
        const dataLayer = window.dataLayer;

        dataLayer.push({ ecommerce: null }); // Clear the previous ecommerce object
        dataLayer.push({
            event: event,
            ecommerce: {
                currency: '',
                value: 0,
                items: items.map((item) => ({
                    item_id: item.id,
                    item_name: item.name,
                    item_brand: '',
                    item_category: '',
                    item_variant: item.sku,
                    price: item.price,
                    quantity: item.quantity,
                }))
            },
        });
    }
}