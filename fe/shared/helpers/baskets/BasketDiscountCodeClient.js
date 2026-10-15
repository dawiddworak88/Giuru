import AuthenticationHelper from "../globals/AuthenticationHelper";
import BasketResponseHelper from "./BasketResponseHelper";

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

            return await BasketResponseHelper.read(response, generalErrorMessage);
        }
        catch {
            return { isSaved: false, notice: null, jsonResponse: null, message: generalErrorMessage };
        }
    }
}
