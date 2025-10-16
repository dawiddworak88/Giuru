import React, { useContext, useState, Fragment } from "react";
import { toast } from "react-toastify";
import PropTypes from "prop-types";
import { Context } from "../../../../shared/stores/Store";
import {
    FormControl, InputLabel, Select, MenuItem, Button,
    Table, TableBody, TableCell, TableContainer, TextField,
    TableHead, TableRow, Paper, CircularProgress, Fab
} from "@mui/material";
import { Edit } from "@mui/icons-material";
import AuthenticationHelper from "../../../../shared/helpers/globals/AuthenticationHelper";
import Files from "../../../../shared/components/Files/Files";

function EditOrderForm(props) {
    const [state, dispatch] = useContext(Context);
    const [orderStatusId, setOrderStatusId] = useState(props.orderStatusId);

    const handleOrderStatusSubmit = (e) => {
        e.preventDefault();

        dispatch({ type: "SET_IS_LOADING", payload: true });

        var orderStatus = {
            orderId: props.id,
            orderStatusId
        };

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" },
            body: JSON.stringify(orderStatus)
        };

        fetch(props.updateOrderStatusUrl, requestOptions)
            .then((response) => {
                dispatch({ type: "SET_IS_LOADING", payload: false });

                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {
                    if (response.ok) {
                        setOrderStatusId(jsonResponse.orderStatusId);
                        toast.success(jsonResponse.message);
                    }
                    else {
                        toast.error(jsonResponse?.message || props.generalErrorMessage);
                    }
                });
            }).catch(() => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(props.generalErrorMessage);
            });
    };

    const GetTotalQuantities = (item) => {
        return item.quantity + item.stockQuantity + item.outletQuantity;
    }

    return (
        <section className="section section-small-padding edit-order">
            <h1 className="subtitle is-4">{props.title}</h1>
            <h2 className="subtitle is-5 mb-2">{props.orderStatusLabel}</h2>
            <div className="columns is-desktop">
                <div className="column is-half">
                    <form className="is-modern-form" onSubmit={handleOrderStatusSubmit} method="post">
                        <div className="columns is-desktop">
                            <div className="column">
                                {props.id &&
                                    <div className="field">
                                        <InputLabel id="id-label">{props.idLabel} {props.id}</InputLabel>
                                    </div>
                                }
                                <div className="columns is-desktop">
                                    <div className="column is-half">
                                        <div className="field">
                                            <FormControl fullWidth={true} variant="standard">
                                                <InputLabel id="order-status-label">{props.orderStatusLabel}</InputLabel>
                                                <Select
                                                    labelId="order-status-label"
                                                    id="orderStatus"
                                                    name="orderStatus"
                                                    value={orderStatusId}
                                                    onChange={(e) => {
                                                        e.preventDefault();
                                                        setOrderStatusId(e.target.value);
                                                    }}>
                                                    {props.orderStatuses.map((status, index) => {
                                                        return (
                                                            <MenuItem key={index} value={status.id}>{status.name}</MenuItem>
                                                        );
                                                    })}
                                                </Select>
                                            </FormControl>
                                        </div>
                                    </div>
                                    <div className="column is-half">
                                        <div className="column">
                                            <div className="field">
                                                <Button type="submit" variant="contained" color="primary" disabled={state.isLoading}>
                                                    {props.saveText}
                                                </Button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div className="column">
                                    {props.deliveryAddress &&
                                        <div className="field">
                                            <InputLabel id="delivery-address-label">{props.deliveryAddressLabel}: {props.deliveryAddress}</InputLabel>
                                        </div>
                                    }
                                    {props.billingAddress &&
                                        <div className="field">
                                            <InputLabel id="billing-address-label">{props.billingAddressLabel}: {props.billingAddress}</InputLabel>
                                        </div>
                                    }
                                </div>
                            </div>
                        </div>
                    </form>
                    <div className="mt-5">
                        <h2 className="subtitle is-5 mb-2">{props.clientLabel}</h2>
                        <div>
                            <a href={props.clientUrl}>{props.clientName}</a>
                        </div>
                    </div>
                </div>
            </div>
            {props.orderItems && props.orderItems.length > 0 &&
                <div className="mt-5">
                    <h2 className="subtitle is-5 mb-2">{props.orderItemsLabel}</h2>
                    <div className="edit-order__items">
                        <div className="orderitems__table">
                            <TableContainer component={Paper}>
                                <Table aria-label={props.orderItemsLabel}>
                                    <TableHead>
                                        <TableRow>
                                            <TableCell></TableCell>
                                            <TableCell></TableCell>
                                            <TableCell>{props.skuLabel}</TableCell>
                                            <TableCell>{props.nameLabel}</TableCell>
                                            <TableCell>{props.quantityLabel}</TableCell>
                                            <TableCell>{props.stockQuantityLabel}</TableCell>
                                            <TableCell>{props.outletQuantityLabel}</TableCell>
                                            <TableCell className="has-text-weight-bold">{props.inTotalLabel}</TableCell>
                                            <TableCell>{props.orderStatusLabel}</TableCell>
                                            <TableCell>{props.expectedDateOfProductOnStockLabel}</TableCell>
                                            <TableCell>{props.externalReferenceLabel}</TableCell>
                                            <TableCell>{props.moreInfoLabel}</TableCell>
                                            <TableCell>{props.unitPriceLabel}</TableCell>
                                            <TableCell>{props.priceLabel}</TableCell>
                                            <TableCell>{props.currencyLabel}</TableCell>
                                        </TableRow>
                                    </TableHead>
                                    <TableBody>
                                        {props.orderItems.map((item, index) => {
                                            return (
                                                <TableRow key={index}>
                                                    <TableCell>
                                                        <Fab href={props.editUrl + "/" + item.id} size="small" color="secondary" aria-label={props.editLabel}>
                                                            <Edit />
                                                        </Fab>
                                                    </TableCell>
                                                    <TableCell><a href={item.productUrl} target="_blank"><img className="edit-order__item-product-image" src={item.imageSrc} alt={item.imageAlt} /></a></TableCell>
                                                    <TableCell>{item.sku}</TableCell>
                                                    <TableCell>{item.name}</TableCell>
                                                    <TableCell>{item.quantity}</TableCell>
                                                    <TableCell>{item.stockQuantity}</TableCell>
                                                    <TableCell>{item.outletQuantity}</TableCell>
                                                    <TableCell className="has-text-weight-bold">{GetTotalQuantities(item)}</TableCell>
                                                    <TableCell>{item.orderItemStatusName}</TableCell>
                                                    <TableCell>{item.expectedDateOfProductOnStock}</TableCell>
                                                    <TableCell>{item.externalReference}</TableCell>
                                                    <TableCell>{item.moreInfo}</TableCell>
                                                    <TableCell>{item.unitPrice}</TableCell>
                                                    <TableCell>{item.price}</TableCell>
                                                    <TableCell>{item.currency}</TableCell>
                                                </TableRow>
                                            )
                                        })}
                                    </TableBody>
                                </Table>
                            </TableContainer>
                        </div>
                    </div>
                </div>
            }
            {props.customOrder &&
                <Fragment>
                    <div className="mt-5">
                        <h2 className="subtitle is-5 mb-2">{props.customOrderLabel}</h2>
                        <div className="edit-order__custom-order">
                            <TextField
                                id="customOrder"
                                name="customOrder"
                                variant="standard"
                                value={props.customOrder}
                                fullWidth={true}
                                multiline={true}
                                disabled={true}
                                InputProps={{
                                    disableUnderline: true,
                                    className: "p-2"
                                }}
                            />
                        </div>
                    </div>
                    {props.attachments &&
                        <Files {...props.attachments} />
                    }
                </Fragment>
            }
            <div className="field mt-3">
                <a href={props.ordersUrl} className="button is-text">{props.navigateToOrders}</a>
            </div>
            {state.isLoading && <CircularProgress className="progressBar" />}
        </section >
    );
}

EditOrderForm.propTypes = {
    title: PropTypes.string.isRequired,
    id: PropTypes.string,
    skuLabel: PropTypes.string.isRequired,
    nameLabel: PropTypes.string.isRequired,
    quantityLabel: PropTypes.string.isRequired,
    externalReferenceLabel: PropTypes.string.isRequired,
    moreInfoLabel: PropTypes.string.isRequired,
    orderItemsLabel: PropTypes.string.isRequired,
    generalErrorMessage: PropTypes.string.isRequired,
    saveText: PropTypes.string.isRequired,
    orderStatusLabel: PropTypes.string.isRequired,
    orderStatuses: PropTypes.array.isRequired,
    orderStatusId: PropTypes.string.isRequired,
    clientLabel: PropTypes.string.isRequired,
    clientName: PropTypes.string.isRequired,
    clientUrl: PropTypes.string.isRequired,
    updateOrderStatusUrl: PropTypes.string.isRequired,
    idLabel: PropTypes.string,
    expectedDateOfProductOnStockLabel: PropTypes.string.isRequired,
    ordersUrl: PropTypes.string.isRequired,
    navigateToOrders: PropTypes.string.isRequired
};

export default EditOrderForm;
