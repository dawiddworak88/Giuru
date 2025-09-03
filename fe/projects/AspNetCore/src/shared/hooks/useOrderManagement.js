import {
    useContext,
    useCallback,
    useState
} from 'react';
import QuantityCalculatorService from '../services/QuantityCalculatorService';
import { toast } from 'react-toastify';
import ProductPricesHelper from '../helpers/prices/ProductPricesHelper';
import { Context } from "../../shared/stores/Store";
import AuthenticationHelper from "../helpers/globals/AuthenticationHelper";
import QueryStringSerializer from '../helpers/serializers/QueryStringSerializer';

export const useOrderManagement = ({
    initialBasketId,
    initialOrderItems,
    maxAllowedOrderQuantity,
    maxAllowedOrderQuantityErrorMessage,
    minOrderQuantityErrorMessage,
    generalErrorMessage,
    updateBasketUrl,
    clearBasketUrl,
    getPriceUrl
}) => {
    const [state, dispatch] = useContext(Context);
    const [basketId, setBasketId] = useState(initialBasketId);
    const [orderItems, setOrderItems] = useState(initialOrderItems || []);

    const groupOrderItems = (items) => {
        const grouped = [];

        items.forEach(item => {
            if (item.outletQuantity > 0) {
                const found = grouped.find(g => 
                    g.sku === item.sku && 
                    g.outletQuantity > 0 &&
                    g.moreInfo === item.moreInfo &&
                    g.externalReference === item.externalReference
                );

                if (found) {
                    found.outletQuantity += item.outletQuantity;
                    found.price = found.price ? (parseFloat(found.price) + parseFloat(item.price)).toFixed(2) : null;
                } else {
                    grouped.push({
                        ...item,
                        quantity: 0,
                        stockQuantity: 0,
                    });
                }
            } else {
                const found = grouped.find(g => 
                    g.sku === item.sku && 
                    (!g.outletQuantity || g.outletQuantity === 0) &&
                    g.moreInfo === item.moreInfo &&
                    g.externalReference === item.externalReference
                );

                if (found) {
                    found.quantity += item.quantity;
                    found.stockQuantity += item.stockQuantity;
                    found.price = found.price ? (parseFloat(found.price) + parseFloat(item.price)).toFixed(2) : null;
                } else {
                    grouped.push({
                        ...item,
                        outletQuantity: 0
                    });
                }
            }
        });
        
        return grouped;
    }

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
                currency: product.currency
            }


            if (isOutletOrder) {
                const outletPrice = await ProductPricesHelper.getPriceByProductSku(
                    getPriceUrl, 
                    product.sku
                );

                if (outletPrice) {
                    orderItem.unitPrice = outletPrice.price;
                    orderItem.price = parseFloat(outletPrice.price * quantity).toFixed(2);
                    orderItem.currency = outletPrice.currency;
                }
            }

            const newItems = groupOrderItems([...orderItems, orderItem]);

            const basket = { 
                id: basketId, 
                items: newItems 
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

                dispatch({ type: "SET_IS_LOADING", payload: false });
                dispatch({
                    type: "SET_TOTAL_BASKET",
                    payload: parseInt(quantity + state.totalBasketItems),
                });

                AuthenticationHelper.HandleResponse(response);

                if (response.ok) {
                    const jsonResponse = await response.json();

                    setBasketId(jsonResponse.id);

                    if (jsonResponse.items?.length > 0) {
                        setOrderItems(jsonResponse.items);
                        resetData?.()
                    }
                }
            } catch {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(generalErrorMessage);
            }
        }, [basketId, orderItems]
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
    }, [basketId, orderItems]);

    const deleteOrderItemFromBasket = useCallback(
        async ({
            orderItem: item,
            resetData
        }) => {
            if (!basketId || !updateBasketUrl) return;

            dispatch({ type: "SET_IS_LOADING", payload: true });
        
            const newItems = orderItems.filter(oi => oi !== item);

            const basket = { 
                id: basketId, 
                items: newItems 
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

                const reducedQuantity = item.quantity + item.stockQuantity + item.outletQuantity;

                dispatch({ type: "SET_IS_LOADING", payload: false });
                dispatch({ type: "SET_TOTAL_BASKET", payload: state.totalBasketItems - reducedQuantity });

                AuthenticationHelper.HandleResponse(response);


                if (response.ok) {
                    const jsonResponse = await response.json();

                    if (jsonResponse.items && jsonResponse.items.length > 0) {
                        setOrderItems(groupOrderItems(jsonResponse.items));

                        resetData?.();
                    }
                }
            } catch {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(generalErrorMessage);
            }
        }, [basketId, orderItems]
    );

    return { 
        basketId, 
        orderItems, 
        addOrderItemToBasket,
        deleteOrderItemFromBasket,
        clearBasket
    };
}