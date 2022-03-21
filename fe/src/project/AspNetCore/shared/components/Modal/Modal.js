import React from "react";
import { TextField } from "@material-ui/core";

const Modal = (props) => {
    const {isActive, setIsActive, stockValue, setStockQuantity, quantityValue, setQuantity, outletValue, setOutletQuantity, maxOutletValue} = props;

    return (
        <div class={isActive ? "modal outlet-modal is-active" : "modal outlet-modal is-active"}>
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
                                value={quantityValue}
                                onChange={(e) => {
                                    setQuantity(e.target.value)
                                }}
                            />
                        </div>
                        <div className="field">
                            <TextField 
                                id="quantity" 
                                name="quantity" 
                                type="number" 
                                label="Z magazynów"
                                inputProps={{ 
                                    min: 0, 
                                    step: 1,
                                    style: { textAlign: 'center' }
                                }}
                                value={stockValue} 
                                onChange={(e) => {
                                    setStockQuantity(e.target.value)
                                }}
                            />
                        </div>
                        <div className="field">
                            <TextField 
                                id="quantity" 
                                name="quantity" 
                                type="number" 
                                label="Z wyprzedaży"
                                inputProps={{ 
                                    min: 0, 
                                    step: 1,
                                    max: maxOutletValue,
                                    style: { textAlign: 'center' }
                                }}
                                value={outletValue}
                                onChange={(e) => {
                                    setOutletQuantity(e.target.value)
                                }}
                            />
                        </div>
                    </div>
                    <hr/>
                    <div className="modal-summary">
                        <div className="test">Podsumowanie</div>
                        <div className="test"></div>
                    </div>
                </section>
                <footer class="modal-card-foot">
                    <button class="button is-success">Dodaj</button>
                    <button class="button">Anuluj</button>
                </footer>
            </div>
        </div>
    )
}

export default Modal;