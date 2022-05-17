import React, { useState, useContext } from "react";
import PropTypes from "prop-types";
import { toast } from "react-toastify";
import { Context } from "../../../../../../shared/stores/Store"
import {
    Stepper, Step, StepLabel, StepContent, TextField, Button, FormHelperText,
    FormControl, InputLabel, Select, MenuItem, RadioGroup, FormControlLabel, Radio, FormLabel
} from "@material-ui/core";
import useForm from "../../../../../../shared/helpers/forms/useForm";
import EmailValidator from "../../../../../../shared/helpers/validators/EmailValidator";
import { RadioButtonChecked, RadioButtonUnchecked } from "@material-ui/icons";

const RegisterForm = (props) => {
    const [state, dispatch] = useContext(Context);
    const [activeStep, setActiveStep] = useState(0);
    const stateSchema = {
       firstName: { value: null, error: "" },
       lastName: { value: null, error: "" },
       email: { value: null, error: "" },
       phoneNumber: { value: null, error: "" },
       contactJobTitle: { value: null, error: "" },
       companyName: { value: null, error: "" },
       companyAddress: { value: null, error: "" },
       companyCity: { value: null, error: "" },
       companyRegion: { value: null, error: "" },
       companyPostalCode: { value: null, error: "" },
       companyCountry: { value: null, error: "" },
       directlyShip: { value: null, error: "" },
       acceptReturns: { value: null, error: "" },
       onlineRetailers: { value: null, error: "" }
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
        },
        onlineRetailers: {
            required: {
                isRequired: true,
                error: props.fieldRequiredErrorMessage
            }
        },
        directlyShip: {
            required: {
                isRequired: true,
                error: props.fieldRequiredErrorMessage
            }
        },
        acceptReturns: {
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

                return response.json().then(jsonResponse => {
                    if (response.ok) {
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

    const {
        values, errors, dirty, disable, handleOnChange, handleOnSubmit
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm);

    const { 
        firstName, lastName, email, phoneNumber, contactJobTitle, companyName, companyAddress, companyCity, 
        companyCountry, companyRegion, companyPostalCode, directlyShip, acceptReturns, onlineRetailers 
    } = values;
    
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
                    <form className="register-form__groups p-6" onSubmit={handleOnSubmit}>
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
                                        <MenuItem key={0} value="">{props.selectJobTitle}</MenuItem>
                                        {props.contactJobTitles && props.contactJobTitles.map((title, index) => {
                                            return (
                                                <MenuItem key={index} value={title.name}>{title.name}</MenuItem>
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
                                    onChange={handleOnChange}
                                    helperText={dirty.phoneNumber ? errors.phoneNumber : ""} 
                                    error={(errors.phoneNumber.length > 0) && dirty.phoneNumber} />
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
                                    label={props.companyPostalCodeLabel}
                                    onChange={handleOnChange} 
                                    helperText={dirty.companyPostalCode ? errors.companyPostalCode : ""} 
                                    error={(errors.companyPostalCode.length > 0) && dirty.companyPostalCode} />
                            </div>
                        </div>
                        <div className="group mb-6" onFocus={() => setActiveStep(2)}>
                            <h1 className="subtitle has-text-centered">{props.logisticalInformationTitle}</h1>
                            <div className="field">
                                <FormControl component="fieldset" fullWidth={true} error={(errors.directlyShip.length > 0) && dirty.directlyShip}>
                                    <FormLabel>{props.directlyShipLabel}</FormLabel>
                                    <RadioGroup name="directlyShip" value={directlyShip} onChange={handleOnChange}>
                                        <FormControlLabel 
                                            value={props.yesLabel} 
                                            label={props.yesLabel}
                                            control={
                                                <Radio 
                                                    inputProps={{hidden: true}}
                                                    icon={<RadioButtonUnchecked />}
                                                    checkedIcon={<RadioButtonChecked />} 
                                                />}  
                                            />
                                        <FormControlLabel 
                                            label={props.noLabel}
                                            value={props.noLabel} 
                                            control={
                                                <Radio 
                                                    inputProps={{hidden: true}} 
                                                    icon={<RadioButtonUnchecked />}
                                                    checkedIcon={<RadioButtonChecked />} 
                                                />} 
                                            />
                                    </RadioGroup>
                                    {errors.directlyShip && dirty.directlyShip && (
                                        <FormHelperText>{errors.directlyShip}</FormHelperText>
                                    )}
                                </FormControl>
                            </div>
                            <div className="field">
                                <FormControl component="fieldset" fullWidth={true} error={(errors.acceptReturns.length > 0) && dirty.acceptReturns}>
                                    <FormLabel>{props.acceptReturnsLabel}</FormLabel>
                                    <RadioGroup name="acceptReturns" value={acceptReturns} onChange={handleOnChange}>
                                        <FormControlLabel 
                                            value={props.yesLabel} 
                                            label={props.yesLabel}
                                            control={
                                                <Radio 
                                                    inputProps={{hidden: true}}
                                                    icon={<RadioButtonUnchecked />}
                                                    checkedIcon={<RadioButtonChecked />} 
                                                />}  
                                            />
                                        <FormControlLabel 
                                            label={props.noLabel}
                                            value={props.noLabel} 
                                            control={
                                                <Radio 
                                                    inputProps={{hidden: true}} 
                                                    icon={<RadioButtonUnchecked />}
                                                    checkedIcon={<RadioButtonChecked />} 
                                                />} 
                                            />
                                    </RadioGroup>
                                    {errors.acceptReturns && dirty.acceptReturns && (
                                        <FormHelperText>{errors.acceptReturns}</FormHelperText>
                                    )}
                                </FormControl>
                            </div>
                            <div className="field">
                                <TextField
                                    id="onlineRetailers"
                                    name="onlineRetailers"
                                    fullWidth={true}
                                    value={onlineRetailers}
                                    label={props.onlineRetailersLabel}
                                    onChange={handleOnChange} 
                                    helperText={dirty.onlineRetailers ? errors.onlineRetailers : ""} 
                                    error={(errors.onlineRetailers.length > 0) && dirty.onlineRetailers} />
                            </div>
                        </div>
                        <div className="is-flex is-justify-content-center">
                            <Button 
                                type="submit" 
                                variant="contained" 
                                color="primary" 
                                fullWidth={true}
                                disabled={state.isLoading || disable}
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
    logisticalInformationTitle: PropTypes.string.isRequired,
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
    onlineRetailersLabel: PropTypes.string.isRequired,
    acceptRetunsLabel: PropTypes.string.isRequired,
    directlyShipLabel: PropTypes.string.isRequired,
    saveText: PropTypes.string.isRequired,
    selectJobTitle: PropTypes.string.isRequired
}

export default RegisterForm;