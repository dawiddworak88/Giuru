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

const RegisterForm = (props) => {
    const [state, dispatch] = useContext(Context);
    const [isSended, setIsSended] = useState(false);
    const [activeStep, setActiveStep] = useState(0);
    const stateSchema = {
       firstName: { value: "", error: "" },
       lastName: { value: "", error: "" },
       email: { value: "", error: "" },
       phoneNumber: { value: "", error: "" },
       contactJobTitle: { value: "", error: "" },
       companyName: { value: "", error: "" },
       companyAddress: { value: "", error: "" },
       companyCity: { value: "", error: "" },
       companyRegion: { value: "", error: "" },
       companyPostalCode: { value: "", error: "" },
       companyCountry: { value: "", error: "" },
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
        }
    };

    const onSubmitForm = (state) => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" },
            body: JSON.stringify(state)
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
        firstName, lastName, email, phoneNumber, contactJobTitle, companyName, 
        companyAddress, companyCity, companyCountry, companyRegion, companyPostalCode,
        acceptedTerms
    } = values;
    
    return (
        <div className="container register-form">
            <div className="columns">
                <div className="column">
                    <div className="register-form__stepper p-6">
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
                    <form className="register-form__groups p-6" onSubmit={handleOnSubmit}>
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
                        </div>
                        <div className="group mb-4" onFocus={() => setActiveStep(1)}>
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
                                    error={(errors.companyCity.length > 0) && dirty.companyCity}/>
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
                        <div className="field">
                            <NoSsr>
                                <FormControlLabel 
                                    control={
                                        <Checkbox 
                                            checked={acceptedTerms}
                                            onChange={(e) => {
                                                setFieldValue({name: "acceptedTerms", value: e.target.checked});
                                            }}/>
                                    }/>
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
            </div>
        </div>
    )
}

RegisterForm.propTypes = {
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

export default RegisterForm;