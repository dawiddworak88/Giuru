import React, { useContext, useState } from "react";
import { Context } from "../../../../shared/stores/Store";
import { toast } from "react-toastify";
import useForm from "../../../../shared/helpers/forms/useForm";
import AuthenticationHelper from "../../../../shared/helpers/globals/AuthenticationHelper";
import {
    TextField, Button, CircularProgress, InputLabel, Autocomplete,
    NoSsr, FormControlLabel, Switch, FormHelperText
} from "@mui/material";
import PropTypes from "prop-types";

const CODE_MAX_LENGTH = 64;
const DESCRIPTION_MAX_LENGTH = 256;

const DiscountCodeForm = (props) => {
    const [state, dispatch] = useContext(Context);
    const clients = props.clients ? props.clients : [];
    const assignedClientIds = props.clientIds ? props.clientIds : [];

    // The version belongs to the form but not to the fields: it moves on with every successful save, and a save
    // that is refused as stale leaves it alone, so the draft stays on screen until the user reloads.
    const [version, setVersion] = useState(props.version ? props.version : null);

    const stateSchema = {
        id: { value: props.id ? props.id : null },
        code: { value: props.code ? props.code : "", error: "" },
        description: { value: props.description ? props.description : "" },
        isDisabled: { value: props.isDisabled ? props.isDisabled : false },
        selectedClients: { value: clients.filter((client) => assignedClientIds.includes(client.id)) }
    };

    const stateValidatorSchema = {
        code: {
            required: {
                isRequired: true,
                error: props.codeRequiredErrorMessage
            },
            validator: {
                func: value => !!value && value.trim().length > 0,
                error: props.codeRequiredErrorMessage
            }
        }
    };

    const onSubmitForm = (state) => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        // The assignment list is always sent, empty or not: an empty list means "assigned to nobody".
        const payload = {
            id: state.id,
            version,
            code: state.code.trim(),
            description: state.description ? state.description.trim() : "",
            isDisabled: state.isDisabled,
            clientIds: state.selectedClients.map((client) => client.id)
        };

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" },
            body: JSON.stringify(payload)
        };

        fetch(props.saveUrl, requestOptions)
            .then((response) => {
                dispatch({ type: "SET_IS_LOADING", payload: false });

                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {
                    if (response.ok) {
                        setFieldValue({ name: "id", value: jsonResponse.id });
                        setVersion(jsonResponse.version);
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
    };

    const {
        values, errors, dirty, disable,
        setFieldValue, handleOnChange, handleOnSubmit
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm, !props.id);

    const { id, code, description, isDisabled, selectedClients } = values;

    return (
        <section className="section section-small-padding">
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
                            <TextField
                                id="code"
                                name="code"
                                label={props.codeLabel}
                                fullWidth={true}
                                value={code}
                                variant="standard"
                                onChange={handleOnChange}
                                slotProps={{ htmlInput: { maxLength: CODE_MAX_LENGTH } }}
                                helperText={dirty.code ? errors.code : ""}
                                error={(errors.code.length > 0) && dirty.code} />
                            <FormHelperText>{props.grulaHint}</FormHelperText>
                        </div>
                        <div className="field">
                            <TextField
                                id="description"
                                name="description"
                                label={props.descriptionLabel}
                                fullWidth={true}
                                value={description}
                                variant="standard"
                                onChange={handleOnChange}
                                slotProps={{ htmlInput: { maxLength: DESCRIPTION_MAX_LENGTH } }} />
                        </div>
                        <div className="field">
                            <Autocomplete
                                id="selectedClients"
                                multiple={true}
                                options={clients}
                                getOptionLabel={(option) => option.name}
                                isOptionEqualToValue={(option, value) => option.id === value.id}
                                filterSelectedOptions={true}
                                fullWidth={true}
                                value={selectedClients}
                                onChange={(event, newValue) => {
                                    setFieldValue({ name: "selectedClients", value: newValue });
                                }}
                                renderInput={(params) => (
                                    <TextField
                                        {...params}
                                        label={props.clientsLabel}
                                        variant="standard"
                                        margin="normal" />
                                )} />
                            {selectedClients.length === 0 &&
                                <FormHelperText>{props.noClientsHint}</FormHelperText>
                            }
                        </div>
                        <div className="field">
                            <NoSsr>
                                <FormControlLabel
                                    control={
                                        <Switch
                                            onChange={() => {
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
                        <div className="field">
                            <Button
                                type="submit"
                                variant="contained"
                                color="primary"
                                disabled={state.isLoading || disable}>
                                {props.saveText}
                            </Button>
                            <a href={props.discountCodesUrl} className="ml-2 button is-text">{props.navigateToDiscountCodes}</a>
                        </div>
                    </form>
                    {state.isLoading && <CircularProgress className="progressBar" />}
                </div>
            </div>
        </section>
    );
};

DiscountCodeForm.propTypes = {
    title: PropTypes.string.isRequired,
    id: PropTypes.string,
    version: PropTypes.string,
    idLabel: PropTypes.string,
    code: PropTypes.string,
    codeLabel: PropTypes.string.isRequired,
    codeRequiredErrorMessage: PropTypes.string.isRequired,
    description: PropTypes.string,
    descriptionLabel: PropTypes.string.isRequired,
    isDisabled: PropTypes.bool,
    clientIds: PropTypes.array,
    clients: PropTypes.array,
    clientsLabel: PropTypes.string.isRequired,
    noClientsHint: PropTypes.string.isRequired,
    grulaHint: PropTypes.string.isRequired,
    activeLabel: PropTypes.string.isRequired,
    inActiveLabel: PropTypes.string.isRequired,
    saveUrl: PropTypes.string.isRequired,
    saveText: PropTypes.string.isRequired,
    generalErrorMessage: PropTypes.string,
    navigateToDiscountCodes: PropTypes.string.isRequired,
    discountCodesUrl: PropTypes.string.isRequired
};

export default DiscountCodeForm;
