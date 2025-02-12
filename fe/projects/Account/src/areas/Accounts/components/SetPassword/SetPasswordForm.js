import React, { useContext, useState } from "react";
import PropTypes from "prop-types";
import { Context } from "../../../../shared/stores/Store";
import { TextField, Button, CircularProgress, NoSsr, FormControlLabel, Checkbox } from "@mui/material";
import useForm from "../../../../shared/helpers/forms/useForm";
import PasswordValidator from "../../../../shared/helpers/validators/PasswordValidator";
import { toast } from "react-toastify";
import NavigationHelper from "../../../../shared/helpers/globals/NavigationHelper";
import ToastHelper from "../../../../shared/helpers/globals/ToastHelper";

function SetPasswordForm(props) {
    const [state, dispatch] = useContext(Context);
    const [approvalsId, setApprovalsId] = useState([]);
    const [approvalCheckboxes, setApprovalCheckboxes] = useState(props.approvals.map((approval) => {
        return {id: approval.id, label: approval.description, name: approval.name, checked: false}
    }))

    const stateSchema = {
        id: { value: props.id ? props.id : null, error: "" },
        password: { value: null, error: "" },
        returnUrl: { value: props.returnUrl ? props.returnUrl : null }
    };

    const stateValidatorSchema = {
        password: {
            required: {
                isRequired: true,
                error: props.passwordRequiredErrorMessage
            },
            validator: {
                func: value => PasswordValidator.validateFormat(value),
                error: props.passwordFormatErrorMessage,
            },
        },
    };

    const onSubmitForm = (state) => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        const payload = {
            ...state,
            clientApprovals: approvalsId
        }

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" },
            body: JSON.stringify(payload)
        };

        fetch(props.submitUrl, requestOptions)
            .then(response => {
                return response.json().then(jsonResponse => {
                    if (response.ok) {
                        toast.success(props.passwordSetSuccessMessage);
                        setTimeout(() => {
                            NavigationHelper.redirect(jsonResponse.url)
                        }, 2000);
                    } else {
                        dispatch({ type: "SET_IS_LOADING", payload: false });
                        toast.error(ToastHelper.withLink(jsonResponse.message, jsonResponse.url, jsonResponse.urlLabel))
                    }
                })
            }).catch(() => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(props.generalErrorMessage);
            });
    };

    const {
        disable, values, errors, dirty, handleOnChange, handleOnSubmit
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm);

    const { id, password } = values;

    const checkboxOnChangeHandler = (id) => {
        setApprovalCheckboxes((prevCheckboxes) =>
        prevCheckboxes.map((checkbox) =>
            checkbox.id === id ? { ...checkbox, checked: !checkbox.checked } : checkbox 
        ));

        if (approvalsId.some((x) => x === id)) {
            setApprovalsId(approvalsId.filter(x => x !== id))
        }
        else {
            setApprovalsId(approvalsId.concat(id))
        }
    };

    return (
        <section className="section is-flex-centered set-password">
            <div>
                <form className="is-modern-form has-text-centered" onSubmit={handleOnSubmit} method="post">
                    <div className="columns is-align-items-center container is-justify-content-space-between">
                        <div className="column is-7 card p-6">
                            <div className="field">
                                <h1 className="title">{props.marketingApprovalHeader}</h1>
                                <p className="subtitle">{props.marketingApprovalText}</p>
                            </div>
                            {approvalCheckboxes && approvalCheckboxes.length > 0 && (
                                <div className="field">
                                    {approvalCheckboxes.map((checkbox) => {
                                        return (
                                            <div className="is-flex mb-5" key={checkbox.id}>
                                                <NoSsr>
                                                    <FormControlLabel
                                                        control={
                                                            <Checkbox
                                                                onChange={() => checkboxOnChangeHandler(checkbox.id)}
                                                                checked={checkbox.checked}
                                                                id={checkbox.id}
                                                                name={checkbox.name}
                                                                color="secondary"/>
                                                        }
                                                    />
                                                </NoSsr>
                                                <div className="has-text-left">
                                                    {checkbox.label}
                                                </div>
                                            </div>
                                        )
                                    })}
                                </div>
                            )}
                            <div className="field">
                                {props.personalDataAdministratorText} <a href={props.privacyPolicyUrl} className="is-underlined" target="_blank">{props.privacyPolicy}</a>
                            </div>
                        </div>
                        <div className="column is-4">
                            <input type="hidden" name="id" value={id} />
                            <div>
                                <h1 className="subtitle is-4">{props.setPasswordText}</h1>
                            </div>
                            <div className="field">
                                <TextField
                                    id="password"
                                    name="password"
                                    type="password"
                                    variant="standard"
                                    label={props.passwordLabel}
                                    fullWidth={true}
                                    value={password}
                                    onChange={handleOnChange}
                                    helperText={dirty.password ? errors.password : ""}
                                    error={(errors.password.length > 0) && dirty.password} />
                            </div>
                            <div className="field">
                                <Button
                                    type="submit"
                                    variant="contained"
                                    color="primary"
                                    disabled={state.isLoading || disable}
                                    fullWidth={true}>
                                    {props.setPasswordText}
                                </Button>
                            </div>
                        </div>
                    </div>
                </form>
            </div>
            {state.isLoading && <CircularProgress className="progressBar" />}
        </section>
    );
}

SetPasswordForm.propTypes = {
    generalErrorMessage: PropTypes.string.isRequired,
    passwordSetSuccessMessage: PropTypes.string.isRequired,
    passwordRequiredErrorMessage: PropTypes.string.isRequired,
    passwordFormatErrorMessage: PropTypes.string.isRequired,
    returnUrl: PropTypes.string.isRequired,
    submitUrl: PropTypes.string.isRequired,
};

export default SetPasswordForm;
