import React, { useState } from "react";
import { toast } from "react-toastify";
import PropTypes from "prop-types";
import { Context } from "../../../../../../shared/stores/Store";
import { Button, CircularProgress } from "@material-ui/core";
import useForm from "../../../../../../shared/helpers/forms/useForm";
import MediaCloud from "../../../../../../shared/components/MediaCloud/MediaCloud";

function UploadForm(props) {
    const [images, setImages] = useState([]);
    const setFieldValue = (value) => {
        setImages(value)
    }
    console.log(images)
    const buttonDisable = images.length == 0 ? true : false;
    return (
        <section className="section section-small-padding product client-form">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div className="columns is-desktop">
                <div className="column is-half">
                    <form className="is-modern-form">
                        <div className="field">
                            <MediaCloud
                                id="images"
                                name="images"
                                label={props.mediaItemsLabel}
                                accept=".png, .jpg"
                                multiple={true}
                                generalErrorMessage={props.generalErrorMessage}
                                deleteLabel={props.deleteLabel}
                                dropFilesLabel={props.dropFilesLabel}
                                dropOrSelectFilesLabel={props.dropOrSelectImagesLabel}
                                files={images}
                                setFieldValue={({value}) => setFieldValue(value)}
                                saveMediaUrl={props.saveMediaUrl} />
                        </div>
                        <div className="field">
                            <Button type="submit" variant="contained" color="primary" disabled={buttonDisable}>
                                {props.saveMediaText}
                            </Button>
                        </div>
                    </form>
                </div>
            </div>
        </section>
    );
}

UploadForm.propTypes = {
    title: PropTypes.string.isRequired,
    mediaItemsLabel: PropTypes.string.isRequired,
    generalErrorMessage: PropTypes.string.isRequired,
    deleteLabel: PropTypes.string.isRequired,
    dropFilesLabel: PropTypes.string.isRequired,
    dropOrSelectImagesLabel: PropTypes.string.isRequired,
    saveMediaUrl: PropTypes.string.isRequired,
    saveMediaText: PropTypes.string.isRequired
}

export default UploadForm;
