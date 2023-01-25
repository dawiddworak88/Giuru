import React, { useState, useEffect } from "react";
import PropTypes from "prop-types";
import { 
    TextField, Button, Dialog, DialogTitle, 
    DialogContent, DialogActions, FormControl,
    InputLabel, MenuItem, Select
} from "@mui/material";

const ProductCardModal = (props) => {
    const [productAttribute, setProductAttribute] = useState(props.attribute)

    //console.log(props.attribute)

    const updateAttribute = (e) => {
        const value = e.target.value;
        const name = e.target.name;

        setProductAttribute(attribute => ({
            ...attribute, [name]: value
        }))
    }

    useEffect(() => {
        setProductAttribute(props.attribute);
    }, props.attribute)

    return (
        <Dialog
            open={props.isOpen}
            onClose={props.handleClose}
            PaperProps={{
                className:"productCard-modal"
            }}
        >
            <DialogTitle>{props.labels.title}</DialogTitle>
            {productAttribute &&
                <DialogContent>
                    <div className="productCard__container">
                        <div className="field">
                            <TextField
                                id="name"
                                name="name"
                                type="text"
                                value={productAttribute.name} 
                                variant="standard" 
                                label="Name" 
                                onChange={(e) => updateAttribute(e)}
                                fullWidth={true} />
                        </div>
                        <div className="field">
                            <TextField 
                                id="title"
                                name="title"
                                type="text"
                                value={productAttribute.title} 
                                variant="standard" 
                                label="Display name"
                                onChange={(e) => updateAttribute(e)}
                                fullWidth={true} />
                        </div>
                        <div className="field">
                            <FormControl fullWidth={true} variant="standard">
                                <InputLabel id="inputType-label">{props.labels.inputTypeLabel}</InputLabel>
                                <Select
                                    labelId="inputType-label"
                                    id="type"
                                    name="type"
                                    value={productAttribute.type ?? "string"}
                                    onChange={(e) => updateAttribute(e)}
                                    >
                                    <MenuItem key={0} value="">{props.selectJobTitle}</MenuItem>
                                    {props.labels.inputTypes && props.labels.inputTypes.map((type, index) => {
                                        return (
                                            <MenuItem key={index} value={type.name.toLowerCase()}>{type.name}</MenuItem>
                                        )
                                    })}
                                </Select>
                            </FormControl>
                        </div>
                    </div>
                </DialogContent>
            }
            <DialogActions>
                <Button type="text" onClick={props.handleClose}>{props.labels.cancelLabel}</Button>
                <Button 
                    type="text" 
                    variant="contained" 
                    color="primary">
                    {props.labels.saveText}
                </Button>
            </DialogActions>
        </Dialog>
    )
}

ProductCardModal.propTypes = {
    isOpen: PropTypes.bool.isRequired,
    labels: PropTypes.object.isRequired,
    handleClose: PropTypes.func.isRequired
}

export default ProductCardModal;