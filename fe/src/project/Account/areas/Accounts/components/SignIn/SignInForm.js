import React from "react";
import PropTypes from "prop-types";
import { TextField, Button } from "@material-ui/core";
import useForm from "../../../../../../shared/helpers/forms/useForm";
import EmailValidator from "../../../../../../shared/helpers/validators/EmailValidator";
import PasswordValidator from "../../../../../../shared/helpers/validators/PasswordValidator";
import NavigationHelper from "../../../../../../shared/helpers/globals/NavigationHelper";

function SignInForm(props) {

    const stateSchema = {
        email: { value: "", error: "" },
        password: { value: "", error: "" }
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
        },
        password: {
            required: {
                isRequired: true,
                error: props.passwordRequiredErrorMessage
            },
            validator: {
                func: value => PasswordValidator.validateFormat(value),
                error: props.passwordFormatErrorMessage,
            },
        }
    };

    const {
        values, errors, dirty,
        disable, handleOnChange
    } = useForm(stateSchema, stateValidatorSchema);

    const { email, password } = values;
    return (
        <section className="section is-flex-centered">
            <div className="account-card">
                <form className="is-modern-form has-text-centered" action={props.submitUrl} method="post">
                    <input type="hidden" name="returnUrl" value={props.returnUrl} />
                    <div>
                        <h1 className="subtitle is-4">{props.signInText}</h1>
                    </div>
                    <div className="field">
                        <TextField id="email" name="email" label={props.enterEmailText} fullWidth={true} 
                            value={email} onChange={handleOnChange} helperText={dirty.email ? errors.email : ""} error={(errors.email.length > 0) && dirty.email} />
                    </div>
                    <div className="field">
                        <TextField id="password" name="password" type="password" label={props.enterPasswordText} fullWidth={true} 
                            value={password} onChange={handleOnChange} helperText={dirty.password ? errors.password : ""} error={(errors.password.length > 0) && dirty.password} />
                    </div>
                    <div className="is-flex is-justify-content-end">
                        <button 
                            className="button is-text"
                            onClick={(e) => {
                                e.preventDefault();
                                NavigationHelper.redirect(props.resetPasswordUrl)
                            }}>
                                {props.forgotPasswordLabel}
                        </button>
                    </div>
                    <div className="field mt-2">
                        <Button type="submit" variant="contained" color="primary" disabled={disable} fullWidth={true}>
                            {props.signInText}
                        </Button>
                    </div>
                </form>
            </div>
        </section>
    );
}

SignInForm.propTypes = {
    emailRequiredErrorMessage: PropTypes.string.isRequired,
    passwordRequiredErrorMessage: PropTypes.string.isRequired,
    emailFormatErrorMessage: PropTypes.string.isRequired,
    passwordFormatErrorMessage: PropTypes.string.isRequired,
    signInText: PropTypes.string.isRequired,
    enterEmailText: PropTypes.string.isRequired,
    enterPasswordText: PropTypes.string.isRequired,
    submitUrl: PropTypes.string.isRequired,
    returnUrl: PropTypes.string
};

export default SignInForm;
