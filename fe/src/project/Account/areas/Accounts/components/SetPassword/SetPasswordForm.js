import React, { useContext } from "react";
import PropTypes from "prop-types";
import { Context } from "../../../../../../shared/stores/Store";
import { TextField, Button, CircularProgress } from "@material-ui/core";
import useForm from "../../../../../../shared/helpers/forms/useForm";
import PasswordValidator from "../../../../../../shared/helpers/validators/PasswordValidator";
import { toast } from "react-toastify";

function SetPasswordForm(props) {

    const [state, dispatch] = useContext(Context);
    const stateSchema = {
        id: {value: props.id ? props.id : null, error: ""},
        password: { value: null, error: "" },
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
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(state)
        };

        fetch(props.submitUrl, requestOptions)
            .then(async (response) => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                const jsonResponse = await response.json();
                if (response.ok) {
                    setFieldValue({ name: "id", value: jsonResponse.id });
                    toast.success(jsonResponse.message);
                } else {
                    toast.error(props.generalErrorMessage);
                }
            }).catch(() => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(props.generalErrorMessage);
            });
    };

    const {
        values, errors, dirty,
        disable, setFieldValue, 
        handleOnChange, handleOnSubmit
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm, !props.id);

    const { password } = values;
    return (
        <section className="section is-flex-centered">
            <div className="account-card">
                <form className="is-modern-form has-text-centered" onSubmit={handleOnSubmit} method="post">
                    <input type="hidden" name="id" value={props.id} />
                    <div>
                        <h1 className="subtitle is-4">{props.setPasswordText}</h1>
                    </div>
                    <div className="field">
                        <TextField 
                            id="password" 
                            name="password" 
                            type="password"
                            label={props.passwordLabel} 
                            fullWidth={true} 
                            value={password} 
                            onChange={handleOnChange} 
                            helperText={dirty.password ? errors.password : ""} 
                            error={(errors.password.length > 0) && dirty.password} />
                    </div>
                    <div className="field">
                        <Button type="submit" variant="contained" color="primary" disabled={state.isLoading || disable} fullWidth={true}>
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
    emailRequiredErrorMessage: PropTypes.string.isRequired,
    passwordRequiredErrorMessage: PropTypes.string.isRequired,
    emailFormatErrorMessage: PropTypes.string.isRequired,
    passwordFormatErrorMessage: PropTypes.string.isRequired,
    signInText: PropTypes.string.isRequired,
    enterEmailText: PropTypes.string.isRequired,
    enterPasswordText: PropTypes.string.isRequired,
    submitUrl: PropTypes.string.isRequired,
};

export default SetPasswordForm;
