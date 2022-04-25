import React, { useEffect, useState } from "react";
import { TextField, IconButton, Button, Dialog, DialogTitle, DialogContent, DialogActions } from "@material-ui/core";
import { Clear } from "@material-ui/icons";
import PropTypes from "prop-types";
import {
    MuiPickersUtilsProvider,
    KeyboardDatePicker,
} from "@material-ui/pickers";
import MomentUtils from "@date-io/moment";
import NavigationHelper from "../../../../../shared/helpers/globals/NavigationHelper";

const Modal = (props) => {
    const {isOpen, maxStockValue, maxOutletValue, handleOrder, handleClose, labels, product} = props;
    const [quantity, setQuantity] = useState(maxStockValue || maxOutletValue ? 0 : 1);
    const [stockQuantity, setStockQuantity] = useState(maxStockValue && maxStockValue > 0 ? 1 : 0);
    const [outletQuantity, setOutletQuantity] = useState(maxOutletValue && maxOutletValue > 0 ? 1 : 0);
    const [externalReference, setExternalReference] = useState(null);
    const [deliveryFrom, setDeliveryFrom] = useState(null);
    const [deliveryTo, setDeliveryTo] = useState(null);
    const [moreInfo, setMoreInfo] = useState(null);

    const handleAddItemToBasket = () => {
        const payload = {
            quantity,
            stockQuantity,
            outletQuantity,
            externalReference,
            deliveryFrom,
            deliveryTo,
            moreInfo
        }

        handleOrder(payload)
    }

    useEffect(() => {
        setQuantity(maxStockValue || maxOutletValue ? 0 : 1);
        setStockQuantity(maxStockValue && maxStockValue > 0 ? 1 : 0);
        setOutletQuantity(maxOutletValue && maxOutletValue > 0 ? 1 : 0);
        setExternalReference(null);
        setDeliveryFrom(null);
        setDeliveryTo(null);
        setMoreInfo(null)
    }, [isOpen])
    
    const maxStock = maxStockValue ? maxStockValue : 0;
    const maxOutlet = maxOutletValue ? maxOutletValue : 0;
    return (
        <Dialog
            open={isOpen}
            onClose={handleClose}
            PaperProps={{
                className:"basket-modal"
            }}>
            <DialogTitle>{labels.title}</DialogTitle>
            <DialogContent>
                {product &&
                    <div className="mb-3">
                        <p className="basket-modal__sku">{labels.skuLabel} {product.sku ? product.sku : product.subtitle}</p>
                        {product.ean &&
                            <p className="basket-modal__ean">{labels.eanLabel} {product.ean}</p>
                        }
                        <h1 className="title is-4 mt-1">{product.title}</h1>
                    </div>
                }
                <div className="field">
                    <TextField 
                        id="quantity" 
                        name="quantity" 
                        type="number"
                        label={labels.quantityLabel}
                        inputProps={{ 
                            min: 0, 
                            step: 1,
                            className: "quantity-input"
                        }}
                        value={quantity}
                        onChange={(e) => {
                            const value = e.target.value;
                            if (value >= 0){
                                setQuantity(value)
                            }
                            else setQuantity(0)
                        }}
                    />
                </div>
                <div className="field">
                    <TextField 
                        id="stockQuantity" 
                        name="stockQuantity" 
                        type="number" 
                        label={labels.stockQuantityLabel + " (" + labels.maximalLabel + " " + maxStock + ")"}
                        inputProps={{ 
                            min: 0, 
                            step: 1,
                            className: "quantity-input"
                        }}
                        value={stockQuantity}
                        onChange={(e) => {
                            const value = e.target.value;
                            if (value >= 0){
                                setStockQuantity(value > maxStock ? maxStock : value);
                            }
                            else setStockQuantity(0)
                        }}
                    />
                </div>
                <div className="field">
                    <TextField 
                        id="outletQuantity" 
                        name="outletQuantity" 
                        type="number" 
                        label={labels.outletQuantityLabel + " (" + labels.maximalLabel + " " + maxOutlet + ")"}
                        inputProps={{ 
                            min: 0, 
                            step: 1,
                            className: "quantity-input"
                        }}
                        value={outletQuantity}
                        onChange={(e) => {
                            const value = e.target.value;
                            if (value >= 0){
                                setOutletQuantity(value > maxOutlet ? maxOutlet : value);
                            }
                            else setOutletQuantity(0)
                        }}
                    />
                </div>
                <div className="field">
                    <TextField 
                        id="externalReference" 
                        name="externalReference" 
                        type="text" 
                        label={labels.externalReferenceLabel}
                        value={externalReference}
                        fullWidth={true}
                        onChange={(e) => {
                            setExternalReference(e.target.value);
                        }} />
                </div>
                <div className="field">
                    <MuiPickersUtilsProvider utils={MomentUtils}>
                        <KeyboardDatePicker
                            id="deliveryFrom"
                            label={labels.deliveryFromLabel}
                            onChange={(date) => {
                                setDeliveryFrom(date);
                            }}
                            okLabel={labels.okLabel}
                            cancelLabel={labels.cancelLabel}
                            InputProps={{
                                endAdornment: (
                                    <IconButton onClick={() => setDeliveryFrom(null)}>
                                        <Clear />
                                    </IconButton>
                                )
                            }}
                            InputAdornmentProps={{
                                position: "start"
                            }}
                            KeyboardButtonProps={{
                                "aria-label": labels.changeDeliveryFromLabel
                            }} 
                            value={deliveryFrom}
                            fullWidth={true}
                            disablePast={true}/>
                    </MuiPickersUtilsProvider>
                </div>
                <div className="field">
                    <MuiPickersUtilsProvider utils={MomentUtils}>
                        <KeyboardDatePicker
                            id="deliveryTo"
                            label={labels.deliveryToLabel}
                            onChange={(date) => {
                                setDeliveryTo(date);
                            }}
                            okLabel={labels.okLabel}
                            cancelLabel={labels.cancelLabel}
                            InputProps={{
                                endAdornment: (
                                    <IconButton onClick={() => setDeliveryTo(null)}>
                                        <Clear />
                                    </IconButton>
                                )
                            }}
                            InputAdornmentProps={{
                                position: "start"
                            }}
                            KeyboardButtonProps={{
                                "aria-label": labels.changeDeliveryFromLabel
                            }} 
                            fullWidth={true}
                            value={deliveryTo}
                            disablePast={true}/>
                    </MuiPickersUtilsProvider>
                </div>
                <div className="field">
                    <TextField 
                        id="moreInfo" 
                        name="moreInfo" 
                        type="text" 
                        value={moreInfo}
                        label={labels.moreInfoLabel}
                        fullWidth={true}
                        onChange={(e) => {
                            setMoreInfo(e.target.value)
                        }} />
                </div>
            </DialogContent>
            <DialogActions>
                <Button type="text" onClick={() => NavigationHelper.redirect(labels.basketUrl)}>{labels.basketLabel}</Button>
                <Button 
                    type="text" 
                    variant="contained" 
                    color="primary" 
                    onClick={() => handleAddItemToBasket()}>
                        {labels.toBasketText}
                </Button>
            </DialogActions>
        </Dialog>
    )
}

Modal.propTypes = {
    isOpen: PropTypes.func,
    addText: PropTypes.string,
    cancelLabel: PropTypes.string,
    moreInfoLabel: PropTypes.string,
    maxOutletValue: PropTypes.number,
    deliveryToLabel: PropTypes.string,
    deliveryFromLabel: PropTypes.string,
    externalReferenceLabel: PropTypes.string,
    outletQuantityLabel: PropTypes.string,
    stockQuantityLabel: PropTypes.string,
    quantityLabel: PropTypes.string,
    maxStockValue: PropTypes.number,
    handleOrder: PropTypes.string,
    closeLabel: PropTypes.string,
    okLabel: PropTypes.string,
    title: PropTypes.string,
    labels: PropTypes.object
}

export default Modal;