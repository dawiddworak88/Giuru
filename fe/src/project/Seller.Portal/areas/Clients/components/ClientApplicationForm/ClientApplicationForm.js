import React, { useContext } from "react";
import PropTypes from "prop-types";
import NavigationHelper from "../../../../../../shared/helpers/globals/NavigationHelper";
import { 
    TextField, Button, InputLabel, CircularProgress,
    FormControl, Select, MenuItem, FormHelperText
} from "@mui/material";
import { toast } from "react-toastify";
import { Context } from "../../../../../../shared/stores/Store";
import useForm from "../../../../../../shared/helpers/forms/useForm";
import EmailValidator from "../../../../../../shared/helpers/validators/EmailValidator";

const ClientApplicationForm = (props) => {
    const [state, dispatch] = useContext(Context);
    const stateSchema = {
        id: { value: props.id ? props.id : null },
        firstName: { value: props.firstName ? props.firstName : "", error: "" },
        lastName: { value: props.lastName ? props.lastName : "", error: "" },
        email: { value: props.email ? props.email : "", error: "" },
        phoneNumber: { value: props.phoneNumber ? props.phoneNumber : "", error: "" },
        contactJobTitle: { value: props.contactJobTitle ? props.contactJobTitle : "", error: "" },
        companyName: { value: props.companyName ? props.companyName : "", error: "" },
        companyAddress: { value: props.companyAddress ? props.companyAddress : "", error: "" },
        companyCity: { value: props.companyCity ? props.companyCity : "", error: "" },
        companyRegion: { value: props.companyRegion ? props.companyRegion : "", error: "" },
        companyPostalCode: { value: props.companyPostalCode ? props.companyPostalCode : "", error: "" },
        companyCountry: { value: props.companyCountry ? props.companyCountry : "", error: "" }
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
        console.log("sadasd")
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
        values, errors, dirty, disable,
        handleOnChange, handleOnSubmit
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm, !props.id);

    const { 
        id, firstName, lastName, email, phoneNumber, contactJobTitle, companyAddress, companyCity, 
        companyCountry, companyName, companyPostalCode, companyRegion 
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
                                <TextField 
                                    id="email" 
                                    name="email" 
                                    label={props.emailLabel} 
                                    fullWidth={true}
                                    value={email} 
                                    onChange={handleOnChange}
                                    variant="standard"
                                    error={(errors.email.length > 0) && dirty.email}
                                    helperText={dirty.email ? errors.email : ""}/>
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
                                    id="companyAddress" 
                                    name="companyAddress" 
                                    label={props.addressLabel} 
                                    fullWidth={true}
                                    value={companyAddress} 
                                    onChange={handleOnChange}
                                    variant="standard"
                                    error={(errors.companyAddress.length > 0) && dirty.companyAddress}
                                    helperText={dirty.companyAddress ? errors.companyAddress : ""} />
                            </div>
                            <div className="field">
                                <TextField 
                                    id="companyCountry" 
                                    name="companyCountry" 
                                    label={props.countryLabel} 
                                    fullWidth={true}
                                    value={companyCountry} 
                                    onChange={handleOnChange}
                                    variant="standard" 
                                    error={(errors.companyCountry.length > 0) && dirty.companyCountry}
                                    helperText={dirty.companyCountry ? errors.companyCountry : ""}/>
                            </div>
                            <div className="field">
                                <TextField 
                                    id="companyCity" 
                                    name="companyCity" 
                                    label={props.cityLabel} 
                                    fullWidth={true}
                                    value={companyCity} 
                                    onChange={handleOnChange}
                                    variant="standard"
                                    error={(errors.companyCity.length > 0) && dirty.companyCity}
                                    helperText={dirty.companyCity ? errors.companyCity : ""} />
                            </div>
                            <div className="field">
                                <TextField 
                                    id="companyRegion" 
                                    name="companyRegion" 
                                    label={props.regionLabel} 
                                    fullWidth={true}
                                    value={companyRegion} 
                                    onChange={handleOnChange}
                                    variant="standard" 
                                    error={(errors.companyRegion.length > 0) && dirty.companyRegion}
                                    helperText={dirty.companyRegion ? errors.companyRegion : ""}/>
                            </div>
                            <div className="field">
                                <TextField 
                                    id="companyPostalCode" 
                                    name="companyPostalCode" 
                                    label={props.postalCodeLabel} 
                                    fullWidth={true}
                                    value={companyPostalCode} 
                                    onChange={handleOnChange}
                                    variant="standard" 
                                    error={(errors.companyPostalCode.length > 0) && dirty.companyPostalCode}
                                    helperText={dirty.companyPostalCode ? errors.companyPostalCode : ""}/>
                            </div>
                        </div>
                        <div className="field">
                            <Button 
                                type="submit" 
                                variant="contained"
                                color="primary"
                                disabled={state.isLoading || disable}>
                                {props.saveText}
                            </Button>
                            <Button
                                className="ml-2"
                                type="text" 
                                variant="contained" 
                                color="secondary"
                                onClick={(e) => {
                                    e.preventDefault();
                                    NavigationHelper.redirect(props.clientsApplicationsUrl);
                                }}>
                                {props.backToClientsApplicationsText}
                            </Button>
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
    companyNameLabel: PropTypes.string.isRequired,
    phoneNumberLabel: PropTypes.string.isRequired,
    emailLabel: PropTypes.string.isRequired,
    contactJobTitleLabel: PropTypes.string.isRequired,
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