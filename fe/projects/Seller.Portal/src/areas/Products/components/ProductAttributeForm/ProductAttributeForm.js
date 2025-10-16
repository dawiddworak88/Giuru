import React, { useContext } from "react";
import PropTypes from "prop-types";
import { toast } from "react-toastify";
import { Context } from "../../../../shared/stores/Store";
import useForm from "../../../../shared/helpers/forms/useForm";
import { TextField, Button, CircularProgress, InputLabel } from "@mui/material";
import NavigationHelper from "../../../../shared/helpers/globals/NavigationHelper";
import AuthenticationHelper from "../../../../shared/helpers/globals/AuthenticationHelper";

function ProductAttributeForm(props) {
    const [state, dispatch] = useContext(Context);
    const stateSchema = {
        id: { value: props.id ? props.id : null, error: "" },
        name: { value: props.name ? props.name : "", error: "" }
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
                        toast.success(jsonResponse.message);

                        if (!id) {

                            NavigationHelper.redirect(props.editUrl + "/" + jsonResponse.id);
                        }
                    }
                    else {
                        toast.error(jsonResponse?.message || props.generalErrorMessage);
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
        handleOnChange,
        handleOnSubmit
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm, !props.id);

    const { id, name } = values;

    return (
        <section className="section section-small-padding product-attribute">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div className="columns is-desktop">
                <div className="column is-half">
                    <form className="is-modern-form" onSubmit={handleOnSubmit} method="post">
                        {id &&
                            <div className="field">
                                <InputLabel id="id-label">{props.idLabel} {id}</InputLabel>
                            </div>
                        }
                        <div className="field">
                            <TextField id="name" name="name" label={props.nameLabel} fullWidth={true} variant="standard"
                                value={name} onChange={handleOnChange} helperText={dirty.name ? errors.name : ""} error={(errors.name.length > 0) && dirty.name} />
                        </div>
                        <div className="field">
                            <Button 
                                type="submit" 
                                variant="contained" 
                                color="primary" 
                                disabled={state.isLoading || disable}>
                                {props.saveText}
                            </Button>
                            <a href={props.attributesUrl} className="ml-2 button is-text">{props.navigateToAttributesLabel}</a>
                        </div>
                    </form>
                </div>
            </div>
            {state.isLoading && <CircularProgress className="progressBar" />}
        </section>
    );
}

ProductAttributeForm.propTypes = {
    id: PropTypes.string,
    name: PropTypes.string,
    nameLabel: PropTypes.string.isRequired,
    nameRequiredErrorMessage: PropTypes.string.isRequired,
    editUrl: PropTypes.string.isRequired,
    saveText: PropTypes.string.isRequired,
    generalErrorMessage: PropTypes.string.isRequired,
    idLabel: PropTypes.string
};

export default ProductAttributeForm;
