import React, { useContext } from "react";
import PropTypes from "prop-types";
import { Context } from "../../../../shared/stores/Store";
import { TextField, Button, CircularProgress } from "@mui/material";
import useForm from "../../../../shared/helpers/forms/useForm";
import PasswordValidator from "../../../../shared/helpers/validators/PasswordValidator";
import { toast } from "react-toastify";
import ResponseStatusConstants from "../../../../shared/constants/ResponseStatusConstants";
import NavigationHelper from "../../../../shared/helpers/globals/NavigationHelper";
import ToastHelper from "../../../../shared/helpers/globals/ToastHelper";

function SetPasswordForm(props) {
    const [state, dispatch] = useContext(Context);
    const stateSchema = {
        id: { value: props.id ? props.id : null, error: "" },
        password: { value: null, error: "" },
        returnUrl: { value: props.returnUrl ? props.returnUrl : null }
    };

    const stateValidatorSchema = {
        password: {
            required: {
                isRequired: true,
                error: props.passwordRequiredErrorMessage
            },
            validator: {
                func: value => PasswordValidator.validateFormat(value),
                error: props.passwordFormatErrorMessage,
            },
        },
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
                return response.json().then(jsonResponse => {
                    if (response.status === ResponseStatusConstants.found()) {
                        toast.success(props.passwordSetSuccessMessage);
                        setTimeout(() => {
                            NavigationHelper.redirect(jsonResponse.url)
                        }, 2000);
                    } else {
                        dispatch({ type: "SET_IS_LOADING", payload: false });
                        toast.error(ToastHelper.withLink(jsonResponse.emailIsConfirmedLabel, jsonResponse.signInUrl, jsonResponse.signInLabel))
                    }
                })
            }).catch(() => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(props.generalErrorMessage);
            });
    };

    const {
        disable, values, errors, dirty, handleOnChange, handleOnSubmit
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm);

    const { id, password } = values;
    return (
        <section className="section is-flex-centered set-password">
            <div className="account-card">
                <form className="is-modern-form has-text-centered" onSubmit={handleOnSubmit} method="post">
                    <input type="hidden" name="id" value={id} />
                    <div>
                        <h1 className="subtitle is-4">{props.setPasswordText}</h1>
                    </div>
                    <div className="field">
                        <TextField 
                            id="password" 
                            name="password" 
                            type="password"
                            variant="standard"
                            label={props.passwordLabel} 
                            fullWidth={true} 
                            value={password} 
                            onChange={handleOnChange} 
                            helperText={dirty.password ? errors.password : ""} 
                            error={(errors.password.length > 0) && dirty.password} />
                    </div>
                    <div className="field">
                        <Button 
                            type="submit" 
                            variant="contained" 
                            color="primary" 
                            disabled={state.isLoading || disable} 
                            fullWidth={true}>
                            {props.setPasswordText}
                        </Button>
                    </div>
                </form>
            </div>
            {state.isLoading && <CircularProgress className="progressBar" />}
        </section>
    );
}

SetPasswordForm.propTypes = {
    generalErrorMessage: PropTypes.string.isRequired,
    passwordSetSuccessMessage: PropTypes.string.isRequired,
    passwordRequiredErrorMessage: PropTypes.string.isRequired,
    passwordFormatErrorMessage: PropTypes.string.isRequired,
    returnUrl: PropTypes.string.isRequired,
    submitUrl: PropTypes.string.isRequired,
};

export default SetPasswordForm;
