import React, { Fragment } from "react";
import PropTypes from "prop-types";
import {
    Table, TableBody, TableCell, TableContainer,
    TableHead, TableRow, Paper, TextField, Fab
} from "@mui/material";
import { Edit } from "@mui/icons-material";
import moment from "moment";
import Files from "../../../../../../shared/components/Files/Files";

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
                                                    <TableCell>{item.orderStatusName}</TableCell>
                                                    <TableCell>{item.orderStatusComment}</TableCell>
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
