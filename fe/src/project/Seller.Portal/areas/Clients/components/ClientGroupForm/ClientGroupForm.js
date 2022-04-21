import React, { useState, useContext } from "react";
import useForm from "../../../../../../shared/helpers/forms/useForm";
import { TextField, Button, InputLabel, CircularProgress } from "@material-ui/core";
import NavigationHelper from "../../../../../../shared/helpers/globals/NavigationHelper";
import AuthenticationHelper from "../../../../../../shared/helpers/globals/AuthenticationHelper";
import { Context } from "../../../../../../shared/stores/Store";
import { toast } from "react-toastify";
import PropTypes from "prop-types";

const ClientGroupForm = (props) => {
    const [state, dispatch] = useContext(Context);
    const [disableSaveButton, setDisableSaveButton] = useState(false);
    const stateSchema = {
        id: { value: props.id ? props.id : null, error: "" },
        name: { value: props.name ? props.name : null, error: "" }
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
        values, errors, dirty, disable,
        setFieldValue, handleOnChange, handleOnSubmit
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm, !props.id);

    const { id, name } = values;
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
                                helperText={dirty.name ? errors.name : ""} 
                                error={(errors.name.length > 0) && dirty.name} />
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
                                    NavigationHelper.redirect(props.groupsUrl);
                                }}>
                                {props.navigateToGroupsText}
                            </Button>
                        </div>
                    </form>
                </div>
            </div>
            {state.isLoading && <CircularProgress className="progressBar" />}
        </section>
    )
}

ClientGroupForm.propTypes = {
    title: PropTypes.string.isRequired,
    idLabel: PropTypes.string.isRequired,
    nameLabel: PropTypes.string.isRequired,
    groupsUrl: PropTypes.string.isRequired,
    navigateToGroupsText: PropTypes.string.isRequired,
    saveText: PropTypes.string.isRequired,
    saveUrl: PropTypes.string.isRequired,
    generalErrorMessage: PropTypes.string.isRequired,
    nameRequiredErrorMessage: PropTypes.string.isRequired
}

export default ClientGroupForm;