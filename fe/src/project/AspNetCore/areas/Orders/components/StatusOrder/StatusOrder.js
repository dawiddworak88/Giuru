import React, { useContext, useState } from "react";
import PropTypes from "prop-types";
import { Context } from "../../../../../../shared/stores/Store";
import { toast } from "react-toastify";
import {
    Table, TableBody, TableCell, TableContainer,
    TableHead, TableRow, Paper, Button
} from "@material-ui/core";
import moment from "moment";
import AuthenticationHelper from "../../../../../../shared/helpers/globals/AuthenticationHelper";

function StatusOrder(props) {
    const [state, dispatch] = useContext(Context);
    const [canceledOrder, setCanceledOrder] = useState(false);
    const [orderStatusId, setOrderStatusId] = useState(props.orderStatusId ? props.orderStatusId : null);

    const handleCancelOrderSubmit = (e) => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        const requestBody = {
            orderId: props.id,
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
                        <div className="status-order-container">
                            <div className="status-ordder__details">{props.orderStatusLabel}: {status.name}</div>
                            {props.expectedDelivery &&
                                <div className="status-ordder__details">{props.expectedDeliveryLabel}: {moment(props.expectedDelivery).format("L")}</div>
                            }
                        </div>
                    }
                    <div className="mt-2">
                        <Button 
                            type="submit" 
                            variant="contained" 
                            color="primary"
                            onClick={handleCancelOrderSubmit}
                            disabled={!props.canCancelOrder || canceledOrder}
                        >
                            {props.cancelOrderLabel}
                        </Button>
                    </div>
                </div>
            </div>
            <div className="mt-5">
                <h2 className="subtitle is-5 status-order__items-subtitle">{props.orderItemsLabel}</h2>
                <div className="edit-order__items">
                    <section className="section">
                        <div className="orderitems__table">
                            <TableContainer component={Paper}>
                                <Table aria-label={props.orderItemsLabel}>
                                    <TableHead>
                                        <TableRow>
                                            <TableCell></TableCell>
                                            <TableCell>{props.skuLabel}</TableCell>
                                            <TableCell>{props.nameLabel}</TableCell>
                                            <TableCell>{props.fabricsLabel}</TableCell>
                                            <TableCell>{props.quantityLabel}</TableCell>
                                            <TableCell>{props.externalReferenceLabel}</TableCell>
                                            <TableCell>{props.deliveryFromLabel}</TableCell>
                                            <TableCell>{props.deliveryToLabel}</TableCell>
                                            <TableCell>{props.moreInfoLabel}</TableCell>
                                        </TableRow>
                                    </TableHead>
                                    <TableBody>
                                        {props.orderItems && props.orderItems.map((item, index) => {
                                            let fabrics = null;
                                            if (item.fabrics.length > 0) {
                                                fabrics = item.fabrics.find(x => x.key === "primaryFabrics") ? item.fabrics.find(x => x.key === "primaryFabrics").values.join(", ") : "";

                                                let secondaryFabrics = item.fabrics.find(x => x.key === "secondaryFabrics") ? item.fabrics.find(x => x.key === "secondaryFabrics").values.join(", ") : "";
                                                if (secondaryFabrics){
                                                    fabrics += ", " + secondaryFabrics;
                                                }
                                            }
                                            return (
                                                <TableRow key={index}>
                                                    <TableCell><a href={item.productUrl}><img className="status-order__item-product-image" src={item.imageSrc} alt={item.imageAlt} /></a></TableCell>
                                                    <TableCell>{item.sku}</TableCell>
                                                    <TableCell>{item.name}</TableCell>
                                                    <TableCell>{fabrics}</TableCell>
                                                    <TableCell>{item.quantity}</TableCell>
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
                    </section>
                </div>
            </div>
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
};

export default StatusOrder;
