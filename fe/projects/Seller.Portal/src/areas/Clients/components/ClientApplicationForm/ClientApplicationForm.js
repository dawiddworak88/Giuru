import React, { useContext, useState } from "react";
import PropTypes from "prop-types";
import {
    TextField, Button, InputLabel, CircularProgress,
    FormControl, FormControlLabel, Select, MenuItem, FormHelperText, Checkbox
} from "@mui/material";
import { toast } from "react-toastify";
import { Context } from "../../../../shared/stores/Store";
import useForm from "../../../../shared/helpers/forms/useForm";
import EmailValidator from "../../../../shared/helpers/validators/EmailValidator";
import AuthenticationHelper from "../../../../shared/helpers/globals/AuthenticationHelper";

const ClientApplicationForm = (props) => {
    const [state, dispatch] = useContext(Context);
    const [isDeliveryAddressEqualBillingAddress, setIsDeliveryAddressEqualBillingAddress] = useState(props.isDeliveryAddressEqualBillingAddress);
    const stateSchema = {
        id: { value: props.id ? props.id : null },
        companyName: { value: props.companyName ? props.companyName : "", error: "" },
        firstName: { value: props.firstName ? props.firstName : "", error: "" },
        lastName: { value: props.lastName ? props.lastName : "", error: "" },
        email: { value: props.email ? props.email : "", error: "" },
        phoneNumber: { value: props.phoneNumber ? props.phoneNumber : "", error: "" },
        communicationLanguage: { value: props.communicationLanguage ? props.communicationLanguage : "", error: "" },
        contactJobTitle: { value: props.contactJobTitle ? props.contactJobTitle : "", error: "" },
        billingAddressId: { value: props.billingAddress ? props.billingAddress.id : "" },
        billingAddressFullName: { value: props.billingAddress ? props.billingAddress.fullName : "", error: "" },
        billingAddressPhoneNumber: { value: props.billingAddress ? props.billingAddress.phoneNumber : "", error: "" },
        billingAddressStreet: { value: props.billingAddress ? props.billingAddress.street : "", error: "" },
        billingAddressRegion: { value: props.billingAddress ? props.billingAddress.region : "", error: "" },
        billingAddressPostalCode: { value: props.billingAddress ? props.billingAddress.postalCode : "", error: "" },
        billingAddressCity: { value: props.billingAddress ? props.billingAddress.city : "", error: "" },
        billingAddressCountry: { value: props.billingAddress ? props.billingAddress.country : "", error: "" },
        deliveryAddressId: { value: props.deliveryAddress ? props.deliveryAddress.id : "" },
        deliveryAddressFullName: { value: props.deliveryAddress ? props.deliveryAddress.fullName : "", error: ""  },
        deliveryAddressPhoneNumber: { value: props.deliveryAddress ? props.deliveryAddress.phoneNumber : "", error: "" },
        deliveryAddressStreet: { value: props.deliveryAddress ? props.deliveryAddress.street : "", error: "" },
        deliveryAddressRegion: { value: props.deliveryAddress ? props.deliveryAddress.region : "", error: "" },
        deliveryAddressPostalCode: { value: props.deliveryAddress ? props.deliveryAddress.postalCode : "", error: "" },
        deliveryAddressCity: { value: props.deliveryAddress ? props.deliveryAddress.city : "", error: "" },
        deliveryAddressCountry: { value: props.deliveryAddress ? props.deliveryAddress.country : "", error: "" },
    };

    const stateValidatorSchema = {
        companyName: {
            required: {
                isRequired: true,
                error: props.fieldRequiredErrorMessage
            }
        },
        firstName: {
            required: {
                isRequired: true,
                error: props.fieldRequiredErrorMessage
            }
        },
        lastName: {
            required: {
                isRequired: true,
                error: props.fieldRequiredErrorMessage
            }
        },
        email: {
            required: {
                isRequired: true,
                error: props.fieldRequiredErrorMessage
            },
            validator: {
                func: value => EmailValidator.validateFormat(value),
                error: props.emailFormatErrorMessage
            }
        },
        phoneNumber: {
            required: {
                isRequired: true,
                error: props.fieldRequiredErrorMessage
            }
        },
        contactJobTitle: {
            required: {
                isRequired: true,
                error: props.fieldRequiredErrorMessage
            }
        },
        communicationLanguage: {
            required: {
                isRequired: true,
                error: props.fieldRequiredErrorMessage
            }
        },
        billingAddressFullName: {
            required: {
                isRequired: true,
                error: props.fieldRequiredErrorMessage
            }
        },
        billingAddressPhoneNumber: {
            required: {
                isRequired: true,
                error: props.fieldRequiredErrorMessage
            }
        },
        billingAddressStreet: {
            required: {
                isRequired: true,
                error: props.fieldRequiredErrorMessage
            }
        },
        billingAddressRegion: {
            required: {
                isRequired: true,
                error: props.fieldRequiredErrorMessage
            }
        },
        billingAddressPostalCode: {
            required: {
                isRequired: true,
                error: props.fieldRequiredErrorMessage
            }
        },
        billingAddressCity: {
            required: {
                isRequired: true,
                error: props.fieldRequiredErrorMessage
            }
        },
        billingAddressCountry: {
            required: {
                isRequired: true,
                error: props.fieldRequiredErrorMessage
            }
        },
        deliveryAddressFullName: {
            required: {
                isRequired: !isDeliveryAddressEqualBillingAddress,
                error: props.fieldRequiredErrorMessage
            }
        },
        deliveryAddressPhoneNumber: {
            required: {
                isRequired: !isDeliveryAddressEqualBillingAddress,
                error: props.fieldRequiredErrorMessage
            }
        },
        deliveryAddressStreet: {
            required: {
                isRequired: !isDeliveryAddressEqualBillingAddress,
                error: props.fieldRequiredErrorMessage
            }
        },
        deliveryAddressRegion: {
            required: {
                isRequired: !isDeliveryAddressEqualBillingAddress,
                error: props.fieldRequiredErrorMessage
            }
        },
        deliveryAddressPostalCode: {
            required: {
                isRequired: !isDeliveryAddressEqualBillingAddress,
                error: props.fieldRequiredErrorMessage
            }
        },
        deliveryAddressCity: {
            required: {
                isRequired: !isDeliveryAddressEqualBillingAddress,
                error: props.fieldRequiredErrorMessage
            }
        },
        deliveryAddressCountry: {
            required: {
                isRequired: !isDeliveryAddressEqualBillingAddress,
                error: props.fieldRequiredErrorMessage
            }
        }
    };

    const onSubmitForm = (state) => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        const payload = {
            ...state,
            isDeliveryAddressEqualBillingAddress,
            billingAddress: {
                id: billingAddressId,
                fullName: billingAddressFullName,
                phoneNumber: billingAddressPhoneNumber,
                street: billingAddressStreet,
                region: billingAddressRegion,
                postalCode: billingAddressPostalCode,
                city: billingAddressCity,
                country: billingAddressCountry
            },
            deliveryAddress: {
                id: deliveryAddressId,
                fullName: deliveryAddressFullName,
                phoneNumber: deliveryAddressPhoneNumber,
                street: deliveryAddressStreet,
                region: deliveryAddressRegion,
                postalCode: deliveryAddressPostalCode,
                city: deliveryAddressCity,
                country: deliveryAddressCountry
            }
        }

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" },
            body: JSON.stringify(payload)
        };

        fetch(props.saveUrl, requestOptions)
            .then(response => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {
                    if (response.ok) {
                        toast.success(jsonResponse.message);
                    }
                    else {
                        toast.error(jsonResponse?.message || props.generalErrorMessage);
                    }
                });

            }).catch(() => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(props.generalErrorMessage);
            });
    }

    const {
        values, errors, dirty, disable,
        handleOnChange, handleOnSubmit
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm, !props.id);

    const {
        id, companyName, firstName, lastName, email, phoneNumber, communicationLanguage, contactJobTitle,
        billingAddressId, billingAddressFullName, billingAddressPhoneNumber, billingAddressStreet, billingAddressRegion, billingAddressPostalCode, billingAddressCity, billingAddressCountry,
        deliveryAddressId, deliveryAddressFullName, deliveryAddressPhoneNumber, deliveryAddressStreet, deliveryAddressRegion, deliveryAddressPostalCode, deliveryAddressCity, deliveryAddressCountry,
    } = values;

    return (
        <section className="section section-small-padding product client-form">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div className="columns is-desktop">
                <div className="column is-half">
                    <form className="is-modern-form" onSubmit={handleOnSubmit}>
                        {id &&
                            <div className="field">
                                <InputLabel id="id-label">{props.idLabel} {id}</InputLabel>
                            </div>
                        }
                        <div className="group mb-4">
                            <div className="field">
                                <TextField
                                    id="companyName"
                                    name="companyName"
                                    label={props.companyNameLabel}
                                    fullWidth={true}
                                    value={companyName}
                                    onChange={handleOnChange}
                                    variant="standard"
                                    error={(errors.companyName.length > 0) && dirty.companyName}
                                    helperText={dirty.companyName ? errors.companyName : ""} />
                            </div>
                            <div className="field">
                                <TextField
                                    id="firstName"
                                    name="firstName"
                                    label={props.firstNameLabel}
                                    fullWidth={true}
                                    value={firstName}
                                    onChange={handleOnChange}
                                    variant="standard"
                                    error={(errors.firstName.length > 0) && dirty.firstName}
                                    helperText={dirty.firstName ? errors.firstName : ""} />
                            </div>
                            <div className="field">
                                <TextField
                                    id="lastName"
                                    name="lastName"
                                    label={props.lastNameLabel}
                                    fullWidth={true}
                                    value={lastName}
                                    onChange={handleOnChange}
                                    variant="standard"
                                    error={(errors.lastName.length > 0) && dirty.lastName}
                                    helperText={dirty.lastName ? errors.lastName : ""} />
                            </div>
                            <div className="field">
                                <FormControl fullWidth={true} variant="standard" error={(errors.contactJobTitle.length > 0) && dirty.contactJobTitle}>
                                    <InputLabel id="contactJobTitle-label">{props.contactJobTitleLabel}</InputLabel>
                                    <Select
                                        labelId="contactJobTitle-label"
                                        id="contactJobTitle"
                                        name="contactJobTitle"
                                        value={contactJobTitle}
                                        onChange={handleOnChange}>
                                        <MenuItem key={0} value="">{props.selectJobTitle}</MenuItem>
                                        {props.contactJobTitles && props.contactJobTitles.map((title, index) => {
                                            return (
                                                <MenuItem key={index} value={title.name}>{title.value}</MenuItem>
                                            )
                                        })}
                                    </Select>
                                    {errors.contactJobTitle && dirty.contactJobTitle && (
                                        <FormHelperText>{errors.contactJobTitle}</FormHelperText>
                                    )}
                                </FormControl>
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
                                <TextField
                                    id="email"
                                    name="email"
                                    label={props.emailLabel}
                                    fullWidth={true}
                                    value={email}
                                    onChange={handleOnChange}
                                    variant="standard"
                                    error={(errors.email.length > 0) && dirty.email}
                                    helperText={dirty.email ? errors.email : ""} />
                            </div>
                            <div className="field">
                                <TextField
                                    id="phoneNumber"
                                    name="phoneNumber"
                                    label={props.phoneNumberLabel}
                                    fullWidth={true}
                                    value={phoneNumber}
                                    onChange={handleOnChange}
                                    variant="standard"
                                    error={(errors.phoneNumber.length > 0) && dirty.phoneNumber}
                                    helperText={dirty.phoneNumber ? errors.phoneNumber : ""} />
                            </div>
                        </div>
                        <div className="group">
                            <h1 className="subtitle has-text-centered">{props.billingAddressTitle}</h1>
                            <div className="field">
                                <TextField
                                    id="billingAddressFullName"
                                    name="billingAddressFullName"
                                    value={billingAddressFullName}
                                    fullWidth={true}
                                    variant="standard"
                                    label={props.addressFullNameLabel}
                                    onChange={handleOnChange}
                                    helperText={dirty.billingAddressFullName ? errors.billingAddressFullName : ""}
                                    error={(errors.billingAddressFullName.length > 0) && dirty.billingAddressFullName} />
                            </div>
                            <div className="field">
                                <TextField
                                    id="billingAddressPhoneNumber"
                                    name="billingAddressPhoneNumber"
                                    value={billingAddressPhoneNumber}
                                    fullWidth={true}
                                    variant="standard"
                                    label={props.addressPhoneNumberLabel}
                                    onChange={handleOnChange}
                                    helperText={dirty.billingAddressPhoneNumber ? errors.billingAddressPhoneNumber : ""}
                                    error={(errors.billingAddressPhoneNumber.length > 0) && dirty.billingAddressPhoneNumber} />
                            </div>
                            <div className="field">
                                <TextField
                                    id="billingAddressStreet"
                                    name="billingAddressStreet"
                                    value={billingAddressStreet}
                                    fullWidth={true}
                                    variant="standard"
                                    label={props.addressStreetLabel}
                                    onChange={handleOnChange}
                                    helperText={dirty.billingAddressStreet ? errors.billingAddressStreet : ""}
                                    error={(errors.billingAddressStreet.length > 0) && dirty.billingAddressStreet} />
                            </div>
                            <div className="field">
                                <TextField
                                    id="billingAddressRegion"
                                    name="billingAddressRegion"
                                    value={billingAddressRegion}
                                    fullWidth={true}
                                    variant="standard"
                                    label={props.addressRegionLabel}
                                    onChange={handleOnChange}
                                    helperText={dirty.billingAddressRegion ? errors.billingAddressRegion : ""}
                                    error={(errors.billingAddressRegion.length > 0) && dirty.billingAddressRegion} />
                            </div>
                            <div className="field">
                                <TextField
                                    id="billingAddressPostalCode"
                                    name="billingAddressPostalCode"
                                    value={billingAddressPostalCode}
                                    fullWidth={true}
                                    variant="standard"
                                    label={props.addressPostalCodeLabel}
                                    onChange={handleOnChange}
                                    helperText={dirty.billingAddressPostalCode ? errors.billingAddressPostalCode : ""}
                                    error={(errors.billingAddressPostalCode.length > 0) && dirty.billingAddressPostalCode} />
                            </div>
                            <div className="field">
                                <TextField
                                    id="billingAddressCity"
                                    name="billingAddressCity"
                                    value={billingAddressCity}
                                    fullWidth={true}
                                    variant="standard"
                                    label={props.addressCityLabel}
                                    onChange={handleOnChange}
                                    helperText={dirty.billingAddressCity ? errors.billingAddressCity : ""}
                                    error={(errors.billingAddressCity.length > 0) && dirty.billingAddressCity} />
                            </div>
                            <div className="field">
                                <TextField
                                    id="billingAddressCountry"
                                    name="billingAddressCountry"
                                    value={billingAddressCountry}
                                    fullWidth={true}
                                    variant="standard"
                                    label={props.addressCountryLabel}
                                    onChange={handleOnChange}
                                    helperText={dirty.billingAddressCountry ? errors.billingAddressCountry : ""}
                                    error={(errors.billingAddressCountry.length > 0) && dirty.billingAddressCountry} />
                            </div>
                            <div className="field">
                                <FormControlLabel
                                    control={
                                        <Checkbox
                                            checked={isDeliveryAddressEqualBillingAddress}
                                            onChange={(e) => {
                                                setIsDeliveryAddressEqualBillingAddress(e.target.checked);
                                            }} />
                                    } />
                                <span>{props.deliveryAddressEqualBillingAddressText}</span>
                            </div>
                        </div>
                        {!isDeliveryAddressEqualBillingAddress &&
                            <div>
                                <h1 className="subtitle has-text-centered">{props.deliveryAddressTitle}</h1>
                                <div className="field">
                                    <TextField
                                        id="deliveryAddressFullName"
                                        name="deliveryAddressFullName"
                                        value={deliveryAddressFullName}
                                        fullWidth={true}
                                        variant="standard"
                                        label={props.addressFullNameLabel}
                                        onChange={handleOnChange} 
                                        helperText={dirty.deliveryAddressFullName ? errors.deliveryAddressFullName : ""}
                                        error={(errors.deliveryAddressFullName.length > 0) && dirty.deliveryAddressFullName} />
                                </div>
                                <div className="field">
                                    <TextField
                                        id="deliveryAddressPhoneNumber"
                                        name="deliveryAddressPhoneNumber"
                                        value={deliveryAddressPhoneNumber}
                                        fullWidth={true}
                                        variant="standard"
                                        label={props.addressPhoneNumberLabel}
                                        onChange={handleOnChange}
                                        helperText={dirty.deliveryAddressPhoneNumber ? errors.deliveryAddressPhoneNumber : ""}
                                        error={(errors.deliveryAddressPhoneNumber.length > 0) && dirty.deliveryAddressPhoneNumber} />
                                </div>
                                <div className="field">
                                    <TextField
                                        id="deliveryAddressStreet"
                                        name="deliveryAddressStreet"
                                        value={deliveryAddressStreet}
                                        fullWidth={true}
                                        variant="standard"
                                        label={props.addressStreetLabel}
                                        onChange={handleOnChange}
                                        helperText={dirty.deliveryAddressStreet ? errors.deliveryAddressStreet : ""}
                                        error={(errors.deliveryAddressStreet.length > 0) && dirty.deliveryAddressStreet} />
                                </div>
                                <div className="field">
                                    <TextField
                                        id="deliveryAddressRegion"
                                        name="deliveryAddressRegion"
                                        value={deliveryAddressRegion}
                                        fullWidth={true}
                                        variant="standard"
                                        label={props.addressRegionLabel}
                                        onChange={handleOnChange}
                                        helperText={dirty.deliveryAddressRegion ? errors.deliveryAddressRegion : ""}
                                        error={(errors.deliveryAddressRegion.length > 0) && dirty.deliveryAddressRegion} />
                                </div>
                                <div className="field">
                                    <TextField
                                        id="deliveryAddressPostalCode"
                                        name="deliveryAddressPostalCode"
                                        value={deliveryAddressPostalCode}
                                        fullWidth={true}
                                        variant="standard"
                                        label={props.addressPostalCodeLabel}
                                        onChange={handleOnChange}
                                        helperText={dirty.deliveryAddressPostalCode ? errors.deliveryAddressPostalCode : ""}
                                        error={(errors.deliveryAddressPostalCode.length > 0) && dirty.deliveryAddressPostalCode} />
                                </div>
                                <div className="field">
                                    <TextField
                                        id="deliveryAddressCity"
                                        name="deliveryAddressCity"
                                        value={deliveryAddressCity}
                                        fullWidth={true}
                                        variant="standard"
                                        label={props.addressCityLabel}
                                        onChange={handleOnChange}
                                        helperText={dirty.deliveryAddressCity ? errors.deliveryAddressCity : ""}
                                        error={(errors.deliveryAddressCity.length > 0) && dirty.deliveryAddressCity} />
                                </div>
                                <div className="field">
                                    <TextField
                                        id="deliveryAddressCountry"
                                        name="deliveryAddressCountry"
                                        value={deliveryAddressCountry}
                                        fullWidth={true}
                                        variant="standard"
                                        label={props.addressCountryLabel}
                                        onChange={handleOnChange}
                                        helperText={dirty.deliveryAddressCountry ? errors.deliveryAddressCountry : ""}
                                        error={(errors.deliveryAddressCountry.length > 0) && dirty.deliveryAddressCountry} />
                                </div>
                            </div>
                        }
                        <div className="field">
                            <Button
                                type="submit"
                                variant="contained"
                                color="primary"
                                disabled={state.isLoading || disable}>
                                {props.saveText}
                            </Button>
                            <a href={props.clientsApplicationsUrl} className="ml-2 button is-text">{props.backToClientsApplicationsText}</a>
                        </div>
                    </form>
                    {state.isLoading && <CircularProgress className="progressBar" />}
                </div>
            </div>
        </section>
    );
}

