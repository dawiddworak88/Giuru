import React, { useEffect, useState } from "react";
import { TextField, Button, Dialog, DialogTitle, DialogContent, DialogActions } from "@mui/material";
import PropTypes from "prop-types";
import NavigationHelper from "../../../shared/helpers/globals/NavigationHelper";

const Modal = (props) => {
    const {isOpen, maxStockValue, maxOutletValue, stockQuantityInBasket, outletQuantityInBasket, handleOrder, handleClose, labels, product} = props;
    const [quantity, setQuantity] = useState(maxStockValue || maxOutletValue ? 0 : 1);
    const [stockQuantity, setStockQuantity] = useState(maxStockValue && maxStockValue > 0 && maxOutletValue && maxOutletValue > 0 ? 1 : maxStockValue && maxStockValue > 0 ? 1 : 0);
    const [outletQuantity, setOutletQuantity] = useState(maxStockValue && maxStockValue > 0 && maxOutletValue && maxOutletValue > 0 ? 0 : maxOutletValue && maxOutletValue > 0 ? 1 : 0);
    const [externalReference, setExternalReference] = useState("");
    const [moreInfo, setMoreInfo] = useState("");

    const handleAddItemToBasket = () => {
        const payload = {
            quantity,
            stockQuantity,
            outletQuantity,
            externalReference,
            moreInfo
        }

        handleOrder(payload)
    }

    useEffect(() => {
        setQuantity(maxStockValue || maxOutletValue ? 0 : 1);
        setStockQuantity(maxStockValue && maxStockValue > 0 && maxOutletValue && maxOutletValue > 0 ? 1 : maxStockValue && maxStockValue > 0 ? 1 : 0);
        setOutletQuantity(maxStockValue && maxStockValue > 0 && maxOutletValue && maxOutletValue > 0 ? 0 : maxOutletValue && maxOutletValue > 0 ? 1 : 0);
        setExternalReference("");
        setMoreInfo("")
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
                        variant="standard"
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
                        variant="standard"
                        label={`${labels.stockQuantityLabel} (${labels.maximalLabel} ${maxStock}) ${stockQuantityInBasket > 0 ? `(${labels.inBasket} ${stockQuantityInBasket})` : ""}`}
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
                        variant="standard"
                        label={`${labels.outletQuantityLabel} (${labels.maximalLabel} ${maxOutlet}) ${outletQuantityInBasket > 0 ? `(${labels.inBasket} ${outletQuantityInBasket})` : ""}`}
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
                        variant="standard"
                        label={labels.externalReferenceLabel}
                        value={externalReference}
                        fullWidth={true}
                        onChange={(e) => {
                            setExternalReference(e.target.value);
                        }} />
                </div>
                <div className="field">
                    <TextField 
                        id="moreInfo" 
                        name="moreInfo" 
                        type="text" 
                        value={moreInfo}
                        label={labels.moreInfoLabel}
                        fullWidth={true}
                        variant="standard"
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
    isOpen: PropTypes.bool,
    addText: PropTypes.string,
    cancelLabel: PropTypes.string,
    moreInfoLabel: PropTypes.string,
    maxOutletValue: PropTypes.number,
    externalReferenceLabel: PropTypes.string,
    outletQuantityLabel: PropTypes.string,
    stockQuantityLabel: PropTypes.string,
    quantityLabel: PropTypes.string,
    maxStockValue: PropTypes.number,
    handleOrder: PropTypes.func,
    closeLabel: PropTypes.string,
    okLabel: PropTypes.string,
    title: PropTypes.string,
    labels: PropTypes.object
}

export default Modal;