import {
    useCallback,
    useState
} from 'react';
import QuantityCalculatorService from '../services/QuantityCalculatorService';
import { toast } from 'react-toastify';
import ProductPricesHelper from '../helpers/prices/ProductPricesHelper';
import { Context } from "../../shared/stores/Store";

export const useOrderManagement = (
    initialBasketId,
    maxAllowedOrderQuantity,
    maxAllowedOrderQuantityErrorMessage,
    minOrderQuantityErrorMessage,
    updateBasketUrl, 
    getPriceUrl
) => {
    const [state, dispatch] = useContext(Context);
    const [basketId, setBasketId] = useState(initialBasketId);
    const [orderItems, setOrderItems] = useState([]);

    const addOrderItemToBasket = useCallback(
        async ({
            product,
            quantity,
            isOutletOrder = false,
            externalReference,
            moreInfo
        }) => {
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
                unitPrice: product.price ?? null,
                price: product.price
                    ? parseFloat(product.price * quantity).toFixed(2)
                    : null,
                currency: product.currency,
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

            const basket = { 
                id: basketId, 
                items: [...orderItems, orderItem] 
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

                const jsonResponse = await response.json();

                if (response.ok) {
                    setBasketId(jsonResponse.id);

                    if (jsonResponse.items?.length > 0) {
                        setOrderItems(jsonResponse.items);
                        resetForm?.();
                    } else {
                        setOrderItems([]);
                    }
                } else {
                    toast.error(generalErrorMessage);
                }
            } catch (err) {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(generalErrorMessage);
            }
        }
  );


    return { 
        basketId, 
        orderItems, 
        addOrderItemToBasket, 
        setOrderItems 
    };
}