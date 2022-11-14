import React, { useContext } from "react";
import PropTypes from "prop-types";
import { toast } from "react-toastify";
import { Context } from "../../../../../../shared/stores/Store";
import MediaCloud from "../../../../../../shared/components/MediaCloud/MediaCloud";
import useForm from "../../../../../../shared/helpers/forms/useForm";
import AuthenticationHelper from "../../../../../../shared/helpers/globals/AuthenticationHelper";
import {
    FormControl, InputLabel, Select, MenuItem, Button, CircularProgress
} from "@mui/material"

const MediaForm = (props) => {
    const [state, dispatch] = useContext(Context);
    const stateSchema = {
        files: { value: [] },
        clientGroupIds: { value: props.clientGroupIds ? props.clientGroupIds : []}
    }

    const stateValidatorSchema = {
        files: {
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
            headers: { 
                "Content-Type": "application/json", 
                "X-Requested-With": "XMLHttpRequest" 
            },
            body: JSON.stringify(state)
        };

        fetch(props.saveUrl, requestOptions)
            .then((res) => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                
                AuthenticationHelper.HandleResponse(res);
                
                return res.json().then(jsonResponse => {
                    if (res.ok) {
                        toast.success(jsonResponse.message);
                    }
                });
            }).catch(() => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(props.generalErrorMessage);
            });
    }

    const {
        values, disable, setFieldValue, handleOnChange, handleOnSubmit
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm);
    
    const { files, clientGroupIds } = values;
    
    return (
        <section className="section section-small-padding product client-form">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div className="columns is-desktop">
                <div className="column is-half">
                    <form className="is-modern-form" onSubmit={handleOnSubmit}>
                        <div className="field">
                            <FormControl fullWidth={true} variant="standard">
                                <InputLabel id="clientGroups-label">{props.groupsLabel}</InputLabel>
                                <Select
                                    labelId="clientGroups-label"
                                    id="clientGroupIds"
                                    name="clientGroupIds"
                                    value={clientGroupIds}
                                    multiple={true}
                                    onChange={handleOnChange}>
                                    {props.groups && props.groups.length > 0 ? (
                                        props.groups.map((group, index) => {
                                            return (
                                                <MenuItem key={index} value={group.id}>{group.name}</MenuItem>
                                            );
                                        })
                                    ) : (
                                        <MenuItem disabled>{props.noGroupsText}</MenuItem>
                                    )}
                                </Select>
                            </FormControl>
                        </div>
                        <div className="field">
                            <MediaCloud
                                id="files"
                                name="files"
                                label={props.mediaItemsLabel}
                                multiple={true}
                                generalErrorMessage={props.generalErrorMessage}
                                deleteLabel={props.deleteLabel}
                                dropFilesLabel={props.dropFilesLabel}
                                dropOrSelectFilesLabel={props.dropOrSelectImagesLabel}
                                files={files}
                                isUploadInChunksEnabled={true}
                                chunkSize={props.chunkSize}
                                saveMediaChunkUrl={props.saveMediaChunkUrl}
                                saveMediaChunkCompleteUrl={props.saveMediaChunkCompleteUrl}
                                setFieldValue={setFieldValue}
                                saveMediaUrl={props.saveMediaUrl} 
                                accept={{
                                    "image/*": [".png", ".jpg", ".webp"],
                                    "application/*": [".pdf", ".docx", ".doc", ".zip"]
                                }}/>
                        </div>
                        <div className="field">
                            <Button 
                                type="submit" 
                                variant="contained" 
                                color="primary"
                                disabled={disable || files.length == 0}>
                                {props.saveText}
                            </Button>
                            <a href={props.mediaUrl} className="ml-2 button is-text">{props.backToMediaText}</a>
                        </div>
                    </form>
                </div>
            </div>
            {state.isLoading && <CircularProgress className="progressBar" />}
        </section>
    );
}

MediaForm.propTypes = {
    title: PropTypes.string.isRequired,
    mediaItemsLabel: PropTypes.string.isRequired,
    generalErrorMessage: PropTypes.string.isRequired,
    deleteLabel: PropTypes.string.isRequired,
    dropFilesLabel: PropTypes.string.isRequired,
    dropOrSelectImagesLabel: PropTypes.string.isRequired,
    saveMediaUrl: PropTypes.string.isRequired,
    backToMediaText: PropTypes.string.isRequired,
    mediaUrl: PropTypes.string.isRequired,
    isUploadInChunksEnabled: PropTypes.bool.isRequired,
    chunkSize: PropTypes.string.isRequired,
    saveMediaChunkUrl: PropTypes.string.isRequired,
    saveMediaChunkCompleteUrl: PropTypes.string.isRequired,
    saveText: PropTypes.string.isRequired,
    noGroupsText: PropTypes.string.isRequired,
    groupsLabel: PropTypes.string.isRequired
}

export default MediaForm;
