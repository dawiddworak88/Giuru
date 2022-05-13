import React, { useState } from "react";
import PropTypes from "prop-types";
import {
    Stepper, Step, StepLabel, StepContent, TextField, Button,
    FormControl, InputLabel, Select, MenuItem
} from "@material-ui/core";
import useForm from "../../../../../../shared/helpers/forms/useForm";
import EmailValidator from "../../../../../../shared/helpers/validators/EmailValidator";

const RegisterForm = (props) => {
    const [activeStep, setActiveStep] = useState(0);
    const stateSchema = {
       firstName: { value: null, error: "" },
       lastName: { value: null, error: "" },
       email: { value: null, error: "" },
       phoneNumber: { value: null, error: "" },
       contactJobTitle: { value: null, error: "" },
       postalCode: { value: null, error: "" }
    };

    const stateValidatorSchema = {
        firstName: {
            required: {
                isRequired: true,
                error: props.firstNameRequiredErrorMessage
            }
        },
        lastName: {
            required: {
                isRequired: true,
                error: props.lastNameRequiredErrorMessage
            }
        },
        email: {
            required: {
                isRequired: true,
                error: props.emailRequiredErrorMessage
            },
            validator: {
                func: value => EmailValidator.validateFormat(value),
                error: props.emailFormatErrorMessage
            }
        },
        contactJobTitle: {
            required: {
                isRequired: true,
                error: props.contactJobTitleRequiredErrorMessage
            }
        },
        postalCode: {
            required: {
                isRequired: true,
                error: props.postalCodeRequiredErrorMessage
            }
        }
    };

    const onSubmitForm = () => {

    }

    const {
        values, errors, dirty, disable,
        setFieldValue, handleOnChange, handleOnSubmit
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm, !props.id);

    const { firstName, lastName, email, phoneNumber, contactJobTitle, postalCode } = values;
    
    return (
        <div className="container register-form">
            <div className="columns">
                <div className="column">
                    <div className="register-form__stepper p-6">
                        <h1 className="title">{props.title}</h1>
                        <p className="subtitle mb-2">{props.subtitle}</p>
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
                    <div className="register-form__groups p-6">
                        <div className="group mb-6" onFocus={() => setActiveStep(0)}>
                            <h1 className="subtitle has-text-centered">{props.contactInformationTitle}</h1>
                            <div className="field">
                                <TextField
                                    id="firstName" 
                                    name="firstName"
                                    value={firstName}
                                    fullWidth={true}
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
                                    label={props.lastNameLabel}
                                    onChange={handleOnChange} 
                                    helperText={dirty.lastName ? errors.lastName : ""} 
                                    error={(errors.lastName.length > 0) && dirty.lastName} />
                            </div>
                            <div className="field">
                                <FormControl fullWidth={true} error={(errors.contactJobTitle.length > 0) && dirty.contactJobTitle}>
                                    <InputLabel id="contactJobTitle-label">{props.contactJobTitleLabel}</InputLabel>
                                    <Select
                                        labelId="contactJobTitle-label"
                                        id="contactJobTitle"
                                        name="contactJobTitle"
                                        value={contactJobTitle}
                                        onChange={handleOnChange}>
                                            <MenuItem value={0}>EXAMPLE</MenuItem>
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
                                    label={props.phoneNumberLabel}
                                />
                            </div>
                        </div>
                        <div className="group mb-6" onFocus={() => setActiveStep(1)}>
                            <h1 className="subtitle has-text-centered">{props.businessInformationTitle}</h1>
                            <div className="field">
                                <TextField
                                    id="compnayName" 
                                    name="compnayName"
                                    fullWidth={true}
                                    label="Nazwa"
                                />
                            </div>
                            <div className="field">
                                <TextField
                                    id="adres"
                                    name="adres"
                                    fullWidth={true}
                                    label="Adres"
                                />
                            </div>
                            <div className="field">
                                <TextField
                                    id="city"
                                    name="city"
                                    fullWidth={true}
                                    label="City/Town"
                                />
                            </div>
                            <div className="field">
                                <TextField
                                    id="region"
                                    name="region"
                                    fullWidth={true}
                                    label="region"
                                />
                            </div>
                            <div className="field">
                                <TextField
                                    id="postalCode"
                                    name="postalCode"
                                    fullWidth={true}
                                    value={postalCode}
                                    label={props.postalCodeLabel}
                                    onChange={handleOnChange} 
                                    helperText={dirty.postalCode ? errors.postalCode : ""} 
                                    error={(errors.postalCode.length > 0) && dirty.postalCode} />
                            </div>
                        </div>
                        <div className="group mb-6" onFocus={() => setActiveStep(2)}>
                            <h1 className="subtitle has-text-centered">{props.logisticalInformationTitle}</h1>
                            <div className="field">
                                <TextField
                                    id="firstname" 
                                    name="firstname"
                                    fullWidth={true}
                                    label="Imię"
                                />
                            </div>
                            <div className="field">
                                <TextField
                                    id="lastname"
                                    name="lastname"
                                    fullWidth={true}
                                    label="Nazwisko"
                                />
                            </div>
                            <div className="field">
                                <TextField
                                    id="email"
                                    name="email"
                                    fullWidth={true}
                                    label="Email"
                                />
                            </div>
                            <div className="field">
                                <TextField
                                    id="telephone"
                                    name="telephone"
                                    fullWidth={true}
                                    label="Numer telefonu"
                                />
                            </div>
                        </div>
                        <div className="is-flex is-justify-content-center">
                            <Button type="submit" variant="contained" color="primary" fullWidth={true}>
                                Aplikuj
                            </Button>
                        </div>
                    </div>
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
    logisticalInformationTitle: PropTypes.string.isRequired,
    firstNameLabel: PropTypes.string.isRequired,
    lastNameLabel: PropTypes.string.isRequired,
    emailLabel: PropTypes.string.isRequired,
    phoneNumberLabel: PropTypes.string.isRequired,
    firstNameRequiredErrorMessage: PropTypes.string.isRequired,
    lastNameRequiredErrorMessage: PropTypes.string.isRequired,
    emailRequiredErrorMessage: PropTypes.string.isRequired,
    emailFormatErrorMessage: PropTypes.string.isRequired,
    contactJobTitleRequiredErrorMessage: PropTypes.string.isRequired,
    contactJobTitleLabel: PropTypes.string.isRequired,
    postalCodeRequiredErrorMessage: PropTypes.string.isRequired,
    postalCodeLabel: PropTypes.string.isRequired
}

export default RegisterForm;