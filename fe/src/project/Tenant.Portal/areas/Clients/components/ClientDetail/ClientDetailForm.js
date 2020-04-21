import React from 'react';
import PropTypes from 'prop-types';
import Button from '@material-ui/core/Button';
import useForm from '../../../../../../shared/helpers/forms/useForm';
import EmailValidator from '../../../../../../shared/helpers/validators/EmailValidator';
import ClientDetailService from '../../services/ClientDetail/ClientDetailService';

function ClientDetailForm(props) {

    const stateSchema = {
        name: { value: '', error: '' },
        email: { value: '', error: '' },
        language: { value: '', error: '' }
    };

    const stateValidatorSchema = {

        name: {
            required: {
                isRequired: true,
                error: props.nameRequiredErrorMessage
            }
        },
        email: {
            required: {
                isRequired: true,
                error: props.emailRequiredErrorMessage
            },
            validator: {
                func: value => EmailValidator.validateFormat(value),
                error: props.emailFormatErrorMessage,
            }
        },
        language: {
            required: {
                isRequired: true,
                error: props.languageRequiredErrorMessage
            }
        },
    };

    function onSubmitForm(state) {
        ClientDetailService.Save(props.saveUrl, state, props.generalErrorMessage);
    }

    const {
        values,
        errors,
        dirty,
        handleOnChange,
        handleOnSubmit,
        disable,
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm);

    const { name, email, language } = values;

    return (
        <form className="is-modern-form" onSubmit={handleOnSubmit} method="post">
            <div className="field">
                <label for="name" className="label">{props.nameLabel}</label>
                <input type="text" placeholder={props.enterNameText} id="name" name="name" value={name} onChange={handleOnChange} />
                {errors.name && dirty.name && (
                <div role="alert" className="error-message has-text-danger is-size-7 has-text-weight-bold">{errors.name}</div>
                )}
            </div>
            <div className="field">
                <label for="email" className="label">{props.emailLabel}</label>
                <input type="text" placeholder={props.enterEmailText} id="email" name="email" value={email} onChange={handleOnChange} />
                {errors.email && dirty.email && (
                <div role="alert" className="error-message has-text-danger is-size-7 has-text-weight-bold">{errors.email}</div>
                )}
            </div>
            <div className="field">
                <label for="language" className="label">{props.languageLabel}</label>
                <div className="select is-dark">
                    <select id="language" name="language" value={language} onChange={handleOnChange}>
                        {props.languages.map(language => {
                            return (
                                <option key={language.value} value={language.value}>{language.text}</option>
                            )
                        })}
                    </select>
                </div>
                {errors.language && dirty.language && (
                <div role="alert" className="error-message has-text-danger is-size-7 has-text-weight-bold">{errors.language}</div>
                )}
            </div>
            <div className="field">
                <button className="button is-primary" type="submit" disabled={disable}>{props.saveText}</button>
            </div>
        </form>
    );
}

ClientDetailForm.propTypes = {
    generalErrorMessage: PropTypes.string.isRequired,
    nameLabel: PropTypes.string.isRequired,
    emailLabel: PropTypes.string.isRequired,
    languageLabel: PropTypes.string.isRequired,
    nameRequiredErrorMessage: PropTypes.string.isRequired,
    emailRequiredErrorMessage: PropTypes.string.isRequired,
    languageRequiredErrorMessage: PropTypes.string.isRequired,
    emailFormatErrorMessage: PropTypes.string.isRequired,
    clientDetailText: PropTypes.string.isRequired,
    enterNameText: PropTypes.string.isRequired,
    enterEmailText: PropTypes.string.isRequired,
    saveText: PropTypes.string.isRequired,
    saveUrl: PropTypes.string.isRequired,
    languages: PropTypes.array.isRequired
};

export default ClientDetailForm;
