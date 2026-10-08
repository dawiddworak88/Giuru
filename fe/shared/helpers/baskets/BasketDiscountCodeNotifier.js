import { toast } from "react-toastify";

// A basket save that dropped a discount code which can no longer be applied says so in the response. Every path that applies
// a basket response goes through here, so the warning reads the same whichever form or hook made the save.
export default class BasketDiscountCodeNotifier {
    static notifyIfRemoved = (jsonResponse) => {
        const message = jsonResponse?.discountCodeRemovedMessage;

        if (message) {
            toast.warning(message);

            return true;
        }

        return false;
    }
}
