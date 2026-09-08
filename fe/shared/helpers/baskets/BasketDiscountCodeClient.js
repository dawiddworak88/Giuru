import AuthenticationHelper from "../globals/AuthenticationHelper";
import ResponseMessageHelper from "../responses/ResponseMessageHelper";

export default class BasketDiscountCodeClient {
    static save = async ({ updateBasketUrl, basket, generalErrorMessage }) => {
        try {
            const response = await fetch(updateBasketUrl, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "X-Requested-With": "XMLHttpRequest"
                },
                body: JSON.stringify(basket)
            });

            AuthenticationHelper.HandleResponse(response);
            const { jsonResponse, message } = await ResponseMessageHelper.read(response, generalErrorMessage);

            return { ok: response.ok && !!jsonResponse, jsonResponse, message };
        }
        catch {
            return { ok: false, jsonResponse: null, message: generalErrorMessage };
        }
    }
}
