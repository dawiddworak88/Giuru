import React, { useContext, useState } from "react";
import { toast } from "react-toastify";
import PropTypes from "prop-types";
import { Button, CircularProgress, NoSsr, FormControlLabel, Switch } from "@mui/material";
import { Context } from "../../../../shared/stores/Store";
import AuthenticationHelper from "../../../../shared/helpers/globals/AuthenticationHelper";
import useForm from "../../../../shared/helpers/forms/useForm";

function SettingsForm(props) {
    const [state, dispatch] = useContext(Context);
    const [reindexProductsDisable, setReindexProductsDisable] = useState(false);
    const stateSchema = {
        isExternalCompletionDates: { value: props.settings.externalCompletionDates === 'true' ? true : false }
    }

    const handleReindexProductsClick = (e) => {

        e.preventDefault();

        setReindexProductsDisable(true);

        dispatch({ type: "SET_IS_LOADING", payload: true });

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" }
        };

        fetch(props.productsIndexTriggerUrl, requestOptions)
            .then(function (response) {

                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {

                    if (response.ok) {

                        dispatch({ type: "SET_IS_LOADING", payload: false });
                        toast.success(jsonResponse.message);
                    }
                    else {

                        setReindexProductsDisable(false);
                        toast.error(props.generalErrorMessage);
                    }
                });
            }).catch(() => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(props.generalErrorMessage);
            });
    };

    const onSubmitForm = (state) => {
        
        const payload = {
            settings: {
                externalCompletionDates: state.isExternalCompletionDates
            }
        }

        dispatch({ type: "SET_IS_LOADING", payload: true });

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" },
            body: JSON.stringify(payload)
        }

        fetch(props.saveUrl, requestOptions)
            .then(function (response) {
                dispatch({ type: "SET_IS_LOADING", payload: false });

                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {
                    if (response.ok) {
                        toast.success(jsonResponse.message);
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

    const {
        values, setFieldValue, handleOnSubmit
    } = useForm(stateSchema, { }, onSubmitForm);

    const {
        isExternalCompletionDates
    } = values;

    return (
        <section className="section section-small-padding settings">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div className="columns is-desktop">
                <div className="column is-half">
                    <div className="field">
                        <Button type="button" onClick={handleReindexProductsClick} variant="contained" color="primary" disabled={state.isLoadingc || reindexProductsDisable}>
                            {props.reindexProductsText}
                        </Button>
                    </div>
                    <form className="is-modern-form" onSubmit={handleOnSubmit}>
                            <div className="field">
                                <NoSsr>
                                    <FormControlLabel
                                        control={
                                            <Switch
                                                onChange={(e) => 
                                                    setFieldValue({name: "isExternalCompletionDates", value: !isExternalCompletionDates})
                                                }
                                                checked={isExternalCompletionDates}
                                                id={"externalCompletionDates"}
                                                name={"externalCompletionDates"}
                                            />
                                        }
                                        label={props.externalCompletionDatesText} />
                                </NoSsr>
                            </div>
                        <div className="field">
                            <Button
                                type="submit"
                                variant="contained"
                                color="primary">
                                {props.saveText}
                            </Button>
                        </div>
                    </form>
                    {state.isLoading && <CircularProgress className="progressBar" />}
                </div>
            </div>
        </section>
    );
}

SettingsForm.propTypes = {
    title: PropTypes.string.isRequired,
    generalErrorMessage: PropTypes.string.isRequired,
    reindexProductsText: PropTypes.string.isRequired,
    productsIndexTriggerUrl: PropTypes.string.isRequired
};

export default SettingsForm;
