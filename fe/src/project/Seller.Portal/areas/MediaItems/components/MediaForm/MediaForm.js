import React, { useState } from "react";
import PropTypes from "prop-types";
import MediaCloud from "../../../../../../shared/components/MediaCloud/MediaCloud";

const MediaForm = (props) => {
    const [images, setImages] = useState([]);
    
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
                                accept=".png, .jpg, .pdf, .zip, .webp, .docx"
                                multiple={true}
                                generalErrorMessage={props.generalErrorMessage}
                                deleteLabel={props.deleteLabel}
                                dropFilesLabel={props.dropFilesLabel}
                                dropOrSelectFilesLabel={props.dropOrSelectImagesLabel}
                                files={images}
                                isUploadInChunksEnabled={true}
                                chunkSize={props.chunkSize}
                                saveMediaChunkUrl={props.saveMediaChunkUrl}
                                saveMediaChunkCompleteUrl={props.saveMediaChunkCompleteUrl}
                                setFieldValue={({value}) => setImages(value)}
                                saveMediaUrl={props.saveMediaUrl} />
                        </div>
                        <div className="field">
                            <a href={props.mediaUrl} className="button is-text">{props.backToMediaText}</a>
                        </div>
                    </form>
                </div>
            </div>
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
    saveMediaChunkCompleteUrl: PropTypes.string.isRequired
}

export default MediaForm;
