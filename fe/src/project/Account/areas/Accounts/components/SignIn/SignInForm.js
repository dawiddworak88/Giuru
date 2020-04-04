import React from 'react';
import PropTypes from 'prop-types';
import useForm from '../../../../../../shared/helpers/forms/useForm';
import EmailValidator from '../../../../../../shared/helpers/validators/EmailValidator';
import PasswordValidator from '../../../../../../shared/helpers/validators/PasswordValidator';

function SignInForm(props) {

    const stateSchema = {
        email: { value: '', error: '' },
        password: { value: '', error: '' }
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

    function onSubmitForm(state) {
        alert(JSON.stringify(state));
    }

    const {
        values,
        errors,
        dirty,
        handleOnChange,
        handleOnSubmit,
        disable,
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm);

    const { email, password } = values;

    return (
        <form className="is-modern-form has-text-centered" onSubmit={handleOnSubmit} method="post">
            <div>
                <h1 className="subtitle is-4">{props.signInText}</h1>
            </div>
            <div className="field">
                <input type="text" placeholder={props.enterEmailText} name="email" value={email} onChange={handleOnChange} />
                {errors.email && dirty.email && (
                <span role="alert" className="has-text-danger is-size-7 has-text-weight-bold">{errors.email}</span>
                )}
            </div>
            <div className="field">
                <input type="password" placeholder={props.enterPasswordText} name="password" value={password} onChange={handleOnChange} />
                {errors.password && dirty.password && (
                <span role="alert" className="has-text-danger is-size-7 has-text-weight-bold">{errors.password}</span>
                )}
            </div>
            <div className="field">
                <button className="button is-primary is-fullwidth" type="submit" disabled={disable}>{props.signInText}</button>
            </div>
        </form>
    );
}

SignInForm.propTypes = {
    emailRequiredErrorMessage: PropTypes.string.isRequired,
    passwordRequiredErrorMessage: PropTypes.string.isRequired,
    emailFormatErrorMessage: PropTypes.string.isRequired,
    passwordFormatErrorMessage: PropTypes.string.isRequired,
    signInText: PropTypes.string.isRequired,
    enterEmailText: PropTypes.string.isRequired,
    enterPasswordText: PropTypes.string.isRequired
};

export default SignInForm;
