import React from "react";
import PropTypes from "prop-types";
import {
    Table, TableBody, TableCell, TableContainer,
    TableHead, TableRow, Paper, TextField
} from "@mui/material";
import moment from "moment";
import OrderFormConstants from "../../../../../../shared/constants/OrderFormConstants";

function StatusOrder(props) {

    const status = props.orderStatuses.find((item) => item.id === props.orderStatusId);
    return (
        <section className="section status-order">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div className="columns is-desktop">
                {status &&
                    <div className="column is-3">
                        <div className="status-ordder__details">{props.orderStatusLabel}: {status.name}</div>
                        {props.expectedDelivery && 
                            <div className="status-ordder__details">{props.expectedDeliveryLabel}: {moment(props.expectedDelivery).format("L")}</div>
                        }
                    </div>
                }
            </div>
            {props.orderItems && props.orderItems.length > 0 &&
                <div className="mt-5">
                    <h2 className="subtitle is-5 mb-2">{props.orderItemsLabel}</h2>
                    <div className="status-order__items">
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
                                                <TableCell>{props.stockQuantityLabel}</TableCell>
                                                <TableCell>{props.outletQuantityLabel}</TableCell>
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
                                                        <TableCell>{item.stockQuantity}</TableCell>
                                                        <TableCell>{item.outletQuantity}</TableCell>
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
            }
            {props.customOrder &&
                <div className="mt-5">
                    <h2 className="subtitle is-5 mb-2">{props.customOrderLabel}</h2>
                    <div className="status-order__items">
                        <TextField 
                            value={props.customOrder}
                            fullWidth={true}
                            multiline={true}
                            disabled={true}
                            rows={OrderFormConstants.minRowsForCustomOrder()}
                            InputProps={{ 
                                className: "p-2" 
                            }}
                        />
                    </div>
                </div>
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
