import AuthenticationHelper from "../globals/AuthenticationHelper";
import QueryStringSerializer from "../serializers/QueryStringSerializer";

export default class ProductPricesHelper {
    static getPriceByProductSku = async (controllerUrl, sku, quantity) => {
        const queryParameters = { sku };

        const url = controllerUrl + "?" + QueryStringSerializer.serialize(queryParameters);

        const response = await fetch(url, {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "X-Requested-With": "XMLHttpRequest"
            }
        });

        AuthenticationHelper.HandleResponse(response);

        const jsonResponse = await response.json();

        if (response.ok) {
            const unitPrice = jsonResponse.currentPrice ? parseFloat(jsonResponse.currentPrice).toFixed(2) : null;
            const price = jsonResponse.currentPrice ? parseFloat(jsonResponse.currentPrice * quantity).toFixed(2) : null;
            const currency = jsonResponse.currency ? jsonResponse.currency : null;

            return { unitPrice, price, currency};
        }
    }
}