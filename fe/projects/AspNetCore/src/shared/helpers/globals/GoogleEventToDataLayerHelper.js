const generateItemId = (item, language) => {
    let name = "B2B_";

    if (language) {
        name += `${language}_`;
    }

    if (item.primaryProductId) {
        name += `${item.primaryProductId}_`;
    }

    if (item.variantId) {
        name += `${item.variantId}`;
    }

    return name;
}
    

export const addGoogleAnalyticsEventToDataLayer = (
    language,
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
                    item_id: generateItemId(item, language),
                    item_name: item.productName,
                    item_brand: '',
                    item_category: '',
                    item_variant: item.productName,
                    price: item.price,
                    quantity: item.quantity,
                }))
            },
        });
    }
}