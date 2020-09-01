import React, { useContext, useCallback, useState } from "react";
import { toast } from "react-toastify";
import PropTypes from "prop-types";
import { UploadCloud } from "react-feather";
import IconConstants from "../../../../../../shared/constants/IconConstants";
import { Context } from "../../../../../../shared/stores/Store";
import { TextField, Button, CircularProgress } from "@material-ui/core";
import Autocomplete from "@material-ui/lab/Autocomplete";
import { useDropzone } from "react-dropzone";
import FetchErrorHandler from "../../../../../../shared/helpers/errorHandlers/FetchErrorHandler";

function ImportOrderForm(props) {

    const defaultProps = {
        options: props.clients,
        getOptionLabel: (option) => option.name
    };

    const [state, dispatch] = useContext(Context);

    const [isClientSelected, setClientSelected] = useState(false);

    const [client, setClient] = useState(null);

    const onDrop = useCallback(acceptedFiles => {

        dispatch({ type: "SET_IS_LOADING", payload: true });

        acceptedFiles.forEach((file) => {

            const formData = new FormData();

            console.log(client);

            if (client) {
                formData.append("clientId", client.id);
            }
            
            formData.append("orderFile", file);

            const requestOptions = {
                method: "POST",
                body: formData
            };

            fetch(props.validateOrderUrl, requestOptions)
                .then(function (response) {

                    dispatch({ type: "SET_IS_LOADING", payload: false });

                    FetchErrorHandler.handleUnauthorizedResponse(response);

                    return response.json().then(jsonResponse => {

                        if (response.ok) {
                            dispatch({ type: "SET_IS_LOADING", payload: false });
                        }
                        else {
                            FetchErrorHandler.consoleLogResponseDetails(state, response, jsonResponse);
                            toast.error(props.generalErrorMessage);
                        }
                    });
                }).catch(error => {
                    dispatch({ type: "SET_IS_LOADING", payload: false });
                    toast.error(props.generalErrorMessage);
                })
        })
    }, [client, dispatch, state, props.generalErrorMessage, props.validateOrderUrl]);

    const { getRootProps, getInputProps, isDragActive } = useDropzone({
        onDrop,
        accept: ".xls, .xlsx",
        multiple: false
    });

    return (
        <div>
            <form className="is-modern-form" method="post">
                <div className="field">
                    <Autocomplete
                        {...defaultProps}
                        id="client"
                        name="client"
                        fullWidth={true}
                        value={client}
                        onChange={(event, newValue) => {

                            if (newValue) {
                                setClientSelected(true);
                                setClient(newValue);
                            }
                            else {
                                setClientSelected(false);
                                setClient(null);
                            }
                        }}
                        autoComplete
                        renderInput={(params) => <TextField {...params} label={props.selectClientLabel} margin="normal" />}
                    />
                </div>
                <div className={isClientSelected ? "field" : "is-hidden"}>
                    <div {...getRootProps()}>
                        <input id="order" name="order" {...getInputProps()} />
                        {
                            isDragActive ?
                                (
                                    <div className="dropzone dropzone--active">
                                        <p>
                                            <UploadCloud size={IconConstants.DefaultSize()} />
                                        </p>
                                        <p>{props.dropFilesLabel}</p>
                                    </div>
                                ) :
                                (
                                    <div className="dropzone">
                                        <p>
                                            <UploadCloud size={IconConstants.DefaultSize()} />
                                        </p>
                                        <p>{props.dropOrSelectFilesLabel}</p>
                                    </div>
                                )
                        }
                    </div>
                </div>
                <div className="field">
                    <Button type="submit" variant="contained" color="primary" disabled={state.isLoading}>
                        {props.saveText}
                    </Button>
                </div>
            </form>
            {state.isLoading && <CircularProgress className="progressBar" />}
        </div>
    );
}

ImportOrderForm.propTypes = {
    saveText: PropTypes.string.isRequired,
    validateOrderUrl: PropTypes.string.isRequired,
    dropFilesLabel: PropTypes.string.isRequired,
    dropOrSelectFilesLabel: PropTypes.string.isRequired,
    generalErrorMessage: PropTypes.string.isRequired
};

export default ImportOrderForm;