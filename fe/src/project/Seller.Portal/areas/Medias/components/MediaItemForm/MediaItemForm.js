import React, { useState, useContext, useEffect } from "react";
import { toast } from "react-toastify";
import PropTypes from "prop-types"; 
import {
    TextField, Button,  CircularProgress
} from "@mui/material";
import {
    PictureAsPdf, Attachment
} from "@mui/icons-material";
import MediaCloud from "../../../../../../shared/components/MediaCloud/MediaCloud";
import NavigationHelper from "../../../../../../shared/helpers/globals/NavigationHelper";
import useForm from "../../../../../../shared/helpers/forms/useForm";
import { Context } from "../../../../../../shared/stores/Store";
import AuthenticationHelper from "../../../../../../shared/helpers/globals/AuthenticationHelper";

const MediaItemForm = (props) => {
    const [state, dispatch] = useContext(Context);
    const [versions, setVersions] = useState(props.versions ? props.versions.slice(1) : []);
    const [images, setImages] = useState(props.versions ? props.versions.slice(0, 1) : []);
    const stateSchema = {
        id: { value: props.id ? props.id : null, error: "" },
        name: { value: props.name ? props.name : null, error: "" },
        description: { value: props.description ? props.description : null, error: "" },
        metadata: { value: props.metaData ? props.metaData : null, error: "" },
    };

    const stateValidatorSchema = {
        id: {
            required: {
                isRequired: true,
                error: props.imagesRequiredErrorMessage
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

        fetch(props.updateMediaVersionUrl, requestOptions)
            .then((response) => {
                dispatch({ type: "SET_IS_LOADING", payload: false });

                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {
                    if (response.ok) {
                        toast.success(jsonResponse.message);
                    }
                });
            }).catch(() => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(props.generalErrorMessage);
            });
    }

    const mediaHandle = (url) => {
        NavigationHelper.redirect(url)
    }

    const {
        values, disable, handleOnChange, handleOnSubmit
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm);

    const {name, description, metadata} = values;
    return (
        <section className="section section-small-padding product client-form media-edit">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div className="columns is-desktop">
                <div className="column is-half">
                    <form className="is-modern-form" onSubmit={handleOnSubmit}>
                        <div className="field">
                            <MediaCloud
                                id="images"
                                name="images"
                                label={props.mediaItemsLabel}
                                accept=".png, .jpg, .pdf, .zip, .webp, .docx"
                                multiple={false}
                                mediaId={props.id}
                                generalErrorMessage={props.generalErrorMessage}
                                deleteLabel={props.deleteLabel}
                                dropFilesLabel={props.dropFilesLabel}
                                dropOrSelectFilesLabel={props.dropOrSelectImagesLabel}
                                files={images}
                                setFieldValue={({value}) => setImages(value)}
                                saveMediaUrl={props.saveMediaUrl} />
                        </div>
                        {versions && versions.length > 0 &&
                            <div className="media-edit__last-files">
                                <h2>{props.latestVersionsLabel}</h2>
                                <div className="media-edit__versions">
                                    {versions.map((version) => {
                                        const url = version.url;
                                        if (version.mimeType.includes("pdf")) {
                                            return (
                                                <div className="version icon-version" key={version.id} onClick={() => mediaHandle(url)}>
                                                    <div className="icon">
                                                        <PictureAsPdf />
                                                    </div>
                                                </div>
                                            )
                                        } else if (version.mimeType.startsWith("image")) {
                                            return (
                                                <div className="version" key={version.id} onClick={() => mediaHandle(url)}>
                                                    <img src={version.url} alt={version.filename} />
                                                </div>
                                            )
                                        } else  {
                                            return (
                                                <div className="version icon-version" key={version.id} onClick={() => mediaHandle(url)}>
                                                    <div className="icon">
                                                        <Attachment/>
                                                    </div>
                                                </div>
                                            )
                                        }
                                    })}
                                </div>
                            </div>
                        }
                        <div className="field">
                            <TextField 
                                id="name" 
                                name="name" 
                                type="text" 
                                label={props.nameLabel}
                                fullWidth={true}
                                value={name}
                                variant="standard"
                                onChange={handleOnChange} />
                        </div>
                        <div className="field">
                            <TextField 
                                id="description" 
                                name="description" 
                                type="text" 
                                label={props.descriptionLabel} 
                                fullWidth={true}
                                variant="standard"
                                value={description} 
                                onChange={handleOnChange} />
                        </div>
                        <div className="field">
                            <TextField 
                                id="metadata" 
                                name="metadata" 
                                type="text" 
                                label={props.metaDataLabel} 
                                fullWidth={true} 
                                value={metadata} 
                                variant="standard"
                                onChange={handleOnChange} />
                        </div>
                        <div className="field">
                            <Button 
                                type="submit" 
                                variant="contained" 
                                color="primary" 
                                disabled={disable || state.isLoading}>
                                {props.saveMediaText}
                            </Button>
                        </div>
                    </form>
        
                    {state.isLoading && <CircularProgress className="progressBar" />}
                </div>
            </div>
        </section>
    )
}

MediaItemForm.propTypes = {
    title: PropTypes.string.isRequired,
    descriptionLabel: PropTypes.string,
    nameLabel: PropTypes.string,
    generalErrorMessage: PropTypes.string,
    mediaItemsLabel: PropTypes.string,
    deleteLabel: PropTypes.string,
    dropFilesLabel: PropTypes.string,
    dropOrSelectImagesLabel: PropTypes.string,
    saveMediaUrl: PropTypes.string,
    backToMediaText: PropTypes.string,
    saveMediaText: PropTypes.string,
    latestVersionsLabel: PropTypes.string
}

export default MediaItemForm;