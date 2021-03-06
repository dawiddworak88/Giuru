import React, { useContext } from "react";
import PropTypes from "prop-types";
import { toast } from "react-toastify";
import { Context } from "../../../../../../shared/stores/Store";
import useForm from "../../../../../../shared/helpers/forms/useForm";
import { TextField, Button, CircularProgress } from "@material-ui/core";

function ProductAttributeForm(props) {

    const [state, dispatch] = useContext(Context);

    const stateSchema = {

        id: { value: props.id ? props.id : null, error: "" },
        key: { value: props.name ? props.name : "", error: "" },
        name: { value: props.name ? props.name : "", error: "" }
    };

    const stateValidatorSchema = {

        key: {
            required: {
                isRequired: true,
                error: props.keyRequiredErrorMessage
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

    const { id, key, name } = values;

    return (
        <section className="section section-small-padding product-attribute">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div className="columns is-desktop">
                <div className="column is-half">
                    <form className="is-modern-form" onSubmit={handleOnSubmit} method="post">
                        {id &&
                            <input id="id" name="id" type="hidden" value={id} />
                        }
                        <div className="field">
                            <TextField id="name" name="name" label={props.nameLabel} fullWidth={true}
                                value={name} onChange={handleOnChange} helperText={dirty.name ? errors.name : ""} error={(errors.name.length > 0) && dirty.name} />
                        </div>
                        <div className="field">
                            <TextField id="key" name="key" label={props.keyLabel} fullWidth={true}
                                value={key} onChange={handleOnChange} helperText={dirty.key ? errors.key : ""} error={(errors.key.length > 0) && dirty.key} />
                        </div>
                        <div className="field">
                            <Button type="submit" variant="contained" color="primary" disabled={state.isLoading || disable}>
                                {props.saveText}
                            </Button>
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
    key: PropTypes.string,
    keyLabel: PropTypes.string.isRequired,
    nameLabel: PropTypes.string.isRequired,
    keyRequiredErrorMessage: PropTypes.string.isRequired,
    nameRequiredErrorMessage: PropTypes.string.isRequired,
    saveText: PropTypes.string.isRequired,
    editLabel: PropTypes.string.isRequired,
    deleteLabel: PropTypes.string.isRequired,
    generalErrorMessage: PropTypes.string.isRequired
};

export default ProductAttributeForm;
