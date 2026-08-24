import AuthenticationHelper from "../globals/AuthenticationHelper";
import QueryStringSerializer from "../serializers/QueryStringSerializer";

export default class ProductPricesHelper {
    // requiresClientId mirrors the two IPriceClientResolver kinds on the server: the buyer app
    // prices for the authenticated principal and sends no client id, while the seller app prices
    // on behalf of a client it picks and cannot produce a price without one. Asking anyway just
    // spends a round trip on a response that is known in advance to be empty.
    static getPriceByProductSku = async (controllerUrl, { sku, clientId = null, discountCode = null, isOutlet = false, requiresClientId = false } = {}) => {
        if (!controllerUrl || !sku || (requiresClientId && !clientId)) {
            return null;
        }

        const queryParameters = { sku, isOutlet };

        if (clientId) {
            queryParameters.clientId = clientId;
        }

        if (discountCode) {
            queryParameters.discountCode = discountCode;
        }

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
