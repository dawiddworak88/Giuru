import { InputLabel, TextField, Button, CircularProgress } from "@mui/material";
import PropTypes from "prop-types";
import React, { useContext } from "react";
import useForm from "../../../../shared/helpers/forms/useForm";
import { Context } from "../../../../shared/stores/Store";
import AuthenticationHelper from "../../../../shared/helpers/globals/AuthenticationHelper";
import { toast } from "react-toastify";

const CurrencyForm = (props) => {
    const [state, dispatch] = useContext(Context);
    const stateSchema = {
        id: { value: props.id ? props.id : null },
        currencyCode: { value: props.currencyCode ? props.currencyCode : "", error: "" },
        symbol: { value: props.symbol ? props.symbol : "", error: "" },
        name: { value: props.name ? props.name : "", error: "" }
    };

    const stateValidatorSchema = {
        currencyCode: {
            required: {
                isRequired: true,
                error: props.fieldRequiredErrorMessage
            }
        },
        symbol: {
            required: {
                isRequired: true,
                error: props.fieldRequiredErrorMessage
            }
        },
        name: {
            required: {
                isRequired: true,
                error: props.fieldRequiredErrorMessage
            }
        }
    };

    const onSubmitForm = (state) => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" },
            body: JSON.stringify(state)
        };

        fetch(props.saveUrl, requestOptions)
            .then(response => {
                dispatch({ type: "SET_IS_LOADING", payload: false });

                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {
                    if (response.ok) {
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
        handleOnChange, handleOnSubmit
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm, !props.id);

    const { id, currencyCode, symbol, name } = values;

    return (
        <section className="section section-small-padding product client-form">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div className="columns is-desktop">
                <div className="column is-half">
                    <form className="is-modern-form" onSubmit={handleOnSubmit} method="post">
                        {id &&
                            <div>
                                <InputLabel id="id-label">{props.idLabel} {id}</InputLabel>
                            </div>
                        }
                        <div className="field">
                            <TextField
                                id="currencyCode"
                                name="currencyCode"
                                label={props.currencyCodeLabel}
                                fullWidth={true}
                                value={currencyCode}
                                variant="standard"
                                onChange={handleOnChange}
                                helperText={dirty.currencyCode ? errors.currencyCode : ""}
                                error={(errors.currencyCode.length > 0) && dirty.currencyCode} />
                        </div>
                        <div className="field">
                            <TextField
                                id="symbol"
                                name="symbol"
                                label={props.symbolLabel}
                                fullWidth={true}
                                value={symbol}
                                variant="standard"
                                onChange={handleOnChange}
                                helperText={dirty.symbol ? errors.symbol : ""}
                                error={(errors.symbol.length > 0) && dirty.symbol} />
                        </div>
                        <div className="field">
                            <TextField
                                id="name"
                                name="name"
                                label={props.nameLabel}
                                fullWidth={true}
                                value={name}
                                variant="standard"
                                onChange={handleOnChange}
                                helperText={dirty.name ? errors.name : ""} 
                                error={(errors.name.length > 0) && dirty.name} />
                        </div>
                        <div className="field ">
                            <Button
                                type="submit"
                                variant="contained"
                                color="primary"
                                disabled={state.isLoading || disable}>
                                {props.saveText}
                            </Button>
                            <a href={props.currenciesUrl} className="ml-2 button is-text">{props.navigateToCurrencies}</a>
                        </div>
                    </form>
                    {state.isLoading && <CircularProgress className="progressBar" />}
                </div>
            </div>
        </section>
    )
}

CurrencyForm.propTypes = {
    id: PropTypes.string,
    saveUrl: PropTypes.string.isRequired,
    title: PropTypes.string.isRequired,
    idLabel: PropTypes.string.isRequired,
    currencyCode: PropTypes.string,
    currencyCodeLabel: PropTypes.string.isRequired,
    symbol: PropTypes.string,
    symbolLabel: PropTypes.string.isRequired,
    name: PropTypes.string,
    nameLabel: PropTypes.string.isRequired,
    saveText: PropTypes.string.isRequired,
    navigateToCurrencies: PropTypes.string.isRequired,
    generalErrorMessage: PropTypes.string.isRequired,
    fieldRequiredErrorMessage: PropTypes.string.isRequired
}

export default CurrencyForm;
