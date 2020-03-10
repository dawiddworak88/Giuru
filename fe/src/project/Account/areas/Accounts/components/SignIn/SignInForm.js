import React from 'react';
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
                error: 'Enter e-mail'
            },
            validator: {
                func: value => EmailValidator.validateFormat(value),
                error: 'Invalid email format.',
            },
        },
        password: {
            required: {
                isRequired: true,
                error: 'Enter password'
            },
            validator: {
                func: value => PasswordValidator.validateFormat(value),
                error: 'Invalid password format.',
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
                <h1 className="title is-1">Sign in</h1>
            </div>
            <div className="field">
                <input type="text" placeholder="Enter e-mail" name="email" value={email} onChange={handleOnChange} />
                {errors.email && dirty.email && (
                <span role="alert" className="has-text-danger is-size-7 has-text-weight-bold">{errors.email}</span>
                )}
            </div>
            <div className="field">
                <input type="password" placeholder="Enter password" name="password" value={password} onChange={handleOnChange} />
                {errors.password && dirty.password && (
                <span role="alert" className="has-text-danger is-size-7 has-text-weight-bold">{errors.password}</span>
                )}
            </div>
            <div className="field">
                <button className="button is-primary is-fullwidth" type="submit" disabled={disable}>Sign in</button>
            </div>
        </form>
    );
}

export default SignInForm;
