import { useCallback } from "react";
import { toast } from "react-toastify";
import BasketDiscountCodeClient from "../helpers/baskets/BasketDiscountCodeClient";

// The discount code state itself stays with the caller: useOrderManagement takes the applied
// code and the sync callback as arguments, so useDiscountCode has to run before it, while
// everything here needs the basket id and the item setters that useOrderManagement returns.
// Splitting the two lets each side keep the order it needs - the caller wires onSave to the
// save returned from here, which is only ever invoked from an event handler.
export const useBasketDiscountCode = ({
    isEnabled,
    updateBasketUrl,
    generalErrorMessage,
    discountCodeAppliedMessage,
    dispatch,
    buildBasket,
    setBasketId,
    setGroupedOrderItems,
    syncFromResponse
}) => {

    // Every basket response - an apply, a removal, an added or deleted line, a file upload -
    // carries the same three things back, so the handlers share this instead of repeating it.
    const applyBasketResponse = useCallback((jsonResponse) => {
        setBasketId(jsonResponse.id);
        setGroupedOrderItems(jsonResponse.items || []);
        syncFromResponse(jsonResponse);
    }, [setBasketId, setGroupedOrderItems, syncFromResponse]);

    // Deliberately not memoised: buildBasket closes over the current basket id and items and is
    // rebuilt every render, so a useCallback would have to list it and would never hold.
    const save = async (newDiscountCode, showSuccessMessage) => {
        if (!isEnabled) return;

        dispatch({ type: "SET_IS_LOADING", payload: true });

        // BasketDiscountCodeClient.save resolves rather than throws, but the loading flag is
        // cleared in a finally regardless: anything that escapes here - building the basket, or
        // applying the response - would otherwise leave the form spinning and block the order.
        try {
            const { ok, jsonResponse, message } = await BasketDiscountCodeClient.save({
                updateBasketUrl,
                basket: buildBasket(newDiscountCode),
                generalErrorMessage
            });

            if (ok) {
                applyBasketResponse(jsonResponse);

                if (showSuccessMessage) {
                    toast.success(discountCodeAppliedMessage);
                }
            }
            else {
                toast.error(message);
            }
        }
        catch {
            toast.error(generalErrorMessage);
        }
        finally {
            dispatch({ type: "SET_IS_LOADING", payload: false });
        }
    };

    return { save, applyBasketResponse };
};
