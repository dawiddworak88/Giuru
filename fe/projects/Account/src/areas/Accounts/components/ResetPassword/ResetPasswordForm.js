import React, { useContext, useState } from "react";
import PropTypes from "prop-types";
import { toast } from "react-toastify";
import { Context } from "../../../../shared/stores/Store";
import { TextField, Button, CircularProgress } from "@mui/material";
import EmailValidator from "../../../../shared/helpers/validators/EmailValidator";
import useForm from "../../../../shared/helpers/forms/useForm";
import AuthenticationHelper from "../../../../shared/helpers/globals/AuthenticationHelper";

const ResetPasswordForm = (props) => {
    const [state, dispatch] = useContext(Context);
    const [disableSaveButton, setDisableSaveButton] = useState(false);
    const stateSchema = {
        email: { value: null, error: "" }
    };

    const stateValidatorSchema = {
        email: {
            required: {
                isRequired: true,
                error: props.emailRequiredErrorMessage
            },
            validator: {
                func: value => EmailValidator.validateFormat(value),
                error: props.emailFormatErrorMessage,
            },
        }
    };

    const onSubmitForm = (state) => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" },
            body: JSON.stringify(state)
        };

        fetch(props.submitUrl, requestOptions)
            .then(response => {
                dispatch({ type: "SET_IS_LOADING", payload: false });

                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {
                    if (response.ok) {
                        setDisableSaveButton(true);
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
        disable, values, errors, dirty, handleOnChange, handleOnSubmit
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm);

    const { email } = values;
    return (
        <section className="section is-flex-centered set-password">
            <div className="account-card">
                <form className="is-modern-form has-text-centered" onSubmit={handleOnSubmit}>
                    <div>
                        <h1 className="subtitle is-4">{props.resetPasswordText}</h1>
                    </div>
                    <div className="field">
                        <TextField 
                            name="email" 
                            type="email"
                            label={props.emailLabel} 
                            fullWidth={true} 
                            value={email} 
                            onChange={handleOnChange} 
                            variant="standard"
                            helperText={dirty.email ? errors.email : ""} 
                            error={(errors.email.length > 0) && dirty.email} />
                    </div>
                    <div className="field">
                        <Button 
                            type="submit" 
                            variant="contained" 
                            color="primary" 
                            disabled={state.isLoading || disable || disableSaveButton} 
                            fullWidth={true}>
                            {props.resetPasswordText}
                        </Button>
                    </div>
                </form>
            </div>
            {state.isLoading && <CircularProgress className="progressBar" />}
        </section>
    )
}

ResetPasswordForm.propTypes = {
    resetPasswordText: PropTypes.string.isRequired,
    emailLabel: PropTypes.string.isRequired,
    emailFormatErrorMessage: PropTypes.string.isRequired,
    emailRequiredErrorMessage: PropTypes.string.isRequired,
    submitUrl: PropTypes.string.isRequired,
    navigateToSignInLabel: PropTypes.string,
    signInUrl: PropTypes.string
}

export default ResetPasswordForm;