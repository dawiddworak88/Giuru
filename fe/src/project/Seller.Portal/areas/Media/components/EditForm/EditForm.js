import React, {useState} from "react";
import PropTypes from "prop-types"; 
import MediaCloud from "../../../../../../shared/components/MediaCloud/MediaCloud";

const EditForm = (props) => {
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
                    </form>
                </div>
            </div>
        </section>
    )
}

export default EditForm;