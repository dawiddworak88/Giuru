import React, { useContext, useState } from "react";
import { toast } from "react-toastify";
import PropTypes from "prop-types";
import { Context } from "../../../../shared/stores/Store";
import { 
    TextField, Button, FormControl, InputLabel, 
    Select, MenuItem, FormHelperText, CircularProgress, 
    Autocomplete, NoSsr, FormControlLabel, Switch
} from "@mui/material";
import useForm from "../../../../shared/helpers/forms/useForm";
import EmailValidator from "../../../../shared/helpers/validators/EmailValidator";
import AuthenticationHelper from "../../../../shared/helpers/globals/AuthenticationHelper";

function ClientForm(props) {
    const [state, dispatch] = useContext(Context);
    const [canCreateAccount, setCanCreateAccount] = useState(props.hasAccount ? props.hasAccount : false);
    const stateSchema = {
        id: { value: props.id ? props.id : null, error: "" },
        name: { value: props.name ? props.name : "", error: "" },
        email: { value: props.email ? props.email : "", error: "" },
        communicationLanguage: { value: props.communicationLanguage ? props.communicationLanguage : null, error: "" },
        phoneNumber: { value: props.phoneNumber ? props.phoneNumber : null },
        country: { value: props.countryId ? props.countries.find((item) => item.id === props.countryId) : null },
        preferedCurrency: { value: props.preferedCurrencyId ? props.currencies.find((item) => item.id === props.preferedCurrencyId) : null },
        clientGroupIds: { value: props.clientGroupsIds ? props.clientGroupsIds : []},
        clientManagerIds: { value: props.clientManagersIds ? props.clientManagersIds : []},
        hasAccount: { value: props.hasAccount ? props.hasAccount : false },
        isDisabled: { value: props.isDisabled ? props.isDisabled : false },
        deliveryAddress: { value: props.defaultDeliveryAddressId ? props.clientAddresses.find((item) => item.id === props.defaultDeliveryAddressId) : null },
        billingAddress: { value: props.defaultBillingAddressId ? props.clientAddresses.find((item) => item.id === props.defaultBillingAddressId) : null }
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
        }
    };

    const countriesProps = {
        options: props.countries,
        getOptionLabel: (option) => option.name
    };

    const currenciesPorps = {
        options: props.currencies,
        getOptionLabel: (option) => option.name
    };

    function onSubmitForm(state) {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        const payload = {
            ...state,
            countryId: country ? country.id : null, 
            preferedCurrencyId: preferedCurrency ? preferedCurrency.id : null,
            defaultDeliveryAddressId: state.deliveryAddress ? state.deliveryAddress.id : null,
            defaultBillingAddressId: state.billingAddress ? state.billingAddress.id : null
        }

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" },
            body: JSON.stringify(payload)
        }

        fetch(props.saveUrl, requestOptions)
            .then(function (response) {
                dispatch({ type: "SET_IS_LOADING", payload: false });

                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {
                    if (response.ok) {
                        setFieldValue({ name: "id", value: jsonResponse.id });
                        setCanCreateAccount(true);
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
    }

    const createAccount = () => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        const payload = {
            name: values.name,
            email: values.email,
            communicationLanguage: values.communicationLanguage
        };

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" },
            body: JSON.stringify(payload)
        };

        fetch(props.accountUrl, requestOptions)
            .then((response) => {
                dispatch({ type: "SET_IS_LOADING", payload: false });

                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {
                    if (response.ok) {
                        setCanCreateAccount(false);
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
        values, errors, dirty, disable,
        setFieldValue, handleOnChange, handleOnSubmit
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm, !props.id);

    const { 
        id, name, email, country, preferedCurrency, clientGroupIds, 
        communicationLanguage, phoneNumber, clientManagerIds,
        deliveryAddress, billingAddress, isDisabled
    } = values;

    return (
        <section className="section section-small-padding product client-form">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div className="columns is-desktop">
                <div className="column is-half">
                    <form className="is-modern-form" onSubmit={handleOnSubmit} method="post">
                        {id &&
                            <div className="field">
                                <InputLabel id="id-label">{props.idLabel} {id}</InputLabel>
                            </div>
                        }                      
                        <div className="field">
                            <TextField 
                                id="name" 
                                name="name" 
                                label={props.nameLabel} 
                                fullWidth={true}
                                value={name} 
                                variant="standard"
                                onChange={handleOnChange} 
                                helperText={dirty.name ? errors.name : ""} 
                                error={(errors.name.length > 0) && dirty.name} />
                        </div>
                        <div className="field">
                            <TextField 
                                id="email" 
                                name="email" 
                                label={props.emailLabel} 
                                fullWidth={true}
                                value={email} 
                                onChange={handleOnChange} 
                                variant="standard"
                                helperText={dirty.email ? errors.email : ""} 
                                error={(errors.email.length > 0) && dirty.email}
                                InputProps={{
                                    readOnly: props.email ? true : false,
                                }} />
                        </div>
                        <div className="field">
                            <Autocomplete
                                {...countriesProps}
                                id="country"
                                name="country"
                                fullWidth={true}
                                value={country}
                                variant="standard"
                                onChange={(event, newValue) => {
                                    setFieldValue({name: "country", value: newValue});
                                }}
                                autoComplete
                                renderInput={(params) => (
                                    <TextField 
                                        {...params} 
                                        label={props.countryLabel} 
                                        variant="standard"
                                        margin="normal"/>
                                )} />
                        </div>
                        <div className="field">
                            <Autocomplete
                                {...currenciesPorps}
                                id="preferedCurrency"
                                name="preferedCurrency"
                                fullWidth={true}
                                value={preferedCurrency}
                                variant="standard"
                                onChange={(event, newValue) => {
                                    setFieldValue({name: "preferedCurrency", value: newValue});
                                }}
                                autoComplete
                                renderInput={(params) => (
                                    <TextField 
                                        {...params} 
                                        label={props.preferedCurrencyLabel} 
                                        variant="standard"
                                        margin="normal"/>
                                )} />
                        </div>
                        <div className="field">
                            <FormControl fullWidth={true} error={(errors.communicationLanguage.length > 0) && dirty.communicationLanguage} variant="standard">
                                <InputLabel id="language-label">{props.languageLabel}</InputLabel>
                                <Select
                                    labelId="language-label"
                                    id="communicationLanguage"
                                    name="communicationLanguage"
                                    value={communicationLanguage}
                                    onChange={handleOnChange}>
                                    {props.languages && props.languages.length > 0 && props.languages.map((language, index) => {
                                        return (
                                            <MenuItem key={index} value={language.value}>{language.text}</MenuItem>
                                        );
                                    })}
                                </Select>
                                {errors.communicationLanguage && dirty.communicationLanguage && (
                                    <FormHelperText>{errors.communicationLanguage}</FormHelperText>
                                )}
                            </FormControl>
                        </div>
                        <div className="field">
                            <FormControl fullWidth={true} variant="standard">
                                <InputLabel id="clientGroups-label">{props.groupsLabel}</InputLabel>
                                <Select
                                    labelId="clientGroups-label"
                                    id="clientGroupIds"
                                    name="clientGroupIds"
                                    value={clientGroupIds}
                                    multiple={true}
                                    onChange={handleOnChange}>
                                    {props.clientGroups && props.clientGroups.length > 0 ? (
                                        props.clientGroups.map((group, index) => {
                                            return (
                                                <MenuItem key={index} value={group.id}>{group.name}</MenuItem>
                                            );
                                        })
                                    ) : (
                                        <MenuItem disabled>{props.noGroupsText}</MenuItem>
                                    )}
                                </Select>
                            </FormControl>
                        </div>
                        <div className="field">
                            <Autocomplete
                                id="deliveryAddress"
                                name="deliveryAddress"
                                options={props.clientAddresses}
                                getOptionLabel={(option) => option.name}
                                fullWidth={true}
                                value={deliveryAddress}
                                variant="standard"
                                onChange={(event, newValue) => {
                                    setFieldValue({name: "deliveryAddress", value: newValue});
                                }}
                                autoComplete
                                renderInput={(params) => (
                                    <TextField 
                                        {...params} 
                                        label={props.deliveryAddressLabel} 
                                        variant="standard"
                                        margin="normal"/>
                                )} />
                        </div>
                        <div className="field">
                            <Autocomplete
                                id="billingAddress"
                                name="billingAddress"
                                options={props.clientAddresses}
                                getOptionLabel={(option) => option.name}
                                fullWidth={true}
                                value={billingAddress}
                                variant="standard"
                                onChange={(event, newValue) => {
                                    setFieldValue({name: "billingAddress", value: newValue});
                                }}
                                autoComplete
                                renderInput={(params) => (
                                    <TextField 
                                        {...params} 
                                        label={props.billingAddressLabel} 
                                        variant="standard"
                                        margin="normal"/>
                                )} />
                        </div>
                        <div className="field">
                            <FormControl fullWidth={true} variant="standard">
                                <InputLabel id="clientManagers-label">{props.clientManagerLabel}</InputLabel>
                                <Select
                                    labelId="clientManagers-label"
                                    id="clientManagerIds"
                                    name="clientManagerIds"
                                    value={clientManagerIds}
                                    multiple={true}
                                    onChange={handleOnChange}>
                                    {props.clientManagers && props.clientManagers.length > 0 ? (
                                        props.clientManagers.map((manager, index) => {
                                            return (
                                                <MenuItem key={index} value={manager.id}>{manager.firstName} {manager.lastName}</MenuItem>
                                            );
                                        })
                                    ) : (
                                        <MenuItem disabled>{props.noManagersText}</MenuItem>
                                    )}
                                </Select>
                            </FormControl>
                        </div>
                        <div className="field">
                            <TextField 
                                id="phoneNumber" 
                                name="phoneNumber" 
                                label={props.phoneNumberLabel} 
                                fullWidth={true}
                                value={phoneNumber} 
                                variant="standard"
                                onChange={handleOnChange} />
                        </div>
                        <div className="field">
                            <NoSsr>
                                <FormControlLabel
                                    control={
                                        <Switch
                                            onChange={e => {
                                                setFieldValue({ name: "isDisabled", value: e.target.checked });
                                            }}
                                            checked={isDisabled}
                                            id="isDisabled"
                                            name="isDisabled"
                                            color="secondary" 
                                        />
                                    }
                                    label={isDisabled ? props.inActiveLabel : props.activeLabel} />
                            </NoSsr>
                        </div>
                        <div className="field client-form__field-row">
                            <Button 
                                type="submit" 
                                variant="contained" 
                                color="primary" 
                                disabled={state.isLoading || disable}>
                                {props.saveText}
                            </Button>
                            <Button 
                                className="ml-2 "
                                type="button" 
                                color="secondary" 
                                variant="contained" 
                                onClick={createAccount} 
                                disabled={state.isLoading || !canCreateAccount}>
                                {props.hasAccount ? props.resetPasswordText : props.accountText}
                            </Button>
                            <a href={props.clientsUrl} className="field-button button is-text">{props.navigateToClientsLabel}</a>
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
    accountText: PropTypes.string,
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
    languages: PropTypes.array.isRequired,
    phoneNumberLabel: PropTypes.string.isRequired,
    resetPasswordText: PropTypes.string.isRequired,
    idLabel: PropTypes.string,
    noGroupsText: PropTypes.string.isRequired,
    groupsLabel: PropTypes.string.isRequired,
    clientManagerLabel: PropTypes.string.isRequired,
    clientManagers: PropTypes.array,
    noManagersText: PropTypes.string.isRequired,
    clientManagerIds: PropTypes.array,
    country: PropTypes.string,
    countryLabel: PropTypes.string.isRequired
};

export default ClientForm;
