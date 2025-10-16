import React, { useContext } from "react";
import PropTypes from "prop-types";
import { 
    TextField, Button, InputLabel, CircularProgress, Autocomplete
} from "@mui/material";
import { toast } from "react-toastify";
import { Context } from "../../../../shared/stores/Store";
import useForm from "../../../../shared/helpers/forms/useForm";
import AuthenticationHelper from "../../../../shared/helpers/globals/AuthenticationHelper";

const ClientAddressForm = (props) => {
    const [state, dispatch] = useContext(Context);
    const stateSchema = {
        id: { value: props.id ? props.id : null },
        client: { value: props.clientId ? props.clients.find((item) => item.id === props.clientId) : null, error: "" },
        company: { value: props.company ? props.company : "" },
        firstName: { value: props.firstName ? props.firstName : "" },
        lastName: { value: props.lastName ? props.lastName : "" },
        phoneNumber: { value: props.phoneNumber ? props.phoneNumber : "", error: "" },
        street: { value: props.street ? props.street : "", error: "" },
        region: { value: props.region ? props.region : "", error: "" },
        postCode: { value: props.postCode ? props.postCode : "", error: "" },
        city: { value: props.city ? props.city : "", error: "" },
        country: { value: props.countryId ? props.countries.find((item) => item.id === props.countryId) : null, error: "" }
    };

    const stateValidatorSchema = {
        phoneNumber: {
            required: {
                isRequired: true,
                error: props.fieldRequiredErrorMessage
            }
        },
        street: {
            required: {
                isRequired: true,
                error: props.fieldRequiredErrorMessage
            }
        },
        region: {
            required: {
                isRequired: true,
                error: props.fieldRequiredErrorMessage
            }
        },
        postCode: {
            required: {
                isRequired: true,
                error: props.fieldRequiredErrorMessage
            }
        },
        city: {
            required: {
                isRequired: true,
                error: props.fieldRequiredErrorMessage
            }
        },
    };

    const onSubmitForm = (state) => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        const requestPayload = {
            id: state.id,
            clientId: state.client ? state.client.id : null,
            company: state.company,
            firstName: state.firstName,
            lastName: state.lastName,
            phoneNumber: state.phoneNumber,
            street: state.street,
            region: state.region,
            city: state.city,
            postCode: state.postCode,
            countryId: state.country ? state.country.id : null
        }

        const requestOptions = {
            method: "POST",
            headers: { 
                "Content-Type": "application/json", 
                "X-Requested-With": "XMLHttpRequest" 
            },
            body: JSON.stringify(requestPayload)
        };

        fetch(props.saveUrl, requestOptions)
            .then(response => {
                dispatch({ type: "SET_IS_LOADING", payload: false });

                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {
                    if (response.ok) {
                        setFieldValue({name: "id", value: jsonResponse.id});
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
        handleOnChange, handleOnSubmit, setFieldValue
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm, !props.id);

    const { id, company, firstName, lastName, phoneNumber, street, region, postCode, city, country, client } = values;

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
                            <Autocomplete
                                id="client"
                                name="client"
                                options={props.clients}
                                getOptionLabel={(option) => option.name}
                                fullWidth={true}
                                value={client}
                                variant="standard"
                                onChange={(event, newValue) => {
                                    setFieldValue({name: "client", value: newValue});
                                }}
                                autoComplete
                                renderInput={(params) => (
                                    <TextField 
                                        {...params} 
                                        label={props.clientLabel} 
                                        variant="standard"
                                        margin="normal"
                                        helperText={dirty.client && !client ? props.fieldRequiredErrorMessage : ""} 
                                        error={dirty.client && !client}/>
                                )} />
                        </div>   
                        <div className="field">
                            <TextField 
                                id="company" 
                                name="company" 
                                label={props.companyLabel} 
                                fullWidth={true}
                                value={company} 
                                variant="standard"
                                onChange={handleOnChange} />
                        </div>        
                        <div className="field">
                            <TextField 
                                id="firstName" 
                                name="firstName" 
                                label={props.firstNameLabel} 
                                fullWidth={true}
                                value={firstName} 
                                variant="standard"
                                onChange={handleOnChange} />
                        </div>
                        <div className="field">
                            <TextField 
                                id="lastName" 
                                name="lastName" 
                                label={props.lastNameLabel} 
                                fullWidth={true}
                                value={lastName} 
                                variant="standard"
                                onChange={handleOnChange} />
                        </div>
                        <div className="field">
                            <TextField 
                                id="phoneNumber" 
                                name="phoneNumber" 
                                label={props.phoneNumberLabel} 
                                fullWidth={true}
                                value={phoneNumber} 
                                variant="standard"
                                onChange={handleOnChange} 
                                helperText={dirty.phoneNumber ? errors.phoneNumber : ""} 
                                error={(errors.phoneNumber.length > 0) && dirty.phoneNumber}/>
                        </div>
                        <div className="field">
                            <TextField 
                                id="street" 
                                name="street" 
                                label={props.streetLabel} 
                                fullWidth={true}
                                value={street} 
                                variant="standard"
                                onChange={handleOnChange} 
                                helperText={dirty.street ? errors.street : ""} 
                                error={(errors.street.length > 0) && dirty.street}/>
                        </div>
                        <div className="field">
                            <TextField 
                                id="region" 
                                name="region" 
                                label={props.regionLabel} 
                                fullWidth={true}
                                value={region} 
                                variant="standard"
                                onChange={handleOnChange} 
                                helperText={dirty.region ? errors.region : ""} 
                                error={(errors.region.length > 0) && dirty.region}/>
                        </div>
                        <div className="field">
                            <TextField 
                                id="postCode" 
                                name="postCode" 
                                label={props.postCodeLabel} 
                                fullWidth={true}
                                value={postCode} 
                                variant="standard"
                                onChange={handleOnChange} 
                                helperText={dirty.postCode ? errors.postCode : ""} 
                                error={(errors.postCode.length > 0) && dirty.postCode}/>
                        </div>
                        <div className="field">
                            <TextField 
                                id="city" 
                                name="city" 
                                label={props.cityLabel} 
                                fullWidth={true}
                                value={city} 
                                variant="standard"
                                onChange={handleOnChange} 
                                helperText={dirty.city ? errors.city : ""} 
                                error={(errors.city.length > 0) && dirty.city}/>
                        </div>
                        <div className="field">
                            <Autocomplete
                                id="country"
                                name="country"
                                options={props.countries}
                                getOptionLabel={(option) => option.name}
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
                                        margin="normal"
                                        helperText={dirty.country && !country ? props.fieldRequiredErrorMessage : ""} 
                                        error={dirty.country && !country}/>
                                )} />
                        </div>
                        <div className="field ">
                            <Button 
                                type="submit" 
                                variant="contained" 
                                color="primary" 
                                disabled={state.isLoading || !client || !country || disable}>
                                {props.saveText}
                            </Button>
                            <a href={props.clientAddressesUrl} className="ml-2 button is-text">{props.navigateToClientAddresses}</a>
                        </div>
                    </form>
                    {state.isLoading && <CircularProgress className="progressBar" />}
                </div>
            </div>
        </section>
    )
}

ClientAddressForm.propTypes = {
    id: PropTypes.string,
    saveUrl: PropTypes.string.isRequired,
    title: PropTypes.string.isRequired,
    idLabel: PropTypes.string.isRequired,
    saveText: PropTypes.string.isRequired,
    navigateToClientAddresses: PropTypes.string.isRequired,
    generalErrorMessage: PropTypes.string.isRequired,
    fieldRequiredErrorMessage: PropTypes.string.isRequired
}

export default ClientAddressForm;