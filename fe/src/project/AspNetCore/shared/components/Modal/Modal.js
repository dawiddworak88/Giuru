import React, { useContext, useState } from "react";
import { Context } from "../../../../../shared/stores/Store";
import { TextField, IconButton } from "@material-ui/core";
import {Clear} from "@material-ui/icons";
import PropTypes from "prop-types";
import {
    MuiPickersUtilsProvider,
    KeyboardDatePicker,
} from "@material-ui/pickers";
import MomentUtils from "@date-io/moment";

const Modal = (props) => {
    const [state, dispatch] = useContext(Context);
    const [quantity, setQuantity] = useState(0);
    const [stockQuantity, setStockQuantity] = useState(0);
    const [outletQuantity, setOutletQuantity] = useState(0);
    const [externalReference, setExternalReference] = useState(null);
    const [deliveryFrom, setDeliveryFrom] = useState(null);
    const [deliveryTo, setDeliveryTo] = useState(null);
    const [moreInfo, setMoreInfo] = useState(null);
    const {isOpen, maxStockValue, maxOutletValue, handleOrder} = props;

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

    return (
        <div class={isOpen ? "modal outlet-modal is-active" : "modal outlet-modal"}>
            <div class="modal-background"></div>
            <div class="modal-card">
                <header class="modal-card-head">
                    <p class="modal-card-title">{props.addToBasketText}</p>
                    <button class="delete" aria-label={props.closeLabel}></button>
                </header>
                <section class="modal-card-body">
                    <div className="modal-container">
                        <div className="field">
                            <TextField 
                                id="quantity" 
                                name="quantity" 
                                type="number"
                                label={props.quantityLabel}
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
                                    label={props.stockQuantityLabel}
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
                                    label={props.outletQuantityLabel}
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
                                label={props.externalReferenceLabel}
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
                                    label={props.deliveryFromLabel}
                                    onChange={(date) => {
                                        setDeliveryFrom(date);
                                    }}
                                    okLabel={props.okLabel}
                                    cancelLabel={props.cancelLabel}
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
                                        "aria-label": props.changeDeliveryFromLabel
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
                                    label={props.deliveryToLabel}
                                    onChange={(date) => {
                                        setDeliveryTo(date);
                                    }}
                                    okLabel={props.okLabel}
                                    cancelLabel={props.cancelLabel}
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
                                        "aria-label": props.changeDeliveryFromLabel
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
                                label={props.moreInfoLabel}
                                fullWidth={true}
                                onChange={(e) => {
                                    setMoreInfo(e.target.value)
                                }} />
                        </div>
                    </div>
                </section>
                <footer class="modal-card-foot">
                    <button class="button is-success" onClick={() => handleAddItemToBasket()}>{props.addText}</button>
                    <button class="button">{props.cancelLabel}</button>
                </footer>
            </div>
        </div>
    )
}

Modal.propTypes = {
    isOpen: PropTypes.string,
    addText: PropTypes.string,
    cancelLabel: PropTypes.string,
    moreInfoLabel: PropTypes.string,
    maxOutletValue: PropTypes.string,
    deliveryToLabel: PropTypes.string,
    deliveryFromLabel: PropTypes.string,
    externalReferenceLabel: PropTypes.string,
    outletQuantityLabel: PropTypes.string,
    stockQuantityLabel: PropTypes.string,
    addToBasketText: PropTypes.string,
    quantityLabel: PropTypes.string,
    maxStockValue: PropTypes.string,
    handleOrder: PropTypes.string,
    closeLabel: PropTypes.string,
    okLabel: PropTypes.string,
}

export default Modal;