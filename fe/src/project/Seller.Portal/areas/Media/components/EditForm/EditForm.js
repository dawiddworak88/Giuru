import React, { useState, useContext } from "react";
import { toast } from "react-toastify";
import PropTypes from "prop-types"; 
import {
    TextField, Button,  CircularProgress
} from "@material-ui/core";
import {
    PictureAsPdf, Attachment
} from "@material-ui/icons";
import MediaCloud from "../../../../../../shared/components/MediaCloud/MediaCloud";
import NavigationHelper from "../../../../../../shared/helpers/globals/NavigationHelper";
import useForm from "../../../../../../shared/helpers/forms/useForm";
import { Context } from "../../../../../../shared/stores/Store";

const EditForm = (props) => {
    const [state, dispatch] = useContext(Context);
    const [versions] = useState(props.versions ? props.versions.slice(1) : []);
    const [images, setImages] = useState(props.versions ? props.versions.slice(0, 1) : []);
    const stateSchema = {
        id: { value: props.id ? props.id : null, error: "" },
        name: {value: props.name ? props.name : null, error: ""},
        description: {value: props.description ? props.description : null, error: ""}
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

        const requestBody = {
            id: state.id,
            name: state.name,
            description: state.description
        };

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(requestBody)
        };

        fetch(props.updateMediaVersionUrl, requestOptions)
            .then((response) => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
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

    const {name, description} = values;
    console.log(JSON.stringify(versions))
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
                                accept=".png, .jpg, .pdf, .zip, .webp"
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
                        {versions &&
                            <div className="media-edit__last-files">
                                <h2>Ostatnie wersje pliku</h2>
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
                                onChange={handleOnChange} />
                        </div>
                        <div className="field">
                            <TextField 
                                id="description" 
                                name="description" 
                                type="text" 
                                label={props.descriptionLabel} 
                                fullWidth={true} 
                                value={description} 
                                onChange={handleOnChange} />
                        </div>
                        <div className="field">
                            <Button type="submit" variant="contained" color="primary" disabled={disable || state.isLoading}>
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

EditForm.propTypes = {
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
    saveMediaText: PropTypes.string
}

export default EditForm;