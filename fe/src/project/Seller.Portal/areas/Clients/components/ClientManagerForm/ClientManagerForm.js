import React, { useContext } from "react"
import { toast } from "react-toastify";
import { Context } from "../../../../../../shared/stores/Store";
import PropTypes from "prop-types";
import NavigationHelper from "../../../../../../shared/helpers/globals/NavigationHelper";
import AuthenticationHelper from "../../../../../../shared/helpers/globals/AuthenticationHelper";
import useForm from "../../../../../../shared/helpers/forms/useForm";
import { 
    TextField, Button, InputLabel, CircularProgress, FormControl, Select, MenuItem
} from "@mui/material";

const ClientManagerForm = (props) => {
    const [state, dispatch] = useContext(Context);
    const stateSchema = {
        id: { value: props.id ? props.id : null, error: "" },
        clientIds: { value: props.clientIds ? props.clientIds : []},
    };

    const stateValidatorSchema = {
        clientIds: {
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

    const { id, clientIds } = values;
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
                            <FormControl fullWidth={true} variant="standard" error={(errors.clientIds && errors.clientIds.length > 0) && dirty.clientIds}>
                                <InputLabel id="clients-label">{props.clientsLabel}</InputLabel>
                                <Select
                                    labelId="clients-label"
                                    id="clientIds"
                                    name="clientIds"
                                    value={clientIds}
                                    multiple={true}
                                    onChange={handleOnChange}>
                                    {props.clients && props.clients.length > 0 ? (
                                        props.clients.map((client, index) => {
                                            return (
                                                <MenuItem key={index} value={client.id}>{client.name}</MenuItem>
                                            );
                                        })
                                    ) : (
                                        <MenuItem disabled>{props.noClientsText}</MenuItem>
                                    )}
                                </Select>
                                {errors.clientIds && dirty.clientIds && (
                                    <FormHelperText>{errors.clientIds}</FormHelperText>
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
                            <Button
                                className="ml-2"
                                type="button" 
                                variant="contained" 
                                color="secondary" 
                                onClick={(e) => {
                                    e.preventDefault();
                                    NavigationHelper.redirect(props.managersUrl);
                                }}>
                                {props.navigateToManagersText}
                            </Button>
                        </div>
                    </form>
                </div>
            </div>
            {state.isLoading && <CircularProgress className="progressBar" />}
        </section>
    )
}

ClientManagerForm.propTypes = {
    title: PropTypes.string.isRequired,
    idLabel: PropTypes.string.isRequired,
    id: PropTypes.string,
    clientsLabel: PropTypes.string.isRequired,
    noClientsText: PropTypes.string,
    clients: PropTypes.array,
    navigateToManagersText: PropTypes.string.isRequired,
    saveText: PropTypes.string.isRequired,
    fieldRequiredErrorMessage: PropTypes.string.isRequired,
    saveUrl: PropTypes.string.isRequired,
    clientIds: PropTypes.array,
    managersUrl: PropTypes.string.isRequired,
    generalErrorMessage: PropTypes.string.isRequired
}

export default ClientManagerForm;