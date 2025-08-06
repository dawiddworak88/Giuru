import React, { useEffect, useState } from "react";
import { TextField, Button, Dialog, DialogTitle, DialogContent, DialogActions } from "@mui/material";
import PropTypes from "prop-types";
import NavigationHelper from "../../../shared/helpers/globals/NavigationHelper";

const Modal = (props) => {
    const {isOpen, handleOrder, handleClose, labels, product} = props;
    const [quantity, setQuantity] = useState(1);
    const [externalReference, setExternalReference] = useState("");
    const [moreInfo, setMoreInfo] = useState("");
    const [isOutletOrder, setIsOutletOrder] = useState(!product.inStock && product.inOutlet);

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
    }, [isOpen])
    
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
                            min: 1, 
                            step: 1,
                            className: "quantity-input"
                        }}
                        value={quantity}
                        onChange={(e) => {
                            const value = e.target.value;
                            if (value >= 1){
                                setQuantity(value)
                            }
                            else setQuantity(1)
                        }}
                    />
                </div>
                {product.availableOutletQuantity > 0 &&
                    <div className="field">
                        <FormControlLabel
                            control={
                                <Checkbox
                                    checked={isOutletOrder}
                                    onChange={(e) => {
                                        setIsOutletOrder(e.target.checked);
                                    }} />
                            }
                            label={"Product from outlet"}
                            disabled={product.inStock && !product.inOutlet}
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
    handleOrder: PropTypes.func,
    closeLabel: PropTypes.string,
    okLabel: PropTypes.string,
    title: PropTypes.string,
    labels: PropTypes.object
}

export default Modal;