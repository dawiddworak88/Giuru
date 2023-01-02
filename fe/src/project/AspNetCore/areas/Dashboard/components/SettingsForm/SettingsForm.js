import React, { useContext, useState, Fragment } from "react";
import { toast } from "react-toastify";
import { Context } from "../../../../../../shared/stores/Store";
import { Button, IconButton, InputAdornment, TextField } from "@mui/material";
import PropTypes from "prop-types";
import { FileCopyOutlined } from "@mui/icons-material";
import ClipboardHelper from "../../../../../../shared/helpers/globals/ClipboardHelper";

const SettingsForm = (props) => {
    const [state, dispatch] = useContext(Context);
    const [appSecret, setAppSecret] = useState(props.appSecret ? props.appSecret : null);
    const [name, setName] = useState(props.name ? props.name : null)

    const generateClientAppSecret = () => {
        console.log("asdasd")
        dispatch({ type: "SET_IS_LOADING", payload: true });

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" }
        };

        fetch(props.generateAppSecretUrl, requestOptions)
            .then(response => {
                dispatch({ type: "SET_IS_LOADING", payload: false });

                return response.json().then(jsonResponse => {
                    if (response.ok) {
                        toast.success(jsonResponse.message);
                        setAppSecret(jsonResponse.id);
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

    return (
        <div className="settings-form pl-5">
            <h1 className="subtitle is-size-3 mb-6">{props.accountSettings}</h1>
            <div className="form-group">
                <div className="settings-form-section">
                    <h2 className="settings-form-section__title">{props.accountData}</h2>
                </div>
                <div className="fields">
                    <div className="field">
                        <TextField 
                            id="name"
                            name="name"
                            label={props.nameLabel}
                            value={name}
                            variant="standard"
                            fullWidth={true}
                            InputProps={{
                                readOnly: true
                            }}
                        />
                    </div>
                    <div className="field">
                        <TextField 
                            id="email"
                            name="email"
                            label={props.emailLabel}
                            value={props.email}
                            variant="standard"
                            fullWidth={true}
                            InputProps={{
                                readOnly: true
                            }}
                        />
                    </div>
                    <div className="field">
                        <TextField 
                            id="phoneNumber"
                            name="phoneNumber"
                            label={props.phoneNumberLabel}
                            value={props.phoneNumber}
                            variant="standard"
                            fullWidth={true}
                            InputProps={{
                                readOnly: true
                            }}
                        />
                    </div>
                </div>
            </div>
            <div className="form-group">
                <div className="settings-form-section">
                    <h2 className="settings-form-section__title">{props.apiIdentifierTitle}</h2>
                    <p className="settings-form-section__description">{props.apiIdentifierDescription}</p>
                </div>
                <div className="fields">
                    <div className="field">
                        <TextField 
                            id="organisationId"
                            name="organisationId"
                            label={props.organisationLabel}
                            value={props.organisationId}
                            variant="standard"
                            fullWidth={true}
                            InputProps={{
                                readOnly: true,
                                endAdornment: (
                                    <InputAdornment position="end">
                                        <IconButton onClick={() => ClipboardHelper.copyToClipboard(props.organisationId)} aria-label={props.copyLabel} title={props.copyLabel}>
                                            <FileCopyOutlined />
                                        </IconButton>
                                    </InputAdornment>
                                )
                            }}
                        />
                    </div>
                    <div className="field">
                        <TextField 
                            id="appSecret"
                            name="appSecret"
                            label={props.appSecretLabel}
                            value={appSecret}
                            variant="standard"
                            fullWidth={true}
                            InputProps={{
                                readOnly: true,
                                endAdornment: appSecret ? (
                                    <InputAdornment position="end">
                                        <IconButton onClick={() => ClipboardHelper.copyToClipboard(appSecret)} aria-label={props.copyLabel} title={props.copyLabel}>
                                            <FileCopyOutlined />
                                        </IconButton>
                                    </InputAdornment>
                                ) : (
                                    <InputAdornment position="end">
                                        <Button onClick={generateClientAppSecret}>{props.generateText}</Button>
                                    </InputAdornment>
                                )
                            }}
                        />
                    </div>
                </div>
            </div>
        </div>
    )
}

SettingsForm.propTypes = {
    generalErrorMessage: PropTypes.string.isRequired,
    generateAppSecretUrl: PropTypes.string.isRequired,
    copyLabel: PropTypes.string,
    name: PropTypes.string.isRequired,
    nameLabel: PropTypes.string.isRequired,
    accountSettings: PropTypes.string.isRequired,
    accountData: PropTypes.string.isRequired,
    emailLabel: PropTypes.string.isRequired,
    email: PropTypes.string.isRequired,
    phoneNumber: PropTypes.string,
    phoneNumberLabel: PropTypes.string.isRequired,
    apiIdentifierTitle: PropTypes.string.isRequired,
    apiIdentifierDescription: PropTypes.string.isRequired,
    appSecretLabel: PropTypes.string.isRequired,
    appSecret: PropTypes.string,
    organisationId: PropTypes.string.isRequired,
    organisationLabel: PropTypes.string.isRequired,
    generateText: PropTypes.string
}

export default SettingsForm;