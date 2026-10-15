import {
    useContext,
    useCallback,
    useState
} from 'react';
import QuantityCalculatorService from '../services/QuantityCalculatorService';
import { toast } from 'react-toastify';
import ProductPricesHelper from '../helpers/prices/ProductPricesHelper';
import OrderItemsGroupHelper from '../helpers/orders/OrderItemsGroupHelper';
import { Context } from "../../shared/stores/Store";
import AuthenticationHelper from "../helpers/globals/AuthenticationHelper";
import QueryStringSerializer from '../helpers/serializers/QueryStringSerializer';
import ToastSuccessAddProductToBasket from "../components/Toast/ToastSuccessAddProductToBasket";
import BasketResponseHelper from "../helpers/baskets/BasketResponseHelper";

// This hook only adds and removes basket lines; it never applies or clears a code. An empty string
// tells the server to keep the code stored on the basket and reprice the lines against it, and the
// server answers with the code it kept - a save drops a stored code that is no longer valid.
// Forwarding the code this page last saw instead would ask the server to apply a dropped code
// anew, and the catalog and product pages are never told of a drop. A null would be a removal.
const KeepStoredDiscountCode = "";

export const useOrderManagement = ({
    initialBasketId,
    initialOrderItems,
    maxAllowedOrderQuantity,
    maxAllowedOrderQuantityErrorMessage,
    minOrderQuantityErrorMessage,
    generalErrorMessage,
    addProductToBasketMessage,
    updateBasketUrl,
    clearBasketUrl,
    getPriceUrl,
    discountCode,
    isDiscountCodeEnabled,
    onDiscountCodeChanged
}) => {
    const [state, dispatch] = useContext(Context);
    const [basketId, setBasketId] = useState(initialBasketId || null);
    const [orderItems, setOrderItems] = useState(initialOrderItems || []);

    const addOrderItemToBasket = useCallback(
        async ({
            product,
            quantity,
            isOutletOrder = false,
            externalReference,
            moreInfo,
            resetData,
        }) => {
            if (!updateBasketUrl) return;

            const validation = QuantityCalculatorService.validateQuantity(
                quantity, 
                maxAllowedOrderQuantity,
                maxAllowedOrderQuantityErrorMessage,
                minOrderQuantityErrorMessage
            );

            if (!validation.isValid) {
                toast.error(validation.errorMessage);
                return;
            }

            const {
                quantity: regularQuantity,
                stockQuantity,
                outletQuantity
            } = QuantityCalculatorService.calculateOrderItem(
                product,
                quantity,
                isOutletOrder,
                orderItems
            )

            let orderItem = {
                productId: product.id,
                sku: product.sku,
                name: product.name,
                imageId: product.images ? product.images[0] : null,
                quantity: regularQuantity,
                stockQuantity,
                outletQuantity,
                externalReference,
                moreInfo,
                unitPrice: product.price,
                price: product.price
                    ? parseFloat(product.price * quantity).toFixed(2)
                    : null,
                currency: product.currency,
                expectedLeadTime: product.expectedLeadTime || null
            }


            if (isOutletOrder) {
                const outletPrice = await ProductPricesHelper.getPriceByProductSku(
                    getPriceUrl,
                    {
                        sku: product.sku,
                        discountCode: isDiscountCodeEnabled ? discountCode : null,
                        isOutlet: true
                    }
                );

                if (outletPrice) {
                    orderItem.unitPrice = outletPrice.price;
                    orderItem.price = parseFloat(outletPrice.price * quantity).toFixed(2);
                    orderItem.currency = outletPrice.currency;
                }
            }

            const newItems = OrderItemsGroupHelper.groupOrderItems([...orderItems, orderItem]);

            const basket = {
                id: basketId, 
                items: newItems,
                ...(isDiscountCodeEnabled && { discountCode: KeepStoredDiscountCode })
            };

            try {
                const response = await fetch(updateBasketUrl, {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                        "X-Requested-With": "XMLHttpRequest",
                    },
                    body: JSON.stringify(basket),
                });

                AuthenticationHelper.HandleResponse(response);
                const { jsonResponse, message, isSaved, notice } = await BasketResponseHelper.read(response, generalErrorMessage);

                if (isSaved) {
                    setBasketId(jsonResponse.id);
                    onDiscountCodeChanged?.(jsonResponse.discountCode || "");
                    dispatch({
                        type: "SET_TOTAL_BASKET",
                        payload: parseInt(quantity + state.totalBasketItems),
                    });

                    if (addProductToBasketMessage) ToastSuccessAddProductToBasket(addProductToBasketMessage)
                    if (notice) toast.warn(notice);

                    if (jsonResponse.items?.length > 0) {
                        setOrderItems(jsonResponse.items);
                        resetData?.()
                    }
                }
                else {
                    toast.error(message);
                }

                dispatch({ type: "SET_IS_LOADING", payload: false });
            } catch {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(generalErrorMessage);
            }
        }, [
            addProductToBasketMessage, basketId, discountCode, dispatch,
            generalErrorMessage, getPriceUrl, isDiscountCodeEnabled, maxAllowedOrderQuantity,
            maxAllowedOrderQuantityErrorMessage, minOrderQuantityErrorMessage, onDiscountCodeChanged,
            orderItems, state.totalBasketItems, updateBasketUrl
        ]
    );

    const clearBasket = useCallback(async () => {
        if (!basketId || !clearBasketUrl) return;

        dispatch({ type: "SET_IS_LOADING", payload: true });

        const requestOptions = {
            method: "DELETE",
            headers: {
                "Content-Type": "application/json",
                "X-Requested-With": "XMLHttpRequest",
            },
        };

        const requestData = { 
            id: basketId 
        };

        const url = clearBasketUrl + "?" + QueryStringSerializer.serialize(requestData);

        try {
            const response = await fetch(url, requestOptions);

            dispatch({ type: "SET_IS_LOADING", payload: false });
            dispatch({ type: "SET_TOTAL_BASKET", payload: 0 });

            AuthenticationHelper.HandleResponse(response);

            if (response.ok) {
                const jsonResponse = await response.json();

                toast.success(jsonResponse.message);
                setOrderItems([]);
                setBasketId(null);
            }
        } catch {
            dispatch({ type: "SET_IS_LOADING", payload: false });
            toast.error(generalErrorMessage);
        }
    }, [basketId, clearBasketUrl, dispatch, generalErrorMessage]);

    const deleteOrderItemFromBasket = useCallback(
        async ({
            orderItem: item,
            resetData
        }) => {
            if (!basketId || !updateBasketUrl) return;

            dispatch({ type: "SET_IS_LOADING", payload: true });
        
            const newItems = orderItems.filter(oi =>
                !(oi.productId === item.productId &&
                  oi.moreInfo === item.moreInfo &&
                  oi.externalReference === item.externalReference)
                );

            const basket = {
                id: basketId, 
                items: newItems,
                ...(isDiscountCodeEnabled && { discountCode: KeepStoredDiscountCode })
            };

            try {
                const response = await fetch(updateBasketUrl, {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                        "X-Requested-With": "XMLHttpRequest",
                    },
                    body: JSON.stringify(basket),
                });

                AuthenticationHelper.HandleResponse(response);
                const { jsonResponse, message, isSaved, notice } = await BasketResponseHelper.read(response, generalErrorMessage);

                if (isSaved) {
                    const reducedQuantity = item.quantity + item.stockQuantity + item.outletQuantity;

                    onDiscountCodeChanged?.(jsonResponse.discountCode || "");
                    dispatch({ type: "SET_TOTAL_BASKET", payload: state.totalBasketItems - reducedQuantity });

                    if (notice) toast.warn(notice);

                    if (jsonResponse.items && jsonResponse.items.length > 0) {
                        setOrderItems(OrderItemsGroupHelper.groupOrderItems(jsonResponse.items));
                    }
                    else {
                        setOrderItems([]);
                    }

                    resetData?.();
                }
                else {
                    toast.error(message);
                }

                dispatch({ type: "SET_IS_LOADING", payload: false });
            } catch {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(generalErrorMessage);
            }
        }, [
            basketId, dispatch, generalErrorMessage, isDiscountCodeEnabled,
            onDiscountCodeChanged, orderItems, state.totalBasketItems, updateBasketUrl
        ]
    );

    const setGroupedOrderItems = useCallback((items) => {
        setOrderItems(OrderItemsGroupHelper.groupOrderItems(items));
    }, []);

    return { 
        basketId, 
        orderItems, 
        setBasketId,
        setGroupedOrderItems,
        addOrderItemToBasket,
        deleteOrderItemFromBasket,
        clearBasket
    };
}