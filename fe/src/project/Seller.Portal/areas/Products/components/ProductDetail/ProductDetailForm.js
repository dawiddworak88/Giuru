import React, { useContext, useState } from "react";
import { toast } from "react-toastify";
import PropTypes from "prop-types";
import Autocomplete from "@material-ui/lab/Autocomplete";
import { Context } from "../../../../../../shared/stores/Store";
import useForm from "../../../../../../shared/helpers/forms/useForm";
import { TextField, Button, CircularProgress } from "@material-ui/core";
import DynamicForm from "../../../../../../shared/components/DynamicForm/DynamicForm";

function ProductDetailForm(props) {

    const defaultProps = {
        options: props.categories,
        getOptionLabel: (option) => option.name
    };

    const [state, dispatch] = useContext(Context);
    
    const stateSchema = {

        id: { value: props.id ? props.id : null, error: "" },
        name: { value: props.name ? props.name : "", error: "" },
        sku: { value: props.sku ? props.sku : "", error: "" },
        primaryProductId: { value: props.primaryProductId ? props.primaryProductId : "" },
        images: { value: props.images ? props.images : [] },
        files: { value: props.files ? props.files : [] }
    };

    const stateValidatorSchema = {

        sku: {
            required: {
                isRequired: true,
                error: props.skuRequiredErrorMessage
            }
        },
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
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(state)
        };

        fetch(props.saveUrl, requestOptions)
            .then(function (response) {

                dispatch({ type: "SET_IS_LOADING", payload: false });

                return response.json().then(jsonResponse => {

                    if (response.ok) {

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

    const { id, name, sku, primaryProductId, images, files } = values;

    return (
        <div>
            <form className="is-modern-form" onSubmit={handleOnSubmit} method="post">
                <input type="hidden" value={schemaId} />
                <div className="field">
                    <Autocomplete
                        {...defaultProps}
                        id="category"
                        name="category"
                        fullWidth={true}
                        value={category}
                        onChange={(event, newValue) => {
                        }}
                        autoComplete
                        renderInput={(params) => <TextField {...params} label={props.selectCategoryLabel} margin="normal" />}
                    />
                </div>
                <div className="field">
                    <TextField id="sku" name="sku" label={props.skuLabel} fullWidth={true}
                        value={sku} onChange={handleOnChange} helperText={dirty.sku ? errors.sku : ""} error={(errors.sku.length > 0) && dirty.sku} />
                </div>
                <div className="field">
                    <TextField id="name" name="name" label={props.nameLabel} fullWidth={true}
                        value={name} onChange={handleOnChange} helperText={dirty.name ? errors.name : ""} error={(errors.name.length > 0) && dirty.name} />
                </div>
                {jsonSchema &&
                    <DynamicForm jsonSchema={jsonSchema} uiSchema={uiSchema} formData={formData} onChange={handleOnChange} />
                }
                <div className="field">
                    <Button type="submit" variant="contained" color="primary" disabled={state.isLoading || disable}>
                        {props.saveText}
                    </Button>
                </div>
            </form>
            {state.isLoading && <CircularProgress className="progressBar" />}
        </div>
    );
}

ProductDetailForm.propTypes = {
    skuLabel: PropTypes.string.isRequired,
    nameLabel: PropTypes.string.isRequired,
    saveText: PropTypes.string.isRequired,
    categories: PropTypes.array.isRequired,
    generalErrorMessage: PropTypes.string.isRequired
};

export default ProductDetailForm;
