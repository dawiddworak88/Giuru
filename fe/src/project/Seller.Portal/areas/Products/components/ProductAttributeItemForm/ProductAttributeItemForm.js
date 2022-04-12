import React, { useContext, useState } from "react";
import PropTypes from "prop-types";
import { toast } from "react-toastify";
import { Context } from "../../../../../../shared/stores/Store";
import useForm from "../../../../../../shared/helpers/forms/useForm";
import { TextField, Button, CircularProgress, InputLabel } from "@material-ui/core";
import AuthenticationHelper from "../../../../../../shared/helpers/globals/AuthenticationHelper";
import NavigationHelper from "../../../../../../shared/helpers/globals/NavigationHelper";

function ProductAttributeItemForm(props) {
    const [disableSaveButton, setDisableSaveButton] = useState(false);
    const [state, dispatch] = useContext(Context);

    const stateSchema = {

        id: { value: props.id ? props.id : null, error: "" },
        name: { value: props.name ? props.name : "", error: "" },
        productAttributeId: { value: props.productAttributeId ? props.productAttributeId : null }
    };

    const stateValidatorSchema = {

        name: {
            required: {
                isRequired: true,
                error: props.nameRequiredErrorMessage
            }
        }
    };

    function onSubmitForm(state) {

        dispatch({ type: "SET_IS_LOADING", payload: true });

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" },
            body: JSON.stringify(state)
        };

        fetch(props.saveUrl, requestOptions)
            .then(function (response) {

                dispatch({ type: "SET_IS_LOADING", payload: false });

                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {

                    if (response.ok) {
                        setDisableSaveButton(true);
                        setFieldValue({ name: "id", value: jsonResponse.id });
                        toast.success(jsonResponse.message);
                    }
                    else {
                        toast.error(props.generalErrorMessage);
                    }
                });
            }).catch(() => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(props.generalErrorMessage);
            });
    }

    const {
        values,
        errors,
        dirty,
        disable,
        setFieldValue,
        handleOnChange,
        handleOnSubmit
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm, !props.id);

    const { id, name, productAttributeId } = values;

    return (
        <section className="section section-small-padding product-attribute-item">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div className="columns is-desktop">
                <div className="column is-half">
                    <form className="is-modern-form" onSubmit={handleOnSubmit} method="post">
                        {id &&
                            <div className="field">
                                <InputLabel id="id-label">{props.idLabel} {id}</InputLabel>
                            </div>
                        }
                        <input id="productAttributeId" name="productAttributeId" type="hidden" value={productAttributeId} />
                        <div className="field">
                            <TextField id="name" name="name" label={props.nameLabel} fullWidth={true}
                                value={name} onChange={handleOnChange} helperText={dirty.name ? errors.name : ""} error={(errors.name.length > 0) && dirty.name} />
                        </div>
                        <div className="field">
                            <Button 
                                type="submit" 
                                variant="contained" 
                                color="primary" 
                                disabled={state.isLoading || disable || disableSaveButton}>
                                {props.saveText}
                            </Button>
                            <Button
                                className="ml-2"
                                type="button"
                                variant="contained" 
                                color="secondary"
                                onClick={(e) => {
                                    e.preventDefault();
                                    NavigationHelper.redirect(props.productAttributeUrl);
                                }}>
                                {props.navigateToProductAttributesLabel}
                            </Button>
                        </div>
                    </form>
                </div>
            </div>
            {state.isLoading && <CircularProgress className="progressBar" />}
        </section>
    );
}

ProductAttributeItemForm.propTypes = {
    id: PropTypes.string,
    name: PropTypes.string,
    nameLabel: PropTypes.string.isRequired,
    nameRequiredErrorMessage: PropTypes.string.isRequired,
    saveText: PropTypes.string.isRequired,
    generalErrorMessage: PropTypes.string.isRequired,
    idLabel: PropTypes.string
};

export default ProductAttributeItemForm;
