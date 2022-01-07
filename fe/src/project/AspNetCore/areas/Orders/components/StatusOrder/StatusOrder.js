import React, { useContext, useState, useEffect } from "react";
import PropTypes from "prop-types";
import { Context } from "../../../../../../shared/stores/Store";
import { CircularProgress } from "@material-ui/core";
import {
    Table, TableBody, TableCell, TableContainer,
    TableHead, TableRow, Paper
} from "@material-ui/core";
import moment from "moment";

function StatusOrder(props) {

    const [state,] = useContext(Context);
    const [orderStatuses, setOrderStatuses] = useState([]);
    const [orderStatus, setOrderStatus] = useState("");

    const getOrderStatuses = (e) => {
        const requestOptions = {
            method: "GET",
            headers: { "Content-Type": "application/json" }
        };

        fetch(props.orderStatusesUrl, requestOptions)
            .then((response) => {
                return response.json().then(jsonResponse => {
                    setOrderStatuses(jsonResponse);
                });
            });
    };

    useEffect(() => {
        getOrderStatuses();
    });

    const getOrderStatus = () => {
        const status = orderStatuses.find((item) => item.id === props.orderStatusId);
        if (status){
            setOrderStatus(status.name);
        };
    };

    return (
        <section className="section section-small-padding status-order">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div className="columns is-desktop">
                {getOrderStatus() &&
                    <div className="column is-3">
                        <div className="status-ordder__details">{props.orderStatusLabel}: {orderStatus}</div>
                        {props.expectedDelivery && 
                            <div className="status-ordder__details">{props.expectedDeliveryLabel}: {moment(props.expectedDelivery).format("L")}</div>
                        }
                    </div>
                }
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
                                            <TableCell>{props.quantityLabel}</TableCell>
                                            <TableCell>{props.externalReferenceLabel}</TableCell>
                                            <TableCell>{props.deliveryFromLabel}</TableCell>
                                            <TableCell>{props.deliveryToLabel}</TableCell>
                                            <TableCell>{props.moreInfoLabel}</TableCell>
                                        </TableRow>
                                    </TableHead>
                                    <TableBody>
                                        {props.orderItems && props.orderItems.map((item, index) => (
                                            <TableRow key={index}>
                                                <TableCell><a href={item.productUrl}><img className="status-order__item-product-image" src={item.imageSrc} alt={item.imageAlt} /></a></TableCell>
                                                <TableCell>{item.sku}</TableCell>
                                                <TableCell>{item.name}</TableCell>
                                                <TableCell>{item.quantity}</TableCell>
                                                <TableCell>{item.externalReference}</TableCell>
                                                <TableCell>{item.deliveryFrom && <span>{moment(item.deliveryFrom).format("L")}</span>}</TableCell>
                                                <TableCell>{item.deliveryTo && <span>{moment(item.deliveryTo).format("L")}</span>}</TableCell>
                                                <TableCell>{item.moreInfo}</TableCell>
                                            </TableRow>
                                        ))}
                                    </TableBody>
                                </Table>
                            </TableContainer>
                        </div>
                    </section>
                </div>
            </div>
            {state.isLoading && <CircularProgress className="progressBar" />}
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
    orderStatusesUrl: PropTypes.string.isRequired
};

export default StatusOrder;
