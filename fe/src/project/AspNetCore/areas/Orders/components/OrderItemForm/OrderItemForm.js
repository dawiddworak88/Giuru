import React from "react";
import PropTypes from "prop-types";
import {
    Button, TextField
} from "@mui/material";
import NavigationHelper from "../../../../../../shared/helpers/globals/NavigationHelper";
import OrderItemStatusChanges from "../../../../../../shared/components/OrderItemStatusChanges/OrderItemStatusChanges";

const OrderItemForm = (props) => {

    return (
        <section className="section section-small-padding order-item">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div className="columns is-desktop">
                <div className="column is-half">
                    <div className="order-item__status">{props.orderStatusLabel}: {props.orderStatusName}</div>
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
                        <Button 
                            type="button" 
                            variant="contained" 
                            color="secondary" 
                            onClick={(e) => {
                                e.preventDefault();
                                NavigationHelper.redirect(props.orderUrl);
                            }}>
                            {props.navigateToOrderLabel}
                        </Button> 
                    </div>
                </div>
            </div>
            {props.orderItemStatusChanges &&
                <OrderItemStatusChanges {...props.orderItemStatusChanges} />
            }
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
    outletQuantity: PropTypes.string.isRequired,
    orderStatusLabel: PropTypes.string.isRequired,
    orderStatusCommentLabel: PropTypes.string.isRequired,
    orderUrl: PropTypes.string.isRequired,
    navigateToOrderLabel: PropTypes.string.isRequired,
    orderItemStatusChanges: PropTypes.array
};

export default OrderItemForm;