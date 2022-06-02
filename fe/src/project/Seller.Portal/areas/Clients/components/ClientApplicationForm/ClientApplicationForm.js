import React, { useContext, useState } from "react";
import PropTypes from "prop-types";
import NavigationHelper from "../../../../../../shared/helpers/globals/NavigationHelper";
import { 
    TextField, Button, FormControl, InputLabel, 
    Select, MenuItem, FormHelperText 
} from "@mui/material";

const ClientApplicationForm = (props) => {
    return (
        <section className="section section-small-padding product client-form">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div className="columns is-desktop">
                <div className="column is-half">
                    {id &&
                        <div className="field">
                            <InputLabel id="id-label">{props.idLabel} {id}</InputLabel>
                        </div>
                    }
                    <div className="group">
                        <div className="field">
                            <TextField 
                                id="name" 
                                name="name" 
                                label={props.firstNameLabel} 
                                fullWidth={true}
                                value={props.firstName} 
                                variant="standard" />
                        </div>
                        <div className="field">
                            <TextField 
                                id="name" 
                                name="name" 
                                label={props.lastNameLabel} 
                                fullWidth={true}
                                value={props.lastName} 
                                variant="standard" />
                        </div>
                        <div className="field">
                            <TextField 
                                id="name" 
                                name="name" 
                                label={props.contactJobTitleLabel} 
                                fullWidth={true}
                                value={props.contactJobTitle} 
                                variant="standard" />
                        </div>
                        <div className="field">
                            <TextField 
                                id="name" 
                                name="name" 
                                label={props.emailLabel} 
                                fullWidth={true}
                                value={props.email} 
                                variant="standard" />
                        </div>
                        <div className="field">
                            <TextField 
                                id="name" 
                                name="name" 
                                label={props.phoneNumberLabel} 
                                fullWidth={true}
                                value={props.phoneNumber} 
                                variant="standard" />
                        </div>
                    </div>
                    <div className="group">
                        <div className="field">
                            <TextField 
                                id="name" 
                                name="name" 
                                label={props.companyNameLabel} 
                                fullWidth={true}
                                value={props.companyName} 
                                variant="standard" />
                        </div>
                        <div className="field">
                            <TextField 
                                id="name" 
                                name="name" 
                                label={props.companyAddressLabel} 
                                fullWidth={true}
                                value={props.companyAddress} 
                                variant="standard" />
                        </div>
                        <div className="field">
                            <TextField 
                                id="name" 
                                name="name" 
                                label={props.companyCountryLabel} 
                                fullWidth={true}
                                value={props.companyCountry} 
                                variant="standard" />
                        </div>
                        <div className="field">
                            <TextField 
                                id="name" 
                                name="name" 
                                label={props.companyCityLabel} 
                                fullWidth={true}
                                value={props.companyCity} 
                                variant="standard" />
                        </div>
                        <div className="field">
                            <TextField 
                                id="name" 
                                name="name" 
                                label={props.companyRegionLabel} 
                                fullWidth={true}
                                value={props.companyRegion} 
                                variant="standard" />
                        </div>
                        <div className="field">
                            <TextField 
                                id="name" 
                                name="name" 
                                label={props.companyPostalCodeLabel} 
                                fullWidth={true}
                                value={props.companyPostalCode} 
                                variant="standard" />
                        </div>
                    </div>
                    <div className="field">
                        <Button 
                            type="text" 
                            variant="contained" 
                            color="primary"
                            onClick={() => {
                                NavigationHelper.redirect(props.clientsApplicationsUrl)
                            }}>
                            {props.backToClientsApplicationsText}
                        </Button>
                    </div>
                </div>
            </div>
        </section>
    );
}

ClientApplicationForm.propTypes = {
    title: PropTypes.string.isRequired,
    idLabel: PropTypes.string,
    backToClientsApplicationsText: PropTypes.string.isRequired
};

export default ClientApplicationForm;