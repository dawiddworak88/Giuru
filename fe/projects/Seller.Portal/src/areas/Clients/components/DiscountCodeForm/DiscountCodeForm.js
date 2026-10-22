import React, { useContext } from "react";
import {
    TextField, Button, InputLabel, CircularProgress,
    Autocomplete, FormControlLabel, Switch
} from "@mui/material";
import { toast } from "react-toastify";
import PropTypes from "prop-types";
import useForm from "../../../../shared/helpers/forms/useForm";
import AuthenticationHelper from "../../../../shared/helpers/globals/AuthenticationHelper";
import { Context } from "../../../../shared/stores/Store";

const DiscountCodeForm = (props) => {
    const [state, dispatch] = useContext(Context);
    const stateSchema = {
        id: { value: props.id ? props.id : null, error: "" },
        code: { value: props.code ? props.code : "", error: "" },
        description: { value: props.description ? props.description : "", error: "" },
        isDisabled: { value: props.isDisabled ? props.isDisabled : false },
        clientIds: { value: props.clientIds ? props.clientIds : [] }
    };

    const stateValidatorSchema = {
        code: {
            required: {
                isRequired: true,
                error: props.codeRequiredErrorMessage
            }
        }
    };

    const onSubmitForm = (state) => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        const requestOptions = {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "X-Requested-With": "XMLHttpRequest"
            },
            body: JSON.stringify(state)
        };

        fetch(props.saveUrl, requestOptions)
            .then((response) => {
                dispatch({ type: "SET_IS_LOADING", payload: false });

                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {
                    if (response.ok) {
                        setFieldValue({ name: "id", value: jsonResponse.id });
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
        setFieldValue, handleOnChange, handleOnSubmit
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm, !props.id);

    const { id, code, description, isDisabled, clientIds } = values;

    const assignedClients = props.clients.filter(client => clientIds.includes(client.id));

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
                                id="code"
                                name="code"
                                label={props.codeLabel}
                                fullWidth={true}
                                value={code}
                                onChange={handleOnChange}
                                variant="standard"
                                disabled={!!props.id}
                                inputProps={{ maxLength: 50 }}
                                helperText={dirty.code ? errors.code : ""}
                                error={(errors.code.length > 0) && dirty.code} />
                        </div>
                        <div className="field">
                            <TextField
                                id="description"
                                name="description"
                                label={props.descriptionLabel}
                                fullWidth={true}
                                multiline
                                value={description}
                                onChange={handleOnChange}
                                variant="standard" />
                        </div>
                        <div className="field">
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
                        </div>
                        <div className="field">
                            <Autocomplete
                                multiple
                                options={props.clients}
                                getOptionLabel={(option) => option.name}
                                isOptionEqualToValue={(option, value) => option.id === value.id}
                                id="clientIds"
                                fullWidth={true}
                                value={assignedClients}
                                disabled={state.isLoading}
                                onChange={(event, newValue) => {
                                    setFieldValue({ name: "clientIds", value: newValue.map(client => client.id) });
                                }}
                                renderInput={(params) => <TextField {...params} label={props.clientsLabel} margin="normal" variant="standard" />}
                            />
                        </div>
                        <div className="field">
                            <Button
                                type="submit"
                                variant="contained"
                                color="primary"
                                disabled={state.isLoading || disable}>
                                {props.saveText}
                            </Button>
                            <a href={props.discountCodesUrl} className="ml-2 button is-text">{props.navigateToDiscountCodesText}</a>
                        </div>
                    </form>
                </div>
            </div>
            {state.isLoading && <CircularProgress className="progressBar" />}
        </section>
    )
}

DiscountCodeForm.propTypes = {
    title: PropTypes.string.isRequired,
    idLabel: PropTypes.string.isRequired,
    codeLabel: PropTypes.string.isRequired,
    descriptionLabel: PropTypes.string.isRequired,
    clientsLabel: PropTypes.string.isRequired,
    activeLabel: PropTypes.string.isRequired,
    inActiveLabel: PropTypes.string.isRequired,
    clients: PropTypes.array.isRequired,
    discountCodesUrl: PropTypes.string.isRequired,
    navigateToDiscountCodesText: PropTypes.string.isRequired,
    saveText: PropTypes.string.isRequired,
    saveUrl: PropTypes.string.isRequired,
    generalErrorMessage: PropTypes.string.isRequired,
    codeRequiredErrorMessage: PropTypes.string.isRequired
}

export default DiscountCodeForm;
