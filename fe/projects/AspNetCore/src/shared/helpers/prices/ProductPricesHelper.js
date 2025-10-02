import AuthenticationHelper from "../globals/AuthenticationHelper";
import QueryStringSerializer from "../serializers/QueryStringSerializer";

export default class ProductPricesHelper {
    static getPriceByProductSku = async (controllerUrl, sku) => {
        if (!controllerUrl || !sku) {
            return null;
        }

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

        let jsonResponse = null;

        try {
            jsonResponse = await response.json();
        } catch (e) {
            return null;
        }

        if (response.ok && jsonResponse) {
            const price = jsonResponse.currentPrice;
            const currency = jsonResponse.currencyCode;

            return { price, currency };
        }

        return null;
    }
}