import React, { useContext, useState } from "react";
import { toast } from "react-toastify";
import PropTypes from "prop-types";
import { Context } from "../../../../shared/stores/Store";
import {
    FormControl, InputLabel, Select, MenuItem, 
    Button, TextField, CircularProgress
} from "@mui/material";
import AuthenticationHelper from "../../../../shared/helpers/globals/AuthenticationHelper";
import OrderItemStatusChanges from "../../../../shared/components/OrderItemStatusChanges/OrderItemStatusChanges";

const OrderItemForm = (props) => {
    const [state, dispatch] = useContext(Context);
    const [orderItemStatusId, setOrderItemStatusId] = useState(props.orderItemStatusId ? props.orderItemStatusId : "");
    const [expectedDateOfProductOnStock, setExpectedDateOfProductOnStock] = useState(props.expectedDateOfProductOnStock ? props.expectedDateOfProductOnStock : "");
    const [orderItemStatusChanges, setOrderItemStatusChanges] = useState(props.statusChanges ? props.statusChanges : []);

    const handleSubmitForm = (e) => {
        e.preventDefault();

        dispatch({ type: "SET_IS_LOADING", payload: true });

        const requestPayload = {
            id: props.id,
            orderItemStatusId,
            expectedDateOfProductOnStock
        }

        const requestOptions = {
            method: "POST",
            headers: { 
                "Content-Type": "application/json", 
                "X-Requested-With": "XMLHttpRequest" 
            },
            body: JSON.stringify(requestPayload)
        }

        return fetch(props.saveUrl, requestOptions)
            .then((response) => {
                dispatch({ type: "SET_IS_LOADING", payload: false });

                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {
                    if (response.ok) {
                        toast.success(jsonResponse.message);
                        setOrderItemStatusChangeComment(null);
                        setOrderItemStatusChanges(jsonResponse.statusChanges);
                    }
                    else {
                        toast.error(jsonResponse?.message || props.generalErrorMessage);
                    }
                });
            }).catch(() => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(props.generalErrorMessage);
            });
    }

    const handleChangeOrderItemStatus = (e) => {
        setOrderItemStatusId(e.target.value)
    }

    return (
        <section className="section section-small-padding order-item">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div className="columns is-desktop">
                <div className="column is-half">
                    <form className="is-modern-form" onSubmit={handleSubmitForm}>
                        {props.id &&
                            <div className="field">
                                <InputLabel id="id-label">{props.idLabel} {props.id}</InputLabel>
                            </div>
                        }
                        <div className="mt-2 mb-3 order-item__image">
                            <img src={props.imageUrl} alt={props.imageAlt}/>
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
                        {props.unitPrice &&
                            <div className="field">
                                <TextField 
                                    id="unitPrice" 
                                    name="unitPrice" 
                                    label={props.unitPriceLabel} 
                                    fullWidth={true}
                                    value={props.unitPrice}
                                    variant="standard"
                                    InputProps={{
                                        readOnly: true,
                                    }}
                                />
                            </div>
                        }
                        {props.price &&
                            <div className="field">
                                <TextField 
                                    id="price" 
                                    name="price" 
                                    label={props.priceLabel} 
                                    fullWidth={true}
                                    value={props.price}
                                    variant="standard"
                                    InputProps={{
                                        readOnly: true,
                                    }}
                                />
                            </div>
                        }
                        {orderItemStatusId &&
                            <div className="field">
                                <FormControl variant="standard" fullWidth={true}>
                                    <InputLabel id="orderItemStatus-label">{props.orderStatusLabel}</InputLabel>
                                    <Select
                                        id="orderItemStatus"
                                        name="orderItemStatus"
                                        value={orderItemStatusId}
                                        onChange={(e) => handleChangeOrderItemStatus(e)}>
                                        {props.orderItemStatuses.map((status, index) => {
                                            return (
                                                <MenuItem key={index} value={status.id}>{status.name}</MenuItem>
                                            );
                                        })}
                                    </Select>
                                </FormControl>
                            </div>
                        }
                        <div className="field">
                            <TextField
                                id="expectedDateOfProductOnStock"
                                name="expectedDateOfProductOnStock"
                                label={props.expectedDateOfProductOnStockLabel}
                                variant="standard"
                                fullWidth={true}
                                multiline={true}
                                disabled={props.orderItemStatusId !== '287ee71a-d87f-4563-833a-8e2771d1e5a5'}
                                value={expectedDateOfProductOnStock}
                                onChange={(e) => setExpectedDateOfProductOnStock(e.target.value)}
                            />
                        </div>
                        <div className="field">
                            <Button 
                                type="submit" 
                                variant="contained" 
                                color="primary" 
                                disabled={state.isLoading || props.orderItemStatusId === orderItemStatusId}>
                                {props.saveText}
                            </Button>
                            <a href={props.orderUrl} className="ml-2 button is-text">{props.navigateToOrderLabel}</a>
                        </div>
                    </form>
                </div>
            </div>
            {props.orderItemStatusChanges &&
                <OrderItemStatusChanges
                    statusChanges={orderItemStatusChanges}
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
    outletQuantity: PropTypes.number,
    stockQuantity: PropTypes.number,
    quantity: PropTypes.number,
    expectedDateOfProductOnStockLabel: PropTypes.string.isRequired,
    orderUrl: PropTypes.string.isRequired,
    navigateToOrderLabel: PropTypes.string.isRequired,
    orderItemStatusChanges: PropTypes.object,
    externalReferenceLabel: PropTypes.string.isRequired,
    moreInfoLabel: PropTypes.string.isRequired,
    statusChanges: PropTypes.array,
    cancelOrderItemStatusUrl: PropTypes.string.isRequired
};

export default OrderItemForm;