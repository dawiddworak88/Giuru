import React from "react";
import PropTypes from "prop-types";
import { TextField, Button } from "@mui/material";
import useForm from "../../../../shared/helpers/forms/useForm";
import EmailValidator from "../../../../shared/helpers/validators/EmailValidator";
import PasswordValidator from "../../../../shared/helpers/validators/PasswordValidator";
import NavigationHelper from "../../../../shared/helpers/globals/NavigationHelper";

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
        <section className="section is-flex-direction-column is-flex-centered sign-in">
            <div className="account-card">
                <form className="is-modern-form has-text-centered" action={props.submitUrl} method="post">
                    <input type="hidden" name="returnUrl" value={props.returnUrl} />
                    <h1 className="subtitle is-4">{props.signInText}</h1>
                    <div className="field">
                        <TextField id="email" name="email" label={props.enterEmailText} fullWidth={true} variant="standard"
                            value={email} onChange={handleOnChange} helperText={dirty.email ? errors.email : ""} error={(errors.email.length > 0) && dirty.email} />
                    </div>
                    <div className="field">
                        <TextField id="password" name="password" type="password" label={props.enterPasswordText} fullWidth={true} variant="standard"
                            value={password} onChange={handleOnChange} helperText={dirty.password ? errors.password : ""} error={(errors.password.length > 0) && dirty.password} />
                    </div>
                    <div className="is-flex is-justify-content-end">
                        <a className="button is-text is-size-7" href={props.resetPasswordUrl}>{props.forgotPasswordLabel}</a>
                    </div>
                    {props.errorMessage &&
                        <p className="has-text-danger is-size-7">{props.errorMessage}</p>
                    }
                    <div className="field mt-4">
                        <Button type="submit" variant="contained" color="primary" disabled={disable} fullWidth={true}>
                            {props.signInText}
                        </Button>
                    </div>
                </form>
            </div>
            <div className="account-container has-text-centered mt-4">
                <h2 className="title is-size-6 mb-3">{props.registerLabel}</h2>
                <Button 
                    className="sign-in__client-apply"
                    type="text" 
                    variant="contained" 
                    color="primary" 
                    onClick={() => 
                        NavigationHelper.redirect(props.registerUrl)
                    }
                    fullWidth={true}>
                    {props.registerButtonText}
                </Button>
                <div className="mt-6">
                    <p>{props.contactText}</p>
                    <a href={`mailto:${props.developersEmail}`} className="is-underlined">{props.developersEmail}</a>
                </div>
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
    returnUrl: PropTypes.string,
    registerLabel: PropTypes.string.isRequired,
    developersEmail: PropTypes.string.isRequired,
    contactText: PropTypes.string.isRequired,
    registerUrl: PropTypes.string.isRequired,
    errorMessage: PropTypes.string
};

export default SignInForm;
