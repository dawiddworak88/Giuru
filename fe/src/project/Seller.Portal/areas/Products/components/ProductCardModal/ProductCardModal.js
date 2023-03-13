import React, { useState, useEffect } from "react";
import NavigationHelper from "../../../../../../shared/helpers/globals/NavigationHelper";
import PropTypes from "prop-types";
import { 
    TextField, Button, Dialog, DialogTitle, 
    DialogContent, DialogActions, FormControl,
    InputLabel, MenuItem, Select
} from "@mui/material";
import ProductCardConstants from "../../../../../../shared/constants/ProductCardConstants";

const ProductCardModal = (props) => {
    const [productAttribute, setProductAttribute] = useState(props.attribute)

    const updateAttribute = (e) => {
        const value = e.target.value;
        const name = e.target.name;

        props.setAttribute(attribute => ({
            ...attribute, [name]: value
        }))

        console.log(productAttribute)
    }

    const handleDefinition = () => {
        if (productAttribute.definitionId != undefined) {
            const currentDefinition = props.labels.definitionsOptions.find((definition) => definition.id === productAttribute.definitionId);

            if (currentDefinition != null) {
                NavigationHelper.redirect(currentDefinition.url, "_blank");
            }
        }
    }

    const validateName = (e) => {
        
    }

    useEffect(() => {
        setProductAttribute(props.attribute);
    }, [props.attribute])

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
                                id="title"
                                name="title"
                                type="text"
                                value={productAttribute.title} 
                                variant="standard" 
                                label={props.labels.displayNameLabel}
                                onChange={(e) => updateAttribute(e)}
                                fullWidth={true} />
                        </div>
                        <div className="field">
                            <TextField
                                id="name"
                                name="name"
                                type="text"
                                value={productAttribute.name} 
                                variant="standard" 
                                label={props.labels.nameLabel}
                                onChange={(e) => updateAttribute(e)}
                                inputProps={{
                                    // inputMode: "text",
                                    pattern: "[a-z]"
                                }}
                                InputProps={{
                                    pattern: "[a-z]"
                                }}
                                fullWidth={true} />
                        </div>
                        <div className="field">
                            <FormControl fullWidth={true} variant="standard">
                                <InputLabel id="inputType-label">{props.labels.inputTypeLabel}</InputLabel>
                                <Select
                                    labelId="inputType-label"
                                    id="type"
                                    name="type"
                                    value={productAttribute.definitionId !== undefined ? ProductCardConstants.referenceInputType() : productAttribute.type ?? ProductCardConstants.defaultInputType()}
                                    onChange={(e) => updateAttribute(e)}
                                    >
                                        {props.labels.inputTypes && props.labels.inputTypes.map((type, index) => {
                                            return (
                                                <MenuItem key={index} value={type.value}>{type.text}</MenuItem>
                                            )
                                        })}
                                </Select>
                            </FormControl>
                        </div>
                        {(productAttribute.type === ProductCardConstants.referenceInputType() || productAttribute.definitionId !== undefined)&&
                            <div className="field columns">
                                <div className="column">
                                    <FormControl fullWidth={true} variant="standard">
                                        <InputLabel id="definition-label">{props.labels.definitionLabel}</InputLabel>
                                        <Select
                                            id="definitionId"
                                            name="definitionId"
                                            value={productAttribute.definitionId ?? ""}
                                            onChange={(e) => updateAttribute(e)}
                                        >
                                            {props.labels.definitionsOptions && props.labels.definitionsOptions.map((definition, index) => {
                                                return (
                                                    <MenuItem key={index} value={definition.id}>{definition.name}</MenuItem>
                                                )
                                            })}
                                        </Select>
                                    </FormControl>
                                </div>
                                <div className="column">
                                    <Button 
                                        type="text" 
                                        variant="contained" 
                                        fullWidth={true} 
                                        color="primary" 
                                        onClick={handleDefinition} 
                                        size="large" 
                                        className="mt-2"
                                        disabled={productAttribute.definitionId != undefined ? false : true}>
                                            {props.labels.toDefinitionText}
                                    </Button>
                                </div>
                            </div>
                        }
                    </div>
                </DialogContent>
            }
            <DialogActions>
                <Button type="text" onClick={props.handleClose}>{props.labels.cancelText}</Button>
                <Button 
                    type="text" 
                    variant="contained" 
                    color="primary"
                    onClick={props.handleSave}>
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