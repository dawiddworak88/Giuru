import React, { Fragment, useContext, useState } from "react";
import PropTypes from "prop-types";
import { Context } from "../../../../../../shared/stores/Store";
import { toast } from "react-toastify";
import {
    Table, TableBody, TableCell, TableContainer,
    TableHead, TableRow, Paper, TextField, Fab, Button
} from "@mui/material";
import { Edit } from "@mui/icons-material";
import moment from "moment";
import AuthenticationHelper from "../../../../../../shared/helpers/globals/AuthenticationHelper";
import Files from "../../../../../../shared/components/Files/Files";

function StatusOrder(props) {
    const [state, dispatch] = useContext(Context);
    const [canceledOrder, setCanceledOrder] = useState(false);
    const [orderStatusId, setOrderStatusId] = useState(props.orderStatusId ? props.orderStatusId : null);

    const handleCancelOrderSubmit = (e) => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        const requestBody = {
            orderId: props.id,
            orderStatusId: orderStatusId
        };

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" },
            body: JSON.stringify(requestBody)
        };

        fetch(props.updateOrderStatusUrl, requestOptions)
            .then(function (response) {

                dispatch({ type: "SET_IS_LOADING", payload: false });

                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {
                    if (response.ok) {
                        setCanceledOrder(true);
                        setOrderStatusId(jsonResponse.orderStatusId);
                        toast.success(jsonResponse.message);
                    }
                    else {
                        toast.error(props.generalErrorMessage);
                    }
                });
            }).catch(() => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(props.generalErrorMessage);
            });
    };

    const status = props.orderStatuses.find((item) => item.id === orderStatusId);
    
    return (
        <section className="section status-order">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div className="columns is-desktop">
                <div className="column is-3">
                    {status &&
                        <div className="column is-3">
                            <div className="status-ordder__details">{props.orderStatusLabel} {status.name}</div>
                            {props.expectedDelivery && 
                                <div className="status-ordder__details">{props.expectedDeliveryLabel}: {moment(props.expectedDelivery).format("L")}</div>
                            }
                        </div>
                    }
                    {props.canCancelOrder && !canceledOrder &&
                        <div className="mt-2">
                            <Button 
                                type="text" 
                                variant="contained" 
                                color="primary"
                                onClick={handleCancelOrderSubmit}
                            >
                                {props.cancelOrderLabel}
                            </Button>
                        </div>
                    }
                </div>
            </div>
            {props.orderItems && props.orderItems.length > 0 &&
                <div className="mt-5">
                    <h2 className="subtitle is-5 mb-2">{props.orderItemsLabel}</h2>
                    <div className="status-order__items">
                        <div className="orderitems__table">
                            <TableContainer component={Paper}>
                                <Table aria-label={props.orderItemsLabel}>
                                    <TableHead>
                                        <TableRow>
                                            <TableCell></TableCell>
                                            <TableCell></TableCell>
                                            <TableCell>{props.skuLabel}</TableCell>
                                            <TableCell>{props.nameLabel}</TableCell>
                                            <TableCell></TableCell>
                                            <TableCell>{props.quantityLabel}</TableCell>
                                            <TableCell>{props.stockQuantityLabel}</TableCell>
                                            <TableCell>{props.outletQuantityLabel}</TableCell>
                                            <TableCell>{props.orderStatusLabel}</TableCell>
                                            <TableCell>{props.orderStatusCommentLabel}</TableCell>
                                            <TableCell>{props.externalReferenceLabel}</TableCell>
                                            <TableCell>{props.deliveryFromLabel}</TableCell>
                                            <TableCell>{props.deliveryToLabel}</TableCell>
                                            <TableCell>{props.moreInfoLabel}</TableCell>
                                        </TableRow>
                                    </TableHead>
                                    <TableBody>
                                        {props.orderItems && props.orderItems.map((item, index) => {
                                            return (
                                                <TableRow key={index}>
                                                    <TableCell>
                                                        <Fab href={props.editUrl + "/" + item.id} size="small" color="secondary" aria-label={props.editLabel}>
                                                            <Edit />
                                                        </Fab>
                                                    </TableCell>
                                                    <TableCell><a href={item.productUrl}><img className="status-order__item-product-image" src={item.imageSrc} alt={item.imageAlt} /></a></TableCell>
                                                    <TableCell>{item.sku}</TableCell>
                                                    <TableCell>{item.name}</TableCell>
                                                    <TableCell>{item.productAttributes}</TableCell>
                                                    <TableCell>{item.quantity}</TableCell>
                                                    <TableCell>{item.stockQuantity}</TableCell>
                                                    <TableCell>{item.outletQuantity}</TableCell>
                                                    <TableCell>{item.orderItemStatusName}</TableCell>
                                                    <TableCell>{item.orderItemStatusChangeComment}</TableCell>
                                                    <TableCell>{item.externalReference}</TableCell>
                                                    <TableCell>{item.deliveryFrom && <span>{moment(item.deliveryFrom).format("L")}</span>}</TableCell>
                                                    <TableCell>{item.deliveryTo && <span>{moment(item.deliveryTo).format("L")}</span>}</TableCell>
                                                    <TableCell>{item.moreInfo}</TableCell>
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
                        <div className="status-order__custom-order">
                            <TextField 
                                id="customOrder"
                                name="customOrder"
                                value={props.customOrder}
                                fullWidth={true}
                                multiline={true}
                                disabled={true}
                                variant="standard"
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
        </section >
    );
}

StatusOrder.propTypes = {
    title: PropTypes.string.isRequired,
    id: PropTypes.string,
    skuLabel: PropTypes.string.isRequired,
    nameLabel: PropTypes.string.isRequired,
    quantityLabel: PropTypes.string.isRequired,
    externalReferenceLabel: PropTypes.string.isRequired,
    deliveryFromLabel: PropTypes.string.isRequired,
    deliveryToLabel: PropTypes.string.isRequired,
    moreInfoLabel: PropTypes.string.isRequired,
    orderItemsLabel: PropTypes.string.isRequired,
    orderStatusLabel: PropTypes.string.isRequired,
    orderStatusId: PropTypes.string.isRequired,
    customOrderLabel: PropTypes.string.isRequired
};

export default StatusOrder;
