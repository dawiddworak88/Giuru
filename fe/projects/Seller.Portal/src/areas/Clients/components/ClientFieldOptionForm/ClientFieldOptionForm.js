import React, { useContext } from "react";
import useForm from "../../../../shared/helpers/forms/useForm";
import { 
    TextField, Button, InputLabel, CircularProgress
} from "@mui/material";
import AuthenticationHelper from "../../../../shared/helpers/globals/AuthenticationHelper";
import { Context } from "../../../../shared/stores/Store";
import { toast } from "react-toastify";
import PropTypes from "prop-types";

const ClientFieldOptionForm = (props) => {
    const [state, dispatch] = useContext(Context);
    const stateSchema = {
        id: { value: props.id ? props.id : null, error: "" },
        name: { value: props.name ? props.name : null, error: "" },
        value: { value: props.value ? props.value : null, error: "" },
    };

    const stateValidatorSchema = {
        name: {
            required: {
                isRequired: true,
                error: props.fieldRequiredErrorMessage
            }
        },
        value: {
            required: {
                isRequired: true,
                error: props.fieldRequiredErrorMessage
            }
        }
    };

    const onSubmitForm = (state) => {
        dispatch({ type: "SET_IS_LOADING", payload: true });
        
        const requestOptions = {
            method: "POST",
            headers: { 
                "Content-Type": "application/json",
                "X-Requested-With": "XMLHttpRequest"
            },
            body: JSON.stringify(state)
        };

        fetch(props.saveUrl, requestOptions)
            .then((response) => {
                dispatch({ type: "SET_IS_LOADING", payload: false });

                AuthenticationHelper.HandleResponse(response);

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
        values, errors, dirty, disable,
        setFieldValue, handleOnChange, handleOnSubmit
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm, !props.id);

    const { id, name, value } = values;

    return (
        <section className="section section-small-padding">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div className="columns is-desktop">
                <div className="column is-half">
                    <form className="is-modern-form" onSubmit={handleOnSubmit}>
                        {id &&
                            <div className="field">
                                <InputLabel id="id-label">{props.idLabel} {id}</InputLabel>
                            </div>
                        }
                        <div className="field">
                            <TextField 
                                id="name" 
                                name="name" 
                                label={props.nameLabel} 
                                fullWidth={true}
                                value={name} 
                                onChange={handleOnChange} 
                                variant="standard"
                                helperText={dirty.name ? errors.name : ""} 
                                error={(errors.name.length > 0) && dirty.name} />
                        </div>
                        <div className="field">
                            <TextField 
                                id="value" 
                                name="value" 
                                label={props.valueLabel} 
                                fullWidth={true}
                                value={value} 
                                onChange={handleOnChange} 
                                variant="standard"
                                helperText={dirty.value ? errors.value : ""} 
                                error={(errors.value.length > 0) && dirty.value} />
                        </div>
                        <div className="field">
                            <Button 
                                type="submit" 
                                variant="contained" 
                                color="primary"
                                disabled={state.isLoading || disable}>
                                {props.saveText}
                            </Button>
                            <a href={props.clientFieldUrl} className="ml-2 button is-text">{props.navigateToFieldText}</a>
                        </div>
                    </form>
                </div>
            </div>
            {state.isLoading && <CircularProgress className="progressBar" />}
        </section>
    )
}
ClientFieldOptionForm.propTypes = {
    title: PropTypes.string.isRequired,
    idLabel: PropTypes.string.isRequired,
    nameLabel: PropTypes.string.isRequired,
    valueLabel: PropTypes.string.isRequired, 
    clientFieldUrl: PropTypes.string.isRequired,
    navigateToFieldsText: PropTypes.string.isRequired,
    saveText: PropTypes.string.isRequired,
    saveUrl: PropTypes.string.isRequired,
    generalErrorMessage: PropTypes.string.isRequired,
    fieldRequiredErrorMessage: PropTypes.string.isRequired
}

export default ClientFieldOptionForm;