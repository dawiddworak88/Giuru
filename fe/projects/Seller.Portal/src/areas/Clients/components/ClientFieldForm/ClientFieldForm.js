import React, { useContext } from "react";
import useForm from "../../../../shared/helpers/forms/useForm";
import { 
    TextField, Button, InputLabel, CircularProgress, 
    FormControl, Select, FormHelperText, MenuItem
} from "@mui/material";
import AuthenticationHelper from "../../../../shared/helpers/globals/AuthenticationHelper";
import { Context } from "../../../../shared/stores/Store";
import { toast } from "react-toastify";
import PropTypes from "prop-types";
import NavigationHelper from "../../../../shared/helpers/globals/NavigationHelper";

const ClientFieldForm = (props) => {
    const [state, dispatch] = useContext(Context);
    const stateSchema = {
        id: { value: props.id ? props.id : null, error: "" },
        name: { value: props.name ? props.name : null, error: "" },
        type: { value: props.types ? props.types.find(type => type.value == props.type) : null, error: ""}
    };

    const stateValidatorSchema = {
        name: {
            required: {
                isRequired: true,
                error: props.fieldRequiredErrorMessage
            }
        },
        type: {
            required: {
                isRequired: true,
                error: props.fieldRequiredErrorMessage
            }
        }
    };

    const onSubmitForm = (state) => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        const requestPayload = {
            id: state.id,
            name: state.name,
            type: state.type ? state.type.value : null
        }
        
        const requestOptions = {
            method: "POST",
            headers: { 
                "Content-Type": "application/json",
                "X-Requested-With": "XMLHttpRequest"
            },
            body: JSON.stringify(requestPayload)
        };

        fetch(props.saveUrl, requestOptions)
            .then((response) => {
                dispatch({ type: "SET_IS_LOADING", payload: false });

                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {
                    if (response.ok) {
                        setFieldValue({ name: "id", value: jsonResponse.id });
                        toast.success(jsonResponse.message);

                        if (type.value == "select" || props.type == "select") {
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
        values, errors, dirty, disable,
        setFieldValue, handleOnChange, handleOnSubmit
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm, !props.id);

    const { id, name, type } = values;

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
                            <FormControl fullWidth={true} variant="standard" error={(errors.type.length > 0) && dirty.type}>
                                <InputLabel id="type-label">{props.typeLabel}</InputLabel>
                                <Select
                                    labelId="type-label"
                                    id="type"
                                    name="type"
                                    value={type}
                                    onChange={handleOnChange}>
                                    {props.types && props.types.map((type, index) => {
                                        return (
                                            <MenuItem key={index} value={type}>{type.text}</MenuItem>
                                        )
                                    })}
                                </Select>
                                {errors.type && dirty.type && (
                                    <FormHelperText>{errors.type}</FormHelperText>
                                )}
                            </FormControl>
                        </div>
                        <div className="field">
                            <Button 
                                type="submit" 
                                variant="contained" 
                                color="primary"
                                disabled={state.isLoading || disable}>
                                {props.saveText}
                            </Button>
                            <a href={props.clientFieldsUrl} className="ml-2 button is-text">{props.navigateToFieldsText}</a>
                        </div>
                    </form>
                </div>
            </div>
            {state.isLoading && <CircularProgress className="progressBar" />}
        </section>
    )
}

ClientFieldForm.propTypes = {
    title: PropTypes.string.isRequired,
    idLabel: PropTypes.string.isRequired,
    nameLabel: PropTypes.string.isRequired,
    typeLabel: PropTypes.string.isRequired, 
    clientFieldsUrl: PropTypes.string.isRequired,
    navigateToFieldsText: PropTypes.string.isRequired,
    saveText: PropTypes.string.isRequired,
    saveUrl: PropTypes.string.isRequired,
    editUrl: PropTypes.string.isRequired,
    generalErrorMessage: PropTypes.string.isRequired,
    fieldRequiredErrorMessage: PropTypes.string.isRequired
}

export default ClientFieldForm;