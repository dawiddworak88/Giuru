import React, {useState, useContext} from "react";
import PropTypes from "prop-types";
import Autocomplete from "@material-ui/lab/Autocomplete";
import {
    FormControl, InputLabel, Select, MenuItem, FormHelperText, CircularProgress, IconButton
} from "@material-ui/core";
import { Clear } from "@material-ui/icons";
import { TextField, Button } from "@material-ui/core";
import { Context } from "../../../../../../shared/stores/Store";
import { toast } from "react-toastify";
import useForm from "../../../../../../shared/helpers/forms/useForm";
import NavigationHelper from "../../../../../../shared/helpers/globals/NavigationHelper";
import { MuiPickersUtilsProvider, KeyboardDatePicker,} from "@material-ui/pickers";
import MomentUtils from "@date-io/moment";
import QuantityValidator from "../../../../../../shared/helpers/validators/QuantityValidator";

const InventoryAddForm = (props) => {

    const [state, dispatch] = useContext(Context);
    const [showBackToInventoryListButton, setShowBackToInventoryListButton] = useState(false);
    
    const stateSchema = {
        id: { value: props.id ? props.id : null, error: "" },
        warehouseId: { value: props.warehouseId ? props.warehouseId : null, error: "" },
        product: { value: props.productId ? props.products.find((item) => item.id === props.productId) : null, error: "" },
        quantity: { value: props.quantity ? props.quantity : 0, error: "" },
        restockableInDays: { value: props.restockableInDays ? props.restockableInDays : null, error: "" },
        availableQuantity: { value: props.availableQuantity ? props.availableQuantity : 0, error: "" },
        expectedDelivery: { value: props.expectedDelivery ? props.expectedDelivery : null, error: "" }
    };

    const productsProps = {
        options: props.products,
        getOptionLabel: (option) => option.name + " (" + option.sku + ")"
    };

    const stateValidatorSchema = {
        warehouseId: {
            required: {
                isRequired: true,
                error: props.warehouseRequiredErrorMessage
            }
        },
        quantity: {
            required: {
                isRequired: true,
                error: props.quantityRequiredErrorMessage
            },
            validator: {
                func: value => QuantityValidator.validateQuantity(value),
                error: props.quantityFormatErrorMessage,
            }
        }
    };

    const onSubmitForm = (state) => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        const inventoryProduct = {id, warehouseId, productId: state.product.id, quantity, restockableInDays, availableQuantity, expectedDelivery};

        const requestOptions = {
            method: "POST",
            headers: { 
                "Content-Type": "application/json" 
            },
            body: JSON.stringify(inventoryProduct)
        };

        fetch(props.saveUrl, requestOptions)
            .then((res) => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                return res.json().then(jsonRes => {
                    if (res.ok) {
                        toast.success(jsonRes.message);
                        setShowBackToInventoryListButton(true);
                    }
                    else {
                        toast.error(props.generalErrorMessage);
                    }
                });
            });
    };

    const {
        values, errors, dirty, disable, setFieldValue,
        handleOnChange, handleOnSubmit
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm, !props.id);

    const {id, warehouseId, product, quantity, restockableInDays, availableQuantity, expectedDelivery } = values;
    return (
        <section className="section section-small-padding inventory-add">
            <h1 className="subtitle is-4">{props.title}</h1>
                <div className="column is-half inventory-add-content">
                    <form onSubmit={handleOnSubmit} className="is-modern-form" method="post">
                        {id &&
                            <input id="id" name="id" type="hidden" value={id} />
                        }
                        <div className="field">
                            <FormControl fullWidth={true} helperText={dirty.warehouseId ? errors.warehouseId : ""} error={(errors.warehouseId.length > 0) && dirty.warehouseId}>
                                <InputLabel id="warehouse-label">{props.selectWarehouseLabel}</InputLabel>
                                <Select
                                    name="warehouseId"
                                    labelId="warehouse-label"
                                    id="warehouse-select"
                                    value={warehouseId}
                                    label={props.selectWarehouseLabel}
                                    onChange={handleOnChange}>
                                        <MenuItem key={0} value={""}>{props.selectWarehouse}</MenuItem>
                                    {props.warehouses && props.warehouses.map((item, i) => {
                                        return (
                                            <MenuItem key={item.id} value={item.id}>{item.name}</MenuItem>
                                        );
                                    })}
                                </Select>
                                {errors.warehouseId && dirty.warehouseId && (
                                    <FormHelperText>{errors.warehouseId}</FormHelperText>
                                )}
                            </FormControl>
                        </div>
                        <div className="field">
                            <Autocomplete
                                {...productsProps}
                                id="productId"
                                name="productId"
                                fullWidth={true}
                                value={product}
                                onChange={(event, newValue) => {
                                    setFieldValue({name: "product", value: newValue});
                                }}
                                autoComplete
                                renderInput={(params) => (
                                    <TextField 
                                        {...params} 
                                        label={props.selectProductLabel} 
                                        margin="normal"/>
                                )}/>
                        </div>
                        <div className="field">
                            <TextField 
                                id="quantity" 
                                name="quantity" 
                                type="number" 
                                inputProps={{ 
                                    min: "1", 
                                    step: "1" 
                                }}
                                label={props.quantityLabel} 
                                fullWidth={true} 
                                value={quantity} 
                                onChange={handleOnChange}
                                error={(errors.quantity.length > 0) && dirty.quantity}
                                helperText={dirty.quantity ? errors.quantity : ""} />
                        </div>
                        <div className="field">
                            <TextField 
                                id="availableQuantity" 
                                name="availableQuantity" 
                                type="number" 
                                inputProps={{ 
                                    min: "0", 
                                    step: "1" 
                                }}
                                label={props.availableQuantityLabel} 
                                fullWidth={true} 
                                value={availableQuantity} 
                                onChange={handleOnChange}/>
                        </div>
                        <div className="field">
                            <TextField 
                                id="restockableInDays" 
                                name="restockableInDays" 
                                type="number" 
                                inputProps={{ 
                                    min: "0", 
                                    step: "1" 
                                }}
                                label={props.restockableInDaysLabel} 
                                fullWidth={true} 
                                value={restockableInDays} 
                                onChange={handleOnChange}/>
                        </div>
                        <div className="field">
                            <MuiPickersUtilsProvider utils={MomentUtils} >
                                <KeyboardDatePicker
                                    id="expectedDelivery"
                                    label={props.expectedDeliveryLabel}
                                    value={expectedDelivery}
                                    fullWidth={true} 
                                    onChange={(date) => {
                                        setFieldValue({name: "expectedDelivery", value: date});
                                    }}
                                    okLabel={props.okLabel}
                                    cancelLabel={props.cancelLabel}
                                    InputProps={{
                                        endAdornment: (
                                            <IconButton onClick={() => setFieldValue({name: "expectedDelivery", value: null})}>
                                                <Clear />
                                            </IconButton>
                                        )
                                    }}
                                    InputAdornmentProps={{
                                        position: "start"
                                    }}
                                    KeyboardButtonProps={{
                                        "aria-label": props.changeExpectedDeliveryLabel
                                    }} 
                                    disablePast={true}/>
                            </MuiPickersUtilsProvider>
                        </div>
                        <div className="field">
                            {showBackToInventoryListButton ? (
                                <Button 
                                    type="button" 
                                    variant="contained" 
                                    color="primary" 
                                    onClick={(e) => {
                                        e.preventDefault();
                                        NavigationHelper.redirect(props.inventoryUrl);
                                    }}>
                                    {props.navigateToInventoryListText}
                                </Button> 
                            ) : (
                                <Button 
                                    type="subbmit" 
                                    variant="contained"
                                    color="primary"
                                    disabled={state.isLoading || disable || !product}>
                                    {props.saveText}
                                </Button>
                            )}
                        </div>
                    </form>
                    {state.isLoading && <CircularProgress className="progressBar" />}
                </div>
        </section>
    );
};

InventoryAddForm.propTypes = {
    title: PropTypes.string.isRequired,
    products: PropTypes.array.isRequired,
    id: PropTypes.string,
    warehouseId: PropTypes.string,
    productId: PropTypes.string,
    warehouses: PropTypes.array,
    saveUrl: PropTypes.string,
    inventoryUrl: PropTypes.string,
    saveText: PropTypes.string,
    navigateToInventoryListText: PropTypes.string,
    selectProductLabel: PropTypes.string,
    selectWarehouseLabel: PropTypes.string,
    availableQuantityLabel: PropTypes.string,
    restockableInDaysLabel: PropTypes.string,
    expectedDeliveryLabel: PropTypes.string,
    changeExpectedDeliveryLabel: PropTypes.string,
    generalErrorMessage: PropTypes.string,
    quantityLabel: PropTypes.string,
    warehouseRequiredErrorMessage: PropTypes.string,
    productRequiredErrorMessage: PropTypes.string,
    quantityRequiredErrorMessage: PropTypes.string,
    quantityFormatErrorMessage: PropTypes.string
};

export default InventoryAddForm;