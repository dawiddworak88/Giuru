import React, { useContext } from "react";
import { toast } from "react-toastify";
import PropTypes from "prop-types";
import { Context } from "../../../../shared/stores/Store";
import useForm from "../../../../shared/helpers/forms/useForm";
import AuthenticationHelper from "../../../../shared/helpers/globals/AuthenticationHelper";
import EmailValidator from "../../../../shared/helpers/validators/EmailValidator";
import { 
    TextField, InputLabel, Button, CircularProgress,
    NoSsr, FormControlLabel, Switch
} from "@mui/material";
import moment from "moment";

const ClientTeamMemberForm = (props) => {
    const [state, dispatch] = useContext(Context);
    
    const stateSchema = {
        id: { value: props.id ? props.id : null },
        email: { value: props.email ? props.email : "", error: "" },
        firstName: { value: props.firstName ? props.firstName : "", error: "" },
        lastName: { value: props.lastName ? props.lastName : "", error: "" },
        isDisabled: { value: props.isDisabled ? props.isDisabled : false },
        teamMemberApprovalIds: { value: props.teamMemberApprovals ? props.teamMemberApprovals.filter(x => x.isApproved).map(x => x.id) : [] }
    }

    const stateValidatorSchema = {
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
        }
    }

    const onSubmitForm = (state) => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" },
            body: JSON.stringify(state)
        };

        fetch(props.saveUrl, requestOptions)
            .then((response) => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                
                AuthenticationHelper.HandleResponse(response);
                
                return response.json().then(jsonResponse => {
                    if (response.ok) {
                        toast.success(jsonResponse.message);
                        setFieldValue({ name: "id", value: jsonResponse.id });
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

    const handleTeamMemberApprovalChange = (e, approval, teamMemberApprovalIds) => {
        let updatedIds;

        if (e.target.checked) {
            updatedIds = [...teamMemberApprovalIds, approval.id];
        } else {
            updatedIds = teamMemberApprovalIds.filter(id => id !== approval.id);
        }

        return setFieldValue({name: "teamMemberApprovalIds", value: updatedIds})
    }

    const {
        values, errors, dirty, disable,
        setFieldValue, handleOnChange, handleOnSubmit
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm, !props.id);
    
    const { id, email, firstName, lastName, isDisabled, teamMemberApprovalIds } = values;
    
    return (
        <section className="section section-small-padding">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div className="columns is-desktop">
                <div className="column is-half">
                    <form className="is-modern-form" onSubmit={handleOnSubmit}>
                        {id &&
                            <div className="field">
                                <InputLabel id="id-label">{props.idLabel} {id}</InputLabel>
                            </div>
                        }
                        <div className="field">
                            <TextField 
                                id="firstName"
                                name="firstName"
                                label={props.firstNameLabel} 
                                fullWidth={true}
                                value={firstName} 
                                onChange={handleOnChange} 
                                variant="standard"
                                helperText={dirty.firstName ? errors.firstName : ""} 
                                error={(errors.firstName.length > 0) && dirty.firstName} 
                            />
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
                                helperText={dirty.lastName ? errors.lastName : ""} 
                                error={(errors.lastName.length > 0) && dirty.lastName} 
                            />
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
                                InputProps={{
                                    readOnly: props.email ? true : false,
                                }}
                                helperText={dirty.email ? errors.email : ""} 
                                error={(errors.email.length > 0) && dirty.email} 
                            />
                        </div>
                        <div className="field">
                            <NoSsr>
                                <FormControlLabel
                                    control={
                                        <Switch
                                            onChange={e => {
                                                setFieldValue({ name: "isDisabled", value: !isDisabled });
                                            }}
                                            checked={isDisabled ? false : true}
                                            id="isDisabled"
                                            name="isDisabled"
                                            color="secondary" 
                                        />
                                    }
                                    label={isDisabled ? props.inActiveLabel : props.activeLabel} />
                            </NoSsr>
                        </div>
                        {props.teamMemberApprovals && props.teamMemberApprovals.length > 0 &&
                            props.teamMemberApprovals.map((approval, index) => {
                                return (
                                    <div key={index} className="field">
                                        <NoSsr>
                                            <FormControlLabel
                                                control={
                                                    <Switch
                                                        onChange={e => {
                                                            handleTeamMemberApprovalChange(e, approval, teamMemberApprovalIds)
                                                        }}
                                                        checked={teamMemberApprovalIds.indexOf(approval.id) !== -1}
                                                        id={approval.name}
                                                        name={approval.name}
                                                        color="secondary" />
                                                }
                                                label={approval.name} />
                                        </NoSsr>
                                        {teamMemberApprovalIds.indexOf(approval.id) !== -1 && approval.approvalDate &&
                                            <p>
                                                {props.expressedOnLabel}: {moment.utc(approval.approvalDate).local().format("L LT")}
                                            </p>
                                        }
                                    </div>
                                );
                            })
                        }
                        <div className="field">
                            <Button 
                                type="submit" 
                                variant="contained"
                                color="primary"
                                disabled={state.isLoading || disable}>
                                {props.saveText}
                            </Button>
                            <a href={props.clientTeamMembersUrl} className="ml-2 button is-text">{props.navigateToClientTeamMembersListText}</a>
                        </div>
                    </form>
                </div>
            </div>
            {state.isLoading && <CircularProgress className="progressBar" />}
        </section>
    )
}

ClientTeamMemberForm.propTypes = {
    id: PropTypes.string,
    clientId: PropTypes.string,
    clientName: PropTypes.string,
    clientNameLabel: PropTypes.string,
    clientEmail: PropTypes.string,
    clientEmailLabel: PropTypes.string,
    organisationId: PropTypes.string,
    organisationIdLabel: PropTypes.string,
    title: PropTypes.string.isRequired,
    saveText: PropTypes.string.isRequired,
    saveUrl: PropTypes.string.isRequired,
    generalErrorMessage: PropTypes.string.isRequired,
    idLabel: PropTypes.string,
    firstNameLabel: PropTypes.string.isRequired,
    lastNameLabel: PropTypes.string.isRequired,
    emailLabel: PropTypes.string.isRequired,
    firstName: PropTypes.string,
    lastName: PropTypes.string,
    email: PropTypes.string,
    fieldRequiredErrorMessage: PropTypes.string.isRequired,
    emailFormatErrorMessage: PropTypes.string.isRequired,
    clientTeamMembersUrl: PropTypes.string.isRequired,
    navigateToClientTeamMembersListText: PropTypes.string.isRequired,
    expressedOnLabel: PropTypes.string
}

export default ClientTeamMemberForm;


