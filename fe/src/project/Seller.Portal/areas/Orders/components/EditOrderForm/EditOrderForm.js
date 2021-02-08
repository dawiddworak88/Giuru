import React, { useContext } from "react";
import PropTypes from "prop-types";
import { Context } from "../../../../../../shared/stores/Store";
import { CircularProgress } from "@material-ui/core";
import {
    Table, TableBody, TableCell, TableContainer,
    TableHead, TableRow, Paper
} from "@material-ui/core";
import moment from "moment";

function EditOrderForm(props) {

    const [state, dispatch] = useContext(Context);

    return (
        <section className="section section-small-padding edit-order">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div className="is-modern-form">
                <h2 className="subtitle is-5 order__items-subtitle">{props.orderItemsLabel}</h2>
                <div className="order__items">
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
                                                <TableCell><a href={item.productUrl} target="_blank"><img className="order__item-product-image" src={item.imageSrc} alt={item.imageAlt} /></a></TableCell>
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

EditOrderForm.propTypes = {
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
    generalErrorMessage: PropTypes.string.isRequired,
    saveText: PropTypes.string.isRequired
};

export default EditOrderForm;
