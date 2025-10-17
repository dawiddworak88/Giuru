import React, { useContext, useState } from "react";
import PropTypes from "prop-types";
import {
    FormControl, InputLabel, Select, MenuItem, FormHelperText, 
    CircularProgress, TextField, Button, Autocomplete
} from "@mui/material";
import { Context } from "../../../../shared/stores/Store";
import { toast } from "react-toastify";
import useForm from "../../../../shared/helpers/forms/useForm";
import QuantityValidator from "../../../../shared/helpers/validators/QuantityValidator";
import QueryStringSerializer from "../../../../shared/helpers/serializers/QueryStringSerializer";
import AuthenticationHelper from "../../../../shared/helpers/globals/AuthenticationHelper";
import SearchConstants from "../../../../shared/constants/SearchConstants";

const OutletForm = (props) => {
    const [state, dispatch] = useContext(Context);
    const [products, setProducts] = useState(props.products ? props.products : []);
    const stateSchema = {
        id: { value: props.id ? props.id : null, error: "" },
        warehouseId: { value: props.warehouseId ? props.warehouseId : null, error: "" },
        product: { value: props.product ? props.product : null, error: "" },
        quantity: { value: props.quantity ? props.quantity : 0, error: "" },
        availableQuantity: { value: props.availableQuantity ? props.availableQuantity : 0, error: "" },
        title: { value: props.outletTitle ? props.outletTitle : null },
        description: { value: props.outletDescription ? props.outletDescription : null },
        ean: { value: props.ean ? props.ean : null }
    };

    const productsProps = {
        options: products,
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
                error: props.quantityFormatErrorMessage
            }
        },
        availableQuantity: {
            required: {
                isRequired: true,
                error: props.quantityRequiredErrorMessage
            },
            validator: {
                func: value => QuantityValidator.validateQuantity(value),
                error: props.quantityFormatErrorMessage
            }
        }
    };

    const onSubmitForm = (state) => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        const outletProduct = { id, warehouseId, productId: product.id, quantity, availableQuantity, title, description, ean };

        const requestOptions = {
            method: "POST",
            headers: { 
                "Content-Type": "application/json" 
            },
            body: JSON.stringify(outletProduct)
        };

        fetch(props.saveUrl, requestOptions)
            .then((response) => {
                dispatch({ type: "SET_IS_LOADING", payload: false });

                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {
                    if (response.ok) {
                        toast.success(jsonResponse.message);
                        setFieldValue({ name: "id", value: jsonResponse.id });
                    }
                    else {
                        toast.error(jsonResponse?.message || props.generalErrorMessage);
                    }
                });
            });
    };

    const productsSuggesstionFetchRequest = (e) => {
        const { value } = e.target;

        if (value.length >= SearchConstants.minSearchTermLength()){
            const searchParameters = {
                searchTerm: value,
                pageIndex: 1,
                hasPrimaryProduct: true,
                itemsPerPage: SearchConstants.productsSuggestionItemsPerPage()
            };

            const requestOptions = {
                method: "GET",
                headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" }
            };

            const url = props.productsSuggestionUrl + "?" + QueryStringSerializer.serialize(searchParameters);
            return fetch(url, requestOptions)
                .then((response) => {

                    AuthenticationHelper.HandleResponse(response);

                    return response.json().then(jsonResponse => {
                        if (response.ok) {
                            setProducts(jsonResponse.data);
                        }
                    });
                })
        }
    }

    const {
        values, errors, dirty, setFieldValue,
        handleOnChange, handleOnSubmit
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm, !props.id);

    const { id, warehouseId, product, quantity, availableQuantity, title, description, ean } = values;
    return (
        <section className="section section-small-padding inventory-add">
            <h1 className="subtitle is-4">{props.title}</h1>
                <div className="column is-half inventory-add-content">
                    <form onSubmit={handleOnSubmit} className="is-modern-form" method="post">
                        {id &&
                            <div className="field">
                                <InputLabel id="id-label">{props.idLabel} {id}</InputLabel>
                            </div>
                        }
                        <div className="field">
                            <FormControl fullWidth={true} helperText={dirty.warehouseId ? errors.warehouseId : ""} error={(errors.warehouseId.length > 0) && dirty.warehouseId} variant="standard">
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
                                        variant="standard"
                                        onChange={productsSuggesstionFetchRequest}
                                        margin="normal"/>
                                )}/>
                        </div>
                        <div className="field">
                            <TextField 
                                id="title" 
                                name="title" 
                                type="text" 
                                label={props.titleLabel} 
                                fullWidth={true} 
                                variant="standard"
                                value={title} 
                                onChange={handleOnChange}/>
                        </div>
                        <div className="field">
                            <TextField 
                                id="description" 
                                name="description" 
                                type="text" 
                                label={props.descriptionLabel} 
                                fullWidth={true} 
                                value={description} 
                                variant="standard"
                                onChange={handleOnChange}/>
                        </div>
                        <div className="field">
                            <TextField 
                                id="ean" 
                                name="ean" 
                                type="text" 
                                label={props.eanLabel} 
                                fullWidth={true} 
                                value={ean} 
                                variant="standard"
                                onChange={handleOnChange}/>
                        </div>
                        <div className="field">
                            <TextField 
                                id="quantity" 
                                name="quantity" 
                                type="number" 
                                variant="standard"
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
                                variant="standard"
                                onChange={handleOnChange}/>
                        </div>
                        <div className="field">
                            <Button 
                                type="subbmit" 
                                variant="contained"
                                color="primary"
                                disabled={state.isLoading || (quantity <= -1) || (availableQuantity <= -1) || !product}>
                                {props.saveText}
                            </Button>
                            <a href={props.outletUrl} className="ml-2 button is-text">{props.navigateToOutletListText}</a>
                        </div>
                    </form>
                    {state.isLoading && <CircularProgress className="progressBar" />}
                </div>
        </section>
    );
};

OutletForm.propTypes = {
    title: PropTypes.string.isRequired,
    products: PropTypes.array.isRequired,
    id: PropTypes.string,
    warehouseId: PropTypes.string,
    productId: PropTypes.string,
    warehouses: PropTypes.array,
    saveUrl: PropTypes.string,
    outletUrl: PropTypes.string,
    saveText: PropTypes.string,
    navigateToOutletListText: PropTypes.string,
    selectProductLabel: PropTypes.string,
    selectWarehouseLabel: PropTypes.string,
    availableQuantityLabel: PropTypes.string,
    generalErrorMessage: PropTypes.string,
    quantityLabel: PropTypes.string,
    warehouseRequiredErrorMessage: PropTypes.string,
    productRequiredErrorMessage: PropTypes.string,
    quantityRequiredErrorMessage: PropTypes.string,
    quantityFormatErrorMessage: PropTypes.string,
    idLabel: PropTypes.string,
    titleLabel: PropTypes.string,
    descriptionLabel: PropTypes.string,
    productsSuggestionUrl: PropTypes.string.isRequired
};

export default OutletForm;
