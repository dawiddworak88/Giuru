import React, { useEffect, useState } from "react";
import { 
    TextField, Button, Dialog, DialogTitle, DialogContent, 
    DialogActions, FormControlLabel, Checkbox 
} from "@mui/material";
import PropTypes from "prop-types";
import NavigationHelper from "../../../shared/helpers/globals/NavigationHelper";

const Modal = (props) => {
    const {isOpen, handleOrder, handleClose, labels, product, maxOutletValue, outletQuantityInBasket} = props;
    const [quantity, setQuantity] = useState(1);
    const [externalReference, setExternalReference] = useState("");
    const [moreInfo, setMoreInfo] = useState("");
    const [isOutletOrder, setIsOutletOrder] = useState(false);

    const handleAddItemToBasket = () => {
        const payload = {
            quantity,
            externalReference,
            moreInfo,
            isOutletOrder
        }

        handleOrder(payload)
    }

    useEffect(() => {
        setExternalReference("");
        setMoreInfo("")
        setIsOutletOrder(product ? product.inOutlet && !product.inStock : false);
        setQuantity(isOutletOrder && maxOutletValue == 0 ? 0 : 1);
    }, [isOpen])

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
                        label={isOutletOrder ? `${labels.quantityLabel} ${maxOutlet > 0 ? `(${labels.maximalLabel} ${maxOutlet})` : ""} ${outletQuantityInBasket > 0 ? `(${labels.inBasket} ${outletQuantityInBasket})` : ""}` : labels.quantityLabel}
                        inputProps={{ 
                            min: 0, 
                            step: 1,
                            className: "quantity-input"
                        }}
                        value={quantity}
                        onChange={(e) => {
                            const value = e.target.value;
                            if (value >= 0){
                                setQuantity(isOutletOrder && value > maxOutlet ? maxOutlet : value);
                            }
                            else setQuantity(1)
                        }}
                    />
                </div>
                {product && product.inOutlet &&
                    <div className="field">
                        <FormControlLabel
                            control={
                                <Checkbox
                                    checked={isOutletOrder}
                                    onChange={(e) => {
                                        if (product.inOutlet && !product.inStock) {
                                            setIsOutletOrder(true)
                                        } else {
                                            setIsOutletOrder(e.target.checked);
                                        }
                                    }} />
                            }
                            label={labels.outletProductLabel}
                            disabled={product.inOutlet && !product.inStock}
                        />
                    </div> 
                }
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
                    disabled={(isOutletOrder && maxOutlet == 0) || quantity <= 0}
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
    externalReferenceLabel: PropTypes.string,
    quantityLabel: PropTypes.string,
    outletQuantityInBasket: PropTypes.number,
    maxOutletValue: PropTypes.number,
    handleOrder: PropTypes.func,
    closeLabel: PropTypes.string,
    okLabel: PropTypes.string,
    title: PropTypes.string,
    labels: PropTypes.object
}

export default Modal;