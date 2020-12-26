import React, { useContext } from "react";
import PropTypes from "prop-types";
import { Context } from "../../../../../../shared/stores/Store";
import { TextField, Button, FormControl, InputLabel, Select, MenuItem, FormHelperText, CircularProgress } from "@material-ui/core";
import useForm from "../../../../../../shared/helpers/forms/useForm";
import EmailValidator from "../../../../../../shared/helpers/validators/EmailValidator";

function ClientForm(props) {

    const [state, dispatch] = useContext(Context);

    const stateSchema = {

        id: { value: props.id ? props.id : null, error: "" },
        name: { value: props.name ? props.name : "", error: "" },
        contactEmail: { value: props.contactEmail ? props.contactEmail : "", error: "" },
        communicationLanguage: { value: props.communicationLanguage ? props.communicationLanguage : "", error: "" }
    };

    const stateValidatorSchema = {

        name: {
            required: {
                isRequired: true,
                error: props.nameRequiredErrorMessage
            }
        },
        contactEmail: {
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
        }
    };

    function onSubmitForm(state) {

        dispatch({ type: "SET_IS_LOADING", payload: true });

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(state)
        };

        fetch(props.saveUrl, requestOptions)
            .then(function (response) {

                dispatch({ type: "SET_IS_LOADING", payload: false });

                return response.json().then(jsonResponse => {

                    if (response.ok) {

                        setFieldValue({ name: "id", value: jsonResponse.id });
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
    };

    const {
        values,
        errors,
        dirty,
        disable,
        setFieldValue,
        handleOnChange,
        handleOnSubmit
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm, !props.id);

    const { id, name, contactEmail, communicationLanguage } = values;

    return (
        <section className="section section-small-padding product">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div className="columns is-desktop">
                <div className="column is-half">
                    <form className="is-modern-form" onSubmit={handleOnSubmit} method="post">
                        {id &&
                            <input id="id" name="id" type="hidden" value={id} />
                        }
                        <div className="field">
                            <TextField id="name" name="name" label={props.nameLabel} fullWidth={true}
                                value={name} onChange={handleOnChange} helperText={dirty.name ? errors.name : ""} error={(errors.name.length > 0) && dirty.name} />
                        </div>
                        <div className="field">
                            <TextField id="contactEmail" name="contactEmail" label={props.emailLabel} fullWidth={true}
                                value={contactEmail} onChange={handleOnChange} helperText={dirty.contactEmail ? errors.contactEmail : ""} error={(errors.contactEmail.length > 0) && dirty.contactEmail} />
                        </div>
                        <div className="field">
                            <FormControl fullWidth={true} error={(errors.communicationLanguage.length > 0) && dirty.communicationLanguage}>
                                <InputLabel id="language-label">{props.languageLabel}</InputLabel>
                                <Select
                                    labelId="language-label"
                                    id="communicationLanguage"
                                    name="communicationLanguage"
                                    value={communicationLanguage}
                                    onChange={handleOnChange}>
                                    {props.languages.map(language => {
                                        return (
                                            <MenuItem key={language.value} value={language.value}>{language.text}</MenuItem>
                                        );
                                    })}
                                </Select>
                                {errors.communicationLanguage && dirty.communicationLanguage && (
                                    <FormHelperText>{errors.communicationLanguage}</FormHelperText>
                                )}
                            </FormControl>
                        </div>
                        <div className="field">
                            <Button type="submit" variant="contained" color="primary" disabled={state.isLoading || disable}>
                                {props.saveText}
                            </Button>
                        </div>
                    </form>
                    {state.isLoading && <CircularProgress className="progressBar" />}
                </div>
            </div>
        </section>
    );
}

ClientForm.propTypes = {
    id: PropTypes.string,
    name: PropTypes.string,
    email: PropTypes.string,
    communicationLanguage: PropTypes.string,
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

export default ClientForm;
