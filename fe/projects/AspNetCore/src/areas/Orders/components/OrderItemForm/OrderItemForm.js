import React, { useState, useContext } from "react";
import { toast } from "react-toastify";
import { Context } from "../../../../shared/stores/Store";
import PropTypes from "prop-types";
import {
    Button, TextField, CircularProgress
} from "@mui/material";
import OrderItemStatusChanges from "../../../../shared/components/OrderItemStatusChanges/OrderItemStatusChanges";
import AuthenticationHelper from "../../../../../../../shared/helpers/globals/AuthenticationHelper";
import moment from "moment";

const OrderItemForm = (props) => {
    const [state, dispatch] = useContext(Context);
    const [canceledOrderItem, setCanceledOrderItem] = useState(false);
    const [statusChanges, setStatusChanges] = useState(props.statusChanges ? props.statusChanges : [])

    const handleCancelOrderItem = (e) => {
        e.preventDefault();

        dispatch({ type: "SET_IS_LOADING", payload: true });

        const requestPayload = {
            id: props.id
        }

        const requestOptions = {
            method: "POST",
            headers: { 
                "Content-Type": "application/json", 
                "X-Requested-With": "XMLHttpRequest" 
            },
            body: JSON.stringify(requestPayload)
        }

        return fetch(props.cancelOrderItemStatusUrl, requestOptions)
            .then((response) => {
                dispatch({ type: "SET_IS_LOADING", payload: false });

                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {
                    if (response.ok) {
                        toast.success(jsonResponse.message);
                        setCanceledOrderItem(true);
                        setStatusChanges(jsonResponse.statusChanges);
                    }
                });
            }).catch(() => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(props.generalErrorMessage);
            });
    }

    return (
        <section className="section section-small-padding order-item">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div className="columns is-desktop">
                <div className="column is-half">
                    <div className="order-item__status">{props.orderStatusLabel}: {props.orderItemStatusName}</div>
                    <div className="mt-2 mb-3 order-item__image">
                        <img src={props.imageUrl} alt={props.imageAlt} />
                    </div>
                    <div className="field">
                        <TextField 
                            id="productSku" 
                            name="productSku" 
                            label={props.skuLabel} 
                            fullWidth={true}
                            value={props.productSku}
                            variant="standard"
                            InputProps={{
                                readOnly: true,
                            }}
                        />
                    </div>
                    <div className="field">
                        <TextField 
                            id="productName" 
                            name="productName" 
                            label={props.nameLabel} 
                            fullWidth={true}
                            value={props.productName} 
                            variant="standard"
                            InputProps={{
                                readOnly: true,
                            }}
                        />
                    </div>
                    <div className="field">
                        <TextField 
                            id="quantity" 
                            name="quantity" 
                            label={props.quantityLabel} 
                            fullWidth={true}
                            value={props.quantity} 
                            type="number"
                            variant="standard"
                            InputProps={{
                                readOnly: true,
                            }}
                        />
                    </div>
                    <div className="field">
                        <TextField 
                            id="stockQuantity" 
                            name="stockQuantity" 
                            label={props.stockQuantityLabel} 
                            fullWidth={true}
                            value={props.stockQuantity} 
                            type="number"
                            variant="standard"
                            InputProps={{
                                readOnly: true,
                            }}
                        />
                    </div>
                    <div className="field">
                        <TextField 
                            id="outletQuantity" 
                            name="outletQuantity" 
                            label={props.outletQuantityLabel} 
                            fullWidth={true}
                            value={props.outletQuantity}
                            type="number"
                            variant="standard"
                            InputProps={{
                                readOnly: true,
                            }}
                        />
                    </div>
                    <div className="field">
                        <TextField 
                            id="externalReference" 
                            name="externalReference" 
                            label={props.externalReferenceLabel} 
                            fullWidth={true}
                            value={props.externalReference}
                            variant="standard"
                            InputProps={{
                                readOnly: true,
                            }}
                        />
                    </div>
                    <div className="field">
                        <TextField 
                            id="moreInfo" 
                            name="moreInfo" 
                            label={props.moreInfoLabel} 
                            fullWidth={true}
                            value={props.moreInfo}
                            variant="standard"
                            InputProps={{
                                readOnly: true,
                            }}
                        />
                    </div>
                    <div className="field">
                        <TextField 
                            id="deliveryFrom" 
                            name="deliveryFrom" 
                            label={props.deliveryFromLabel}
                            value={props.deliveryFrom ? moment(props.deliveryFrom).format("L") : ""}
                            fullWidth={true}
                            variant="standard"
                            InputProps={{
                                readOnly: true,
                            }}
                        />
                    </div>
                    <div className="field">
                        <TextField 
                            id="deliveryTo" 
                            name="deliveryTo" 
                            label={props.deliveryToLabel}
                            value={props.deliveryTo ? moment(props.deliveryTo).format("L") : ""}
                            fullWidth={true}
                            variant="standard"
                            InputProps={{
                                readOnly: true,
                            }}
                        />
                    </div>
                    <div className="field">
                        {props.canCancelOrderItem && !canceledOrderItem &&
                            <Button 
                                className="mr-2"
                                type="button" 
                                variant="contained"
                                onClick={handleCancelOrderItem}
                                color="secondary">
                                {props.cancelOrderItemLabel}
                            </Button>
                        }
                        <a href={props.orderUrl} className="button is-text">{props.navigateToOrderLabel}</a>
                    </div>
                </div>
            </div>
            {statusChanges && props.orderItemStatusChanges &&
                <OrderItemStatusChanges
                    statusChanges={statusChanges}
                    labels={props.orderItemStatusChanges}
                />
            }
            {state.isLoading && <CircularProgress className="progressBar" />}
        </section>
    );
}

OrderItemForm.propTypes = {
    title: PropTypes.string.isRequired,
    imageAlt: PropTypes.string.isRequired,
    imageUrl: PropTypes.string.isRequired,
    skuLabel: PropTypes.string.isRequired,
    nameLabel: PropTypes.string.isRequired,
    quantityLabel: PropTypes.string.isRequired,
    stockQuantityLabel: PropTypes.string.isRequired,
    orderStatusLabel: PropTypes.string.isRequired,
    orderItemStatusName: PropTypes.string,
    outletQuantity: PropTypes.number,
    stockQuantity: PropTypes.number,
    quantity: PropTypes.number,
    orderStatusCommentLabel: PropTypes.string.isRequired,
    orderUrl: PropTypes.string.isRequired,
    navigateToOrderLabel: PropTypes.string.isRequired,
    orderItemStatusChanges: PropTypes.object,
    deliveryFromLabel: PropTypes.string.isRequired,
    deliveryToLabel: PropTypes.string.isRequired,
    externalReferenceLabel: PropTypes.string.isRequired,
    moreInfoLabel: PropTypes.string.isRequired,
    statusChanges: PropTypes.array,
    cancelOrderItemLabel: PropTypes.string.isRequired,
    cancelOrderItemStatusUrl: PropTypes.string.isRequired
};

export default OrderItemForm;