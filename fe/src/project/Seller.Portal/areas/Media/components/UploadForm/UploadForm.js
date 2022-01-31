import React, { useState } from "react";
import PropTypes from "prop-types";
import { Button } from "@material-ui/core";
import MediaCloud from "../../../../../../shared/components/MediaCloud/MediaCloud";
import NavigationHelper from "../../../../../../shared/helpers/globals/NavigationHelper";

function UploadForm(props) {
    const [images, setImages] = useState([]);
    const backToMediaList = (e) => {
        e.preventDefault();
        NavigationHelper.redirect(props.mediaUrl);
    }
    
    const buttonDisable = images.length === 0 ? true : false;
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
                                setFieldValue={({value}) => setImages(value)}
                                saveMediaUrl={props.saveMediaUrl} />
                        </div>
                        <div className="field">
                            <Button type="submit" variant="contained" color="primary" onClick={backToMediaList} disabled={buttonDisable}>
                                {props.backToMediaText}
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
    backToMediaText: PropTypes.string.isRequired,
    mediaUrl: PropTypes.string.isRequired
}

export default UploadForm;
