import React, { useContext, useCallback, useState } from 'react';
import PropTypes from 'prop-types';
import { UploadCloud } from 'react-feather';
import IconConstants from '../../../../../../shared/constants/IconConstants';
import { Context } from '../../../../../../shared/stores/Store';
import useForm from '../../../../../../shared/helpers/forms/useForm';
import { TextField, Button, CircularProgress } from '@material-ui/core';
import Autocomplete from '@material-ui/lab/Autocomplete';
import { useDropzone } from 'react-dropzone';

function ImportOrderForm(props) {

    const defaultProps = {
        options: props.clients,
        getOptionLabel: (option) => option.name,
    };

    const [state, dispatch] = useContext(Context);

    const [isClientSelected, setClientSelected] = useState(false);

    const stateSchema = {
        clientId: ''
    };

    const stateValidatorSchema = {
    };

    const onDrop = useCallback(acceptedFiles => {
        console.log(acceptedFiles);
    }, []);

    const { getRootProps, getInputProps, isDragActive } = useDropzone({ onDrop });

    function onSubmitForm(state) {

        dispatch({ type: 'SET_IS_LOADING', payload: true });
        console.log(state);
        dispatch({ type: 'SET_IS_LOADING', payload: false });
    }

    const {
        values,
        handleOnChange,
        disable,
        handleOnSubmit
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm);

    const { clientId } = values;

    return (
        <div>
            <form className="is-modern-form" onSubmit={handleOnSubmit} method="post">
                <div className="field">
                    <Autocomplete
                        {...defaultProps}
                        id="client"
                        name="client"
                        fullWidth={true}
                        value={clientId}
                        onChange={(event, newValue) => {
                            
                            handleOnChange(event);

                            if (newValue) {
                                setClientSelected(true);
                            }
                            else {
                                setClientSelected(false);
                            }
                          }}
                        autoComplete
                        includeInputInList
                        renderInput={(params) => <TextField {...params} label="Select a client" margin="normal" />}
                    />
                </div>
                <div className={isClientSelected ? "field" : "is-hidden" }>
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
                    <Button type="submit" variant="contained" color="primary" disabled={state.isLoading || disable}>
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
    generalErrorMessage: PropTypes.string.isRequired
};

export default ImportOrderForm;