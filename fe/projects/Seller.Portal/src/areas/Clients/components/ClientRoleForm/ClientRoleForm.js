import React, { useContext, useState } from "react";
import { Context } from "../../../../shared/stores/Store";
import { toast } from "react-toastify";
import useForm from "../../../../shared/helpers/forms/useForm";
import AuthenticationHelper from "../../../../shared/helpers/globals/AuthenticationHelper";
import { 
    TextField, Button, CircularProgress, InputLabel
} from "@mui/material";
import PropTypes from "prop-types";

const ClientRoleForm = (props) => {
    const [state, dispatch] = useContext(Context);
    const stateSchema = {
        id: { value: props.id ? props.id : null },
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

    const onSubmitForm = (state) => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" },
            body: JSON.stringify(state)
        }

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

    const { id, name } = values;
    return (
        <section className="section section-small-padding">
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
                            <TextField 
                                id="name" 
                                name="name" 
                                label={props.nameLabel} 
                                fullWidth={true}
                                value={name} 
                                variant="standard"
                                onChange={handleOnChange} 
                                helperText={dirty.name ? errors.name : ""} 
                                error={(errors.name.length > 0) && dirty.name} />
                        </div>
                        <div className="field">
                            <Button 
                                type="submit" 
                                variant="contained" 
                                color="primary" 
                                disabled={state.isLoading || disable}>
                                {props.saveText}
                            </Button>
                            <a href={props.rolesUrl} className="ml-2 button is-text">{props.navigateToCliensRoles}</a>
                        </div>
                    </form>
                    {state.isLoading && <CircularProgress className="progressBar" />}
                </div>
            </div>
        </section>
    )
}

ClientRoleForm.propTypes = {
    title: PropTypes.string.isRequired,
    idLabel: PropTypes.string,
    nameLabel: PropTypes.string.isRequired,
    name: PropTypes.string,
    id: PropTypes.string,
    nameRequiredErrorMessage: PropTypes.string.isRequired,
    saveUrl: PropTypes.string.isRequired,
    saveText: PropTypes.string.isRequired,
    navigateToCliensRoles: PropTypes.string.isRequired,
    rolesUrl: PropTypes.string.isRequired
}

export default ClientRoleForm;