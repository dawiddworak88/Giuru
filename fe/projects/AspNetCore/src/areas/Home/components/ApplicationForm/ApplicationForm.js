import React, { useState, useContext } from "react";
import PropTypes from "prop-types";
import { toast } from "react-toastify";
import { Context } from "../../../../shared/stores/Store"
import {
    Stepper, Step, StepLabel, StepContent, TextField, Button, FormHelperText,
    FormControl, InputLabel, Select, MenuItem, NoSsr, FormControlLabel, Checkbox
} from "@mui/material";
import useForm from "../../../../shared/helpers/forms/useForm";
import EmailValidator from "../../../../shared/helpers/validators/EmailValidator";
import NavigationHelper from "../../../../shared/helpers/globals/NavigationHelper";
import AuthenticationHelper from "../../../../shared/helpers/globals/AuthenticationHelper";

const ApplicationForm = (props) => {
    const [state, dispatch] = useContext(Context);
    const [isSended, setIsSended] = useState(false);
    const [activeStep, setActiveStep] = useState(0);
    const stateSchema = {
        firstName: { value: "", error: "" },
        lastName: { value: "", error: "" },
        email: { value: "", error: "" },
        phoneNumber: { value: "", error: "" },
        contactJobTitle: { value: "", error: "" },
        communicationLanguage: { value: "", error: "" },
        companyName: { value: "", error: "" },
        companyAddress: { value: "", error: "" },
        companyCity: { value: "", error: "" },
        companyRegion: { value: "", error: "" },
        companyPostalCode: { value: "", error: "" },
        companyCountry: { value: "", error: "" },
        billingAddressFullName: { value: "", error: "" },
        billingAddressPhoneNumber: { value: "", error: "" },
        billingAddressStreet: { value: "", error: "" },
        billingAddressRegion: { value: "", error: "" },
        billingAddressPostalCode: { value: "", error: "" },
        billingAddressCity: { value: "", error: "" },
        billingAddressCountry: { value: "", error: "" },
        isDeliveryAddressEqualBillingAddress: { value: true },
        deliveryAddressFullName: { value: "" },
        deliveryAddressPhoneNumber: { value: "" },
        deliveryAddressStreet: { value: "" },
        deliveryAddressRegion: { value: "" },
        deliveryAddressPostalCode: { value: "" },
        deliveryAddressCity: { value: "" },
        deliveryAddressCountry: { value: "" },
        acceptedTerms: { value: false }
    };

    const stateValidatorSchema = {
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
        companyName: {
            required: {
                isRequired: true,
                error: props.fieldRequiredErrorMessage
            }
        },
        companyAddress: {
            required: {
                isRequired: true,
                error: props.fieldRequiredErrorMessage
            }
        },
        companyCity: {
            required: {
                isRequired: true,
                error: props.fieldRequiredErrorMessage
            }
        },
        companyRegion: {
            required: {
                isRequired: true,
                error: props.fieldRequiredErrorMessage
            }
        },
        companyPostalCode: {
            required: {
                isRequired: true,
                error: props.fieldRequiredErrorMessage
            }
        },
        companyCountry: {
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
        }
    };

    const onSubmitForm = (state) => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        const payload = {
            ...state,
            billingAddress: {
                fullName: billingAddressFullName,
                phoneNumber: billingAddressPhoneNumber,
                street: billingAddressStreet,
                region: billingAddressRegion,
                postalCode: billingAddressPostalCode,
                city: billingAddressCity,
                country: billingAddressCountry
            },
            deliveryAddress: {
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
                        setIsSended(true);
                        setTimeout(() => {
                            NavigationHelper.redirect(props.signInUrl)
                        }, 3000);
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

    const {
        values, errors, dirty, disable,
        setFieldValue, handleOnChange, handleOnSubmit
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm);

    const {
        firstName, lastName, email, phoneNumber, contactJobTitle, communicationLanguage, companyName,
        companyAddress, companyCity, companyCountry, companyRegion, companyPostalCode,
        billingAddressFullName, billingAddressPhoneNumber, billingAddressStreet, billingAddressRegion, billingAddressPostalCode, billingAddressCity, billingAddressCountry,
        isDeliveryAddressEqualBillingAddress,
        deliveryAddressFullName, deliveryAddressPhoneNumber, deliveryAddressStreet, deliveryAddressRegion, deliveryAddressPostalCode, deliveryAddressCity, deliveryAddressCountry,
        acceptedTerms
    } = values;

    return (
        <div className="container application-form">
            <div className="columns">
                <div className="column">
                    <div className="application-form__stepper p-6">
                        <h1 className="title">{props.title}</h1>
                        <p className="subtitle mb-2 mt-1">{props.subtitle}</p>
                        {props.steps &&
                            <Stepper activeStep={activeStep} orientation="vertical">
                                {props.steps.length > 0 && props.steps.map((step, index) =>
                                    <Step completed={false} key={index}>
                                        <StepLabel>{step.title}</StepLabel>
                                        <StepContent>{step.subtitle}</StepContent>
                                    </Step>
                                )}
                            </Stepper>
                        }
                    </div>
                </div>
                <div className="column">
                    <form className="application-form__groups p-6" onSubmit={handleOnSubmit}>
                        <div className="group mb-6" onFocus={() => setActiveStep(0)}>
                            <h1 className="subtitle has-text-centered">{props.contactInformationTitle}</h1>
                            <div className="field">
                                <TextField
                                    id="firstName"
                                    name="firstName"
                                    value={firstName}
                                    fullWidth={true}
                                    variant="standard"
                                    label={props.firstNameLabel}
                                    onChange={handleOnChange}
                                    helperText={dirty.firstName ? errors.firstName : ""}
                                    error={(errors.firstName.length > 0) && dirty.firstName} />
                            </div>
                            <div className="field">
                                <TextField
                                    id="lastName"
                                    name="lastName"
                                    value={lastName}
                                    fullWidth={true}
                                    variant="standard"
                                    label={props.lastNameLabel}
                                    onChange={handleOnChange}
                                    helperText={dirty.lastName ? errors.lastName : ""}
                                    error={(errors.lastName.length > 0) && dirty.lastName} />
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
                                <TextField
                                    id="email"
                                    name="email"
                                    value={email}
                                    fullWidth={true}
                                    variant="standard"
                                    label={props.emailLabel}
                                    onChange={handleOnChange}
                                    helperText={dirty.email ? errors.email : ""}
                                    error={(errors.email.length > 0) && dirty.email} />
                            </div>
                            <div className="field">
                                <TextField
                                    id="phoneNumber"
                                    name="phoneNumber"
                                    value={phoneNumber}
                                    fullWidth={true}
                                    variant="standard"
                                    label={props.phoneNumberLabel}
                                    onChange={handleOnChange}
                                    helperText={dirty.phoneNumber ? errors.phoneNumber : ""}
                                    error={(errors.phoneNumber.length > 0) && dirty.phoneNumber} />
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
                        </div>
                        <div className="group mb-6" onFocus={() => setActiveStep(1)}>
                            <h1 className="subtitle has-text-centered">{props.businessInformationTitle}</h1>
                            <div className="field">
                                <TextField
                                    id="companyName"
                                    name="companyName"
                                    value={companyName}
                                    fullWidth={true}
                                    variant="standard"
                                    label={props.companyNameLabel}
                                    onChange={handleOnChange}
                                    helperText={dirty.companyName ? errors.companyName : ""}
                                    error={(errors.companyName.length > 0) && dirty.companyName} />
                            </div>
                            <div className="field">
                                <TextField
                                    id="companyAddress"
                                    name="companyAddress"
                                    value={companyAddress}
                                    fullWidth={true}
                                    variant="standard"
                                    label={props.companyAddressLabel}
                                    onChange={handleOnChange}
                                    helperText={dirty.companyAddress ? errors.companyAddress : ""}
                                    error={(errors.companyAddress.length > 0) && dirty.companyAddress} />
                            </div>
                            <div className="field">
                                <TextField
                                    id="companyCountry"
                                    name="companyCountry"
                                    value={companyCountry}
                                    fullWidth={true}
                                    variant="standard"
                                    label={props.companyCountryLabel}
                                    onChange={handleOnChange}
                                    helperText={dirty.companyCountry ? errors.companyCountry : ""}
                                    error={(errors.companyCountry.length > 0) && dirty.companyCountry} />
                            </div>
                            <div className="field">
                                <TextField
                                    id="companyCity"
                                    name="companyCity"
                                    value={companyCity}
                                    fullWidth={true}
                                    variant="standard"
                                    label={props.companyCityLabel}
                                    onChange={handleOnChange}
                                    helperText={dirty.companyCity ? errors.companyCity : ""}
                                    error={(errors.companyCity.length > 0) && dirty.companyCity} />
                            </div>
                            <div className="field">
                                <TextField
                                    id="companyRegion"
                                    name="companyRegion"
                                    value={companyRegion}
                                    fullWidth={true}
                                    variant="standard"
                                    label={props.companyRegionLabel}
                                    onChange={handleOnChange}
                                    helperText={dirty.companyRegion ? errors.companyRegion : ""}
                                    error={(errors.companyRegion.length > 0) && dirty.companyRegion}
                                />
                            </div>
                            <div className="field">
                                <TextField
                                    id="companyPostalCode"
                                    name="companyPostalCode"
                                    fullWidth={true}
                                    value={companyPostalCode}
                                    variant="standard"
                                    label={props.companyPostalCodeLabel}
                                    onChange={handleOnChange}
                                    helperText={dirty.companyPostalCode ? errors.companyPostalCode : ""}
                                    error={(errors.companyPostalCode.length > 0) && dirty.companyPostalCode} />
                            </div>
                        </div>
                        <div className="group mb-4" onFocus={() => setActiveStep(2)}>
                            <div className="group mb-4">
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
                                    <NoSsr>
                                        <FormControlLabel
                                            control={
                                                <Checkbox
                                                    checked={isDeliveryAddressEqualBillingAddress}
                                                    onChange={(e) => {
                                                        setFieldValue({ name: "isDeliveryAddressEqualBillingAddress", value: e.target.checked })
                                                }} />
                                            } />
                                        <span>{props.deliveryAddressEqualBillingAddressText}</span>
                                    </NoSsr>
                                </div>
                            </div>
                            {!isDeliveryAddressEqualBillingAddress &&
                                <div div className="group mb-4 mt-4">
                                    <h1 className="subtitle has-text-centered">{props.deliveryAddressTitle}</h1>
                                    <div className="field">
                                        <TextField
                                            id="deliveryAddressFullName"
                                            name="deliveryAddressFullName"
                                            value={deliveryAddressFullName}
                                            fullWidth={true}
                                            variant="standard"
                                            label={props.addressFullNameLabel}
                                            onChange={handleOnChange} />
                                    </div>
                                    <div className="field">
                                        <TextField
                                            id="deliveryAddressPhoneNumber"
                                            name="deliveryAddressPhoneNumber"
                                            value={deliveryAddressPhoneNumber}
                                            fullWidth={true}
                                            variant="standard"
                                            label={props.addressPhoneNumberLabel}
                                            onChange={handleOnChange} />
                                    </div>
                                    <div className="field">
                                        <TextField
                                            id="deliveryAddressStreet"
                                            name="deliveryAddressStreet"
                                            value={deliveryAddressStreet}
                                            fullWidth={true}
                                            variant="standard"
                                            label={props.addressStreetLabel}
                                            onChange={handleOnChange} />
                                    </div>
                                    <div className="field">
                                        <TextField
                                            id="deliveryAddressRegion"
                                            name="deliveryAddressRegion"
                                            value={deliveryAddressRegion}
                                            fullWidth={true}
                                            variant="standard"
                                            label={props.addressRegionLabel}
                                            onChange={handleOnChange} />
                                    </div>
                                    <div className="field">
                                        <TextField
                                            id="deliveryAddressPostalCode"
                                            name="deliveryAddressPostalCode"
                                            value={deliveryAddressPostalCode}
                                            fullWidth={true}
                                            variant="standard"
                                            label={props.addressPostalCodeLabel}
                                            onChange={handleOnChange} />
                                    </div>
                                    <div className="field">
                                        <TextField
                                            id="deliveryAddressCity"
                                            name="deliveryAddressCity"
                                            value={deliveryAddressCity}
                                            fullWidth={true}
                                            variant="standard"
                                            label={props.addressCityLabel}
                                            onChange={handleOnChange} />
                                    </div>
                                    <div className="field">
                                        <TextField
                                            id="deliveryAddressCountry"
                                            name="deliveryAddressCountry"
                                            value={deliveryAddressCountry}
                                            fullWidth={true}
                                            variant="standard"
                                            label={props.addressCountryLabel}
                                            onChange={handleOnChange} />
                                    </div>
                                </div>
                            }
                        </div>
                        <div className="field">
                            <NoSsr>
                                <FormControlLabel
                                    control={
                                        <Checkbox
                                            checked={acceptedTerms}
                                            onChange={(e) => {
                                                setFieldValue({ name: "acceptedTerms", value: e.target.checked });
                                            }} />
                                    } />
                                <span>{props.acceptTermsText} <a href={props.regulationsUrl} className="is-underlined" target="_blank">{props.regulations}</a>  &amp; <a href={props.privacyPolicyUrl} className="is-underlined" target="_blank">{props.privacyPolicy}</a></span>
                            </NoSsr>
                        </div>
                        <div className="is-flex is-justify-content-center">
                            <Button
                                type="submit"
                                variant="contained"
                                color="primary"
                                fullWidth={true}
                                disabled={state.isLoading || disable || isSended || !acceptedTerms}
                            >
                                {props.saveText}
                            </Button>
                        </div>
                    </form>
                </div>
            </div >
        </div >
    )
}

ApplicationForm.propTypes = {
    title: PropTypes.string.isRequired,
    subtitle: PropTypes.string.isRequired,
    steps: PropTypes.array,
    contactInformationTitle: PropTypes.string.isRequired,
    businessInformationTitle: PropTypes.string.isRequired,
    firstNameLabel: PropTypes.string.isRequired,
    lastNameLabel: PropTypes.string.isRequired,
    emailLabel: PropTypes.string.isRequired,
    phoneNumberLabel: PropTypes.string.isRequired,
    contactJobTitleLabel: PropTypes.string.isRequired,
    fieldRequiredErrorMessage: PropTypes.string.isRequired,
    yesLabel: PropTypes.string.isRequired,
    noLabel: PropTypes.string.isRequired,
    generalErrorMessage: PropTypes.string.isRequired,
    companyNameLabel: PropTypes.string.isRequired,
    companyAddressLabel: PropTypes.string.isRequired,
    companyCountryLabel: PropTypes.string.isRequired,
    companyCityLabel: PropTypes.string.isRequired,
    companyRegionLabel: PropTypes.string.isRequired,
    companyPostalCodeLabel: PropTypes.string.isRequired,
    saveText: PropTypes.string.isRequired,
    selectJobTitle: PropTypes.string.isRequired,
    signInUrl: PropTypes.string.isRequired,
    acceptTermsText: PropTypes.string.isRequired,
    privacyPolicyUrl: PropTypes.string.isRequired,
    regulationsUrl: PropTypes.string.isRequired,
    privacyPolicy: PropTypes.string.isRequired,
    regulations: PropTypes.string.isRequired
}

export default ApplicationForm;