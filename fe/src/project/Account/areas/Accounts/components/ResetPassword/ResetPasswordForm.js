import React, { useContext } from "react";
import PropTypes from "prop-types";
import { Context } from "../../../../../../shared/stores/Store";
import { TextField, Button, CircularProgress } from "@material-ui/core";
import EmailValidator from "../../../../../../shared/helpers/validators/EmailValidator";
import useForm from "../../../../../../shared/helpers/forms/useForm";

const ResetPasswordForm = (props) => {
    const [state, dispatch] = useContext(Context);
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
                            helperText={dirty.email ? errors.email : ""} 
                            error={(errors.email.length > 0) && dirty.email} />
                    </div>
                    <div className="field">
                        <Button type="submit" variant="contained" color="primary" disabled={state.isLoading || disable} fullWidth={true}>
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
    emailRequiredErrorMessage: PropTypes.string.isRequired
}

export default ResetPasswordForm;