ClientApplicationForm.propTypes = {
    title: PropTypes.string.isRequired,
    idLabel: PropTypes.string,
    postalCodeLabel: PropTypes.string.isRequired,
    regionLabel: PropTypes.string.isRequired,
    id: PropTypes.string,
    cityLabel: PropTypes.string.isRequired,
    countryLabel: PropTypes.string.isRequired,
    addressLabel: PropTypes.string.isRequired,
    phoneNumberLabel: PropTypes.string.isRequired,
    emailLabel: PropTypes.string.isRequired,
    contactJobTitleLabel: PropTypes.string.isRequired,
    addressFullNameLabel: PropTypes.string.isRequired,
    addressPhoneNumberLabel: PropTypes.string.isRequired,
    addressStreetLabel: PropTypes.string.isRequired,
    addressRegionLabel: PropTypes.string.isRequired,
    addressPostalCodeLabel: PropTypes.string.isRequired,
    deliveryAddressCity: PropTypes.string.isRequired,
    addressCountryLabel: PropTypes.string.isRequired,
    lastNameLabel: PropTypes.string.isRequired,
    firstNameLabel: PropTypes.string.isRequired,
    firstName: PropTypes.string,
    lastName: PropTypes.string,
    contactJobTitle: PropTypes.string,
    email: PropTypes.string,
    phoneNumber: PropTypes.string,
    companyName: PropTypes.string,
    companyAddress: PropTypes.string,
    companyCountry: PropTypes.string,
    companyCity: PropTypes.string,
    companyRegion: PropTypes.string,
    companyPostalCode: PropTypes.string,
    backToClientsApplicationsText: PropTypes.string.isRequired,
    emailFormatErrorMessage: PropTypes.string.isRequired,
    fieldRequiredErrorMessage: PropTypes.string,
    saveText: PropTypes.string.isRequired,
    contactJobTitles: PropTypes.array.isRequired,
    selectJobTitle: PropTypes.string.isRequired
};

export default ClientApplicationForm;