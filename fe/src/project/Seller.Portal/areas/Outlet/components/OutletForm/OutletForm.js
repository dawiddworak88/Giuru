import React, {useContext, useState} from "react";
import { Context } from "../../../../../../shared/stores/Store";
import useForm from "../../../../../../shared/helpers/forms/useForm";
import { CircularProgress, TextField, Button } from "@material-ui/core";
import Autocomplete from "@material-ui/lab/Autocomplete";
import NavigationHelper from "../../../../../../shared/helpers/globals/NavigationHelper";
import { toast } from "react-toastify";

const OutletForm = (props) => {
    const [state, dispatch] = useContext(Context);
    const [showBackToOutletListButton, setShowBackToOutletListButton] = useState(false);
    const stateSchema = {
        id: { value: props.id ? props.id : null, error: "" },
        product: { value: props.productId ? props.products.find((item) => item.id === props.productId) : null, error: "" }
    };

    const productsProps = {
        options: props.products,
        getOptionLabel: (option) => option.name + " (" + option.sku + ")"
    };

    const stateValidatorSchema = {
        
    };

    const onSubmitForm = (state) => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        const requestBody = {
            id: state.id, 
            productId: state.product.id,
            productName: state.product.name,
            productSku: state.product.sku
        }

        const requestOptions = {
            method: "POST",
            headers: { 
                "Content-Type": "application/json" 
            },
            body: JSON.stringify(requestBody)
        };

        fetch(props.saveUrl, requestOptions)
            .then((res) => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                return res.json().then(jsonResponse => {
                    if (res.ok) {
                        toast.success(jsonResponse.message);
                        setShowBackToOutletListButton(true);
                    }
                    else {
                        toast.error(props.generalErrorMessage);
                    }
                });
            });
    }
    
    const {
        values, setFieldValue, handleOnSubmit
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm, !props.id);

    const { id, product } = values;
    return (
        <section className="section section-small-padding inventory-add">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div className="column is-half inventory-add-content">
                <form onSubmit={handleOnSubmit} className="is-modern-form">
                    {id &&
                        <input id="id" name="id" type="hidden" value={id} />
                    }
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
                        {showBackToOutletListButton ? (
                            <Button 
                                type="button" 
                                variant="contained" 
                                color="primary" 
                                onClick={(e) => {
                                    e.preventDefault();
                                    NavigationHelper.redirect(props.outletUrl);
                                }}>
                                {props.navigateToOutletListText}
                            </Button> 
                        ) : (
                            <Button 
                                type="subbmit" 
                                variant="contained"
                                color="primary"
                                disabled={state.isLoading || !product}>
                                {props.saveText}
                            </Button>
                        )}
                    </div>
                </form>
                {state.isLoading && <CircularProgress className="progressBar" />}
            </div>
        </section>
    )
}

export default OutletForm;