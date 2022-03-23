import React, { useEffect, useState } from "react";
import { TextField, IconButton } from "@material-ui/core";
import { Clear } from "@material-ui/icons";
import PropTypes from "prop-types";
import {
    MuiPickersUtilsProvider,
    KeyboardDatePicker,
} from "@material-ui/pickers";
import MomentUtils from "@date-io/moment";

const Modal = (props) => {
    const [quantity, setQuantity] = useState(0);
    const [stockQuantity, setStockQuantity] = useState(0);
    const [outletQuantity, setOutletQuantity] = useState(0);
    const [externalReference, setExternalReference] = useState(null);
    const [deliveryFrom, setDeliveryFrom] = useState(null);
    const [deliveryTo, setDeliveryTo] = useState(null);
    const [moreInfo, setMoreInfo] = useState(null);

    const {isOpen, setIsOpen, maxStockValue, maxOutletValue, handleOrder, labels} = props;
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
        setQuantity(0);
        setStockQuantity(0);
        setOutletQuantity(0);
        setExternalReference("");
        setDeliveryFrom(null);
        setDeliveryTo(null);
        setMoreInfo("")
    }, [isOpen])

    return (
        <div class={isOpen ? "modal outlet-modal is-active" : "modal outlet-modal fade-in"}>
            <div class="modal-background"></div>
            <div class="modal-card">
                <header class="modal-card-head">
                    <p class="modal-card-title">{labels.title}</p>
                    <button class="delete" aria-label={labels.closeLabel} onClick={() => setIsOpen(false)}></button>
                </header>
                <section class="modal-card-body">
                    <div className="modal-container">
                        <div className="field">
                            <TextField 
                                id="quantity" 
                                name="quantity" 
                                type="number"
                                label={labels.quantityLabel}
                                inputProps={{ 
                                    min: 0, 
                                    step: 1,
                                    style: { textAlign: 'center' }
                                }}
                                value={quantity}
                                onChange={(e) => {
                                    setQuantity(e.target.value)
                                }}
                            />
                        </div>
                        {maxStockValue && maxStockValue > 0 &&
                            <div className="field">
                                <TextField 
                                    id="stockQuantity" 
                                    name="stockQuantity" 
                                    type="number" 
                                    label={labels.stockQuantityLabel}
                                    inputProps={{ 
                                        min: 0, 
                                        step: 1,
                                        max: maxStockValue,
                                        style: { textAlign: 'center' }
                                    }}
                                    value={stockQuantity} 
                                    onChange={(e) => {
                                        setStockQuantity(e.target.value)
                                    }}
                                />
                            </div>
                        }
                        {maxOutletValue && maxOutletValue > 0 &&
                            <div className="field">
                                <TextField 
                                    id="outletQuantity" 
                                    name="outletQuantity" 
                                    type="number" 
                                    label={labels.outletQuantityLabel}
                                    inputProps={{ 
                                        min: 0, 
                                        step: 1,
                                        max: maxOutletValue,
                                        style: { textAlign: 'center' }
                                    }}
                                    value={outletQuantity}
                                    onChange={(e) => {
                                        setOutletQuantity(e.target.value)
                                    }}
                                />
                            </div>
                        }
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
                                label={labels.moreInfoLabel}
                                fullWidth={true}
                                onChange={(e) => {
                                    setMoreInfo(e.target.value)
                                }} />
                        </div>
                    </div>
                </section>
                <footer class="modal-card-foot">
                    <button class="button is-success" onClick={() => handleAddItemToBasket()}>{labels.addText}</button>
                    <button class="button" onClick={() => setIsOpen(false)}>{labels.cancelLabel}</button>
                </footer>
            </div>
        </div>
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