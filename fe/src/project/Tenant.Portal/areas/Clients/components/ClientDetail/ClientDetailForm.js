import React, { useContext }  from 'react';
import PropTypes from 'prop-types';
import { CircularProgress } from '@material-ui/core';
import { Context } from '../../../../../../shared/stores/Store';
import { TextField, Button, FormControl, InputLabel, Select, MenuItem, FormHelperText } from '@material-ui/core';
import useForm from '../../../../../../shared/helpers/forms/useForm';
import EmailValidator from '../../../../../../shared/helpers/validators/EmailValidator';
import ClientDetailService from '../../services/ClientDetail/ClientDetailService';

function ClientDetailForm(props) {

    const [state, dispatch] = useContext(Context);

    const stateSchema = {
        name: { value: '', error: '' },
        email: { value: '', error: '' },
        communicationLanguage: { value: '', error: '' }
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
        communicationLanguage: {
            required: {
                isRequired: true,
                error: props.languageRequiredErrorMessage
            }
        },
    };

    function onSubmitForm(state) {
        ClientDetailService.Save(props.saveUrl, state, props.generalErrorMessage, dispatch);
    }

    const {
        values,
        errors,
        dirty,
        handleOnChange,
        handleOnSubmit
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm);

    const { name, email, language } = values;

    return (
        <form className="is-modern-form" onSubmit={handleOnSubmit} method="post">
            <div className="field">
                <TextField id="name" name="name" label={props.nameLabel} fullWidth={true}
                    value={name} onChange={handleOnChange} helperText={errors.name && dirty.name && errors.name} error={dirty.name} />
            </div>
            <div className="field">
                <TextField id="email" name="email" label={props.emailLabel} fullWidth={true}
                    value={email} onChange={handleOnChange} helperText={errors.email && dirty.email && errors.email} error={dirty.email} />
            </div>
            <div className="field">
                <FormControl fullWidth={true}>
                    <InputLabel id="language-label">{props.languageLabel}</InputLabel>
                    <Select
                        labelId="language-label"
                        id="communicationLanguage"
                        name="communicationLanguage"
                        value={language}
                        onChange={handleOnChange}>
                        {props.languages.map(language => {
                            return (
                                <MenuItem key={language.value} value={language.value}>{language.text}</MenuItem>
                            )
                        })}
                    </Select>
                    {errors.language && dirty.language && (
                        <FormHelperText>{errors.language}</FormHelperText>
                    )}
                </FormControl>
            </div>
            <div className="field">
                <Button type="submit" variant="contained" color="primary" disabled={state.isLoading}>
                    {props.saveText}
                </Button>
            </div>
            {state.isLoading && <CircularProgress className="progressBar" />}
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
