import React from "react";
import {
    Stepper, Step, StepLabel, StepContent, TextField
} from "@material-ui/core";

const RegisterForm = (props) => {
    return (
        <div className="container">
            <div className="columns register-form">
                <div className="column">
                    <div className="p-6">
                        <h1 className="title">Partner with Eltap</h1>
                        <p>Thanks for your interest in selling your products with Wayfair! We're excited to learn more about you and your business</p>
                        <Stepper activeStep={0} orientation="vertical">
                            <Step>
                                <StepLabel>Informacje Kontaktowe</StepLabel>
                                <StepContent>Your primary contact will be Wayfair's point person for your business</StepContent>
                            </Step>
                            <Step>
                                <StepLabel>Informacje Biznesowe</StepLabel>
                                <StepContent>Your primary contact will be Wayfair's point person for your business</StepContent>
                            </Step>
                            <Step>
                                <StepLabel>Informacje Logistyczne</StepLabel>
                                <StepContent>Your primary contact will be Wayfair's point person for your business</StepContent>
                            </Step>
                        </Stepper>
                    </div>
                </div>
                <div className="column">
                    <div className="p-6">
                        <h1 className="subtitle has-text-centered">1. Informacje Kontaktowe</h1>
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
                </div>
            </div>
        </div>
    )
}

export default RegisterForm;