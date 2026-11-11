import { toast } from "react-toastify";
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

    // A code that stopped being valid is dropped by the server on save and explained here.
    static showRemovedMessage = (jsonResponse) => {
        if (jsonResponse?.discountCodeRemovedMessage) {
            toast.warning(jsonResponse.discountCodeRemovedMessage);
        }
    }
}
