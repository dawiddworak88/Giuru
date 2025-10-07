import React, { useContext } from "react"
import { toast } from "react-toastify";
import { Context } from "../../../../shared/stores/Store";
import PropTypes from "prop-types";
import AuthenticationHelper from "../../../../shared/helpers/globals/AuthenticationHelper";
import EmailValidator from "../../../../shared/helpers/validators/EmailValidator";
import useForm from "../../../../shared/helpers/forms/useForm";
import { 
    TextField, Button, InputLabel, CircularProgress
} from "@mui/material";

const ClientAccountManagerForm = (props) => {
    const [state, dispatch] = useContext(Context);
    const stateSchema = {
        id: { value: props.id ? props.id : null },
        firstName: { value: props.firstName ? props.firstName : "", error: "" },
        lastName: { value: props.lastName ? props.lastName : "", error: "" },
        email: { value: props.email ? props.email : "", error: "" },
        phoneNumber: { value: props.phoneNumber ? props.phoneNumber : "" }
    };

    const stateValidatorSchema = {
        firstName: {
            required: {
                isRequired: true,
                error: props.fieldRequiredErrorMessage
            }
        },
        lastName: {
            required: {
                isRequired: true,
                error: props.fieldRequiredErrorMessage
            }
        },
        email: {
            required: {
                isRequired: true,
                error: props.fieldRequiredErrorMessage
            },
            validator: {
                func: value => EmailValidator.validateFormat(value),
                error: props.emailFormatErrorMessage
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

    const { id, firstName, lastName, email, phoneNumber } = values;
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
                                id="firstName" 
                                name="firstName"
                                value={firstName}
                                fullWidth={true}
                                variant="standard"
                                label={props.firstNameLabel}
                                onChange={handleOnChange} 
                                helperText={dirty.firstName ? errors.firstName : ""} 
                                error={(errors.firstName.length > 0) && dirty.firstName} />
                        </div>
                        <div className="field">
                            <TextField
                                id="lastName" 
                                name="lastName"
                                value={lastName}
                                fullWidth={true}
                                variant="standard"
                                label={props.lastNameLabel}
                                onChange={handleOnChange} 
                                helperText={dirty.lastName ? errors.lastName : ""} 
                                error={(errors.lastName.length > 0) && dirty.lastName} />
                        </div>
                        <div className="field">
                            <TextField
                                id="email" 
                                name="email"
                                value={email}
                                fullWidth={true}
                                variant="standard"
                                label={props.emailLabel}
                                onChange={handleOnChange} 
                                helperText={dirty.email ? errors.email : ""} 
                                error={(errors.email.length > 0) && dirty.email} />
                        </div>
                        <div className="field">
                            <TextField
                                id="phoneNumber" 
                                name="phoneNumber"
                                value={phoneNumber}
                                fullWidth={true}
                                variant="standard"
                                label={props.phoneNumberLabel}
                                onChange={handleOnChange} />
                        </div>
                        <div className="field">
                            <Button 
                                type="submit" 
                                variant="contained" 
                                color="primary"
                                disabled={state.isLoading || disable}>
                                {props.saveText}
                            </Button>
                            <a href={props.managersUrl} className="ml-2 button is-text">{props.navigateToManagersText}</a>
                        </div>
                    </form>
                </div>
            </div>
            {state.isLoading && <CircularProgress className="progressBar" />}
        </section>
    )
}

ClientAccountManagerForm.propTypes = {
    title: PropTypes.string.isRequired,
    idLabel: PropTypes.string.isRequired,
    id: PropTypes.string,
    navigateToManagersText: PropTypes.string.isRequired,
    saveText: PropTypes.string.isRequired,
    fieldRequiredErrorMessage: PropTypes.string.isRequired,
    saveUrl: PropTypes.string.isRequired,
    managersUrl: PropTypes.string.isRequired,
    generalErrorMessage: PropTypes.string.isRequired,
    emailFormatErrorMessage: PropTypes.string.isRequired,
    firstNameLabel: PropTypes.string.isRequired,
    lastNameLabel: PropTypes.string.isRequired,
    emailLabel: PropTypes.string.isRequired,
    phoneNumberLabel: PropTypes.string.isRequired,
    firstName: PropTypes.string,
    lastName: PropTypes.string,
    email: PropTypes.string,
    phoneNumber: PropTypes.string
}

export default ClientAccountManagerForm;