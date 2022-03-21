import React, { useContext, useState } from "react";
import { Context } from "../../../../../shared/stores/Store";
import { TextField, IconButton } from "@material-ui/core";
import {Clear} from "@material-ui/icons"
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
                    <p class="modal-card-title">Dodawanie do zamówienia</p>
                    <button class="delete" aria-label="close"></button>
                </header>
                <section class="modal-card-body">
                    <div className="modal-container">
                        <div className="field">
                            <TextField 
                                id="quantity" 
                                name="quantity" 
                                type="number"
                                label="Ogólnie"
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
                                    label="Z magazynów"
                                    inputProps={{ 
                                        min: 0, 
                                        step: 1,
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
                                    label="Z wyprzedaży"
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
                                id="moreInfo" 
                                name="moreInfo" 
                                type="text" 
                                label="Numer własny klienta"
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
                                    label="Początkowa data dostawy:"
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
                                    label="Końcowa data dostawy:"
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
                                label="Uwagi"
                                fullWidth={true}
                                onChange={(e) => {
                                    setMoreInfo(e.target.value)
                                }} />
                        </div>
                    </div>
                </section>
                <footer class="modal-card-foot">
                    <button class="button is-success" onClick={() => handleAddItemToBasket()}>Dodaj</button>
                    <button class="button">Anuluj</button>
                </footer>
            </div>
        </div>
    )
}

export default Modal;