import React, { useContext } from "react";
import useForm from "../../../../shared/helpers/forms/useForm";
import { TextField, Button, InputLabel, CircularProgress, NoSsr, FormControlLabel, Switch } from "@mui/material";
import AuthenticationHelper from "../../../../shared/helpers/globals/AuthenticationHelper";
import { Context } from "../../../../shared/stores/Store";
import { toast } from "react-toastify";
import PropTypes from "prop-types";

const CODE_MAX_LENGTH = 64;
const DESCRIPTION_MAX_LENGTH = 256;

const DiscountCodeForm = (props) => {
    const [state, dispatch] = useContext(Context);
    const stateSchema = {
        id: { value: props.id ? props.id : null, error: "" },
        code: { value: props.code ? props.code : "", error: "" },
        description: { value: props.description ? props.description : "", error: "" },
        isDisabled: { value: props.isDisabled ? props.isDisabled : false }
    };

    const stateValidatorSchema = {
        code: {
            required: {
                isRequired: true,
                error: props.codeRequiredErrorMessage
            },
            validator: {
                func: value => !value || value.trim().length <= CODE_MAX_LENGTH,
                error: props.codeMaxLengthErrorMessage
            }
        },
        description: {
            validator: {
                func: value => !value || value.length <= DESCRIPTION_MAX_LENGTH,
                error: props.descriptionMaxLengthErrorMessage
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
                        // The server message carries the reason, for example that the code is not defined in Grula.
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

    const { id, code, description, isDisabled } = values;

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
                                helperText={dirty.code ? errors.code : ""}
                                error={(errors.code.length > 0) && dirty.code}
                                slotProps={{ input: {
                                    readOnly: id ? true : false,
                                }}} />
                        </div>
                        <div className="field">
                            <TextField
                                id="description"
                                name="description"
                                label={props.descriptionLabel}
                                fullWidth={true}
                                value={description}
                                onChange={handleOnChange}
                                variant="standard"
                                helperText={dirty.description ? errors.description : ""}
                                error={(errors.description.length > 0) && dirty.description} />
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
    id: PropTypes.string,
    code: PropTypes.string,
    description: PropTypes.string,
    isDisabled: PropTypes.bool,
    title: PropTypes.string.isRequired,
    idLabel: PropTypes.string.isRequired,
    codeLabel: PropTypes.string.isRequired,
    descriptionLabel: PropTypes.string.isRequired,
    activeLabel: PropTypes.string.isRequired,
    inActiveLabel: PropTypes.string.isRequired,
    discountCodesUrl: PropTypes.string.isRequired,
    navigateToDiscountCodesText: PropTypes.string.isRequired,
    saveText: PropTypes.string.isRequired,
    saveUrl: PropTypes.string.isRequired,
    generalErrorMessage: PropTypes.string.isRequired,
    codeRequiredErrorMessage: PropTypes.string.isRequired,
    codeMaxLengthErrorMessage: PropTypes.string.isRequired,
    descriptionMaxLengthErrorMessage: PropTypes.string.isRequired
}

export default DiscountCodeForm;