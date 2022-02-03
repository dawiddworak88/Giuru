import React, { useContext, useState } from "react";
import { toast } from "react-toastify";
import PropTypes from "prop-types";
import { Button, CircularProgress } from "@material-ui/core";
import { Context } from "../../../../../../shared/stores/Store";
import AuthenticationHelper from "../../../../../../shared/helpers/globals/AuthenticationHelper";

function SettingsForm(props) {

    const [state, dispatch] = useContext(Context);
    const [reindexProductsDisable, setReindexProductsDisable] = useState(false);

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

    return (
        <section className="section section-small-padding settings">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div className="columns is-desktop">
                <div className="column is-half">
                    <div>
                        <Button type="button" onClick={handleReindexProductsClick} variant="contained" color="primary" disabled={state.isLoadingc || reindexProductsDisable}>
                            {props.reindexProductsText}
                        </Button>
                    </div>
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
