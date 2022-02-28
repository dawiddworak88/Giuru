import React, { useContext } from "react";
import { toast } from "react-toastify";
import PropTypes from "prop-types";
import { Context } from "../../../../../../shared/stores/Store";
import useForm from "../../../../../../shared/helpers/forms/useForm";
import AuthenticationHelper from "../../../../../../shared/helpers/globals/AuthenticationHelper";
import NavigationHelper from "../../../../../../shared/helpers/globals/NavigationHelper";
import MediaCloud from "../../../../../../shared/components/MediaCloud/MediaCloud";
import { 
    TextField, Select, FormControl, InputLabel, MenuItem, Button, CircularProgress 
} from "@material-ui/core";

const NewsItemForm = (props) => {
    const [state, dispatch] = useContext(Context);
    const stateSchema = {
        id: { value: props.id ? props.id : null, error: "" },
        title: { value: props.newsTitle ? props.newsTitle : "", error: "" },
        heroImage: { value: props.images ? props.images : [] },
    }

    const onSubmitForm = () => {

    }

    const stateValidatorSchema = () => {

    }

    const {
        values, errors, dirty, disable,
        setFieldValue, handleOnChange, handleOnSubmit
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm, !props.id);

    const { title, heroImage } = values;
    return (
        <section className="section section-small-padding category">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div className="columns is-desktop">
                <div className="column is-half">
                    <form className="is-modern-form" onSubmit={handleOnSubmit}>
                        <div className="field">
                            <MediaCloud
                                id="heroImage"
                                name="heroImage"
                                label={props.heroImageLabel}
                                accept=".png, .jpg, .webp"
                                multiple={false}
                                generalErrorMessage={props.generalErrorMessage}
                                deleteLabel={props.deleteLabel}
                                dropFilesLabel={props.dropFilesLabel}
                                dropOrSelectFilesLabel={props.dropOrSelectImagesLabel}
                                files={heroImage}
                                setFieldValue={setFieldValue}
                                saveMediaUrl={props.saveMediaUrl} />
                        </div>
                        <div className="field">
                            <TextField 
                                id="title" 
                                name="title"
                                label={props.titleLabel} 
                                fullWidth={true}
                                value={title} 
                                onChange={handleOnChange} 
                            />
                        </div>
                        <div className="field">

                        </div>
                    </form>
                </div>
            </div>
        </section>
    )
}

NewsItemForm.propTypes = {
    id: PropTypes.string,
    title: PropTypes.string.isRequired,
}

export default NewsItemForm;