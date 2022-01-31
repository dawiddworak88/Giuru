import React, {useState, useContext} from "react";
import PropTypes from "prop-types"; 
import {
    TextField, Button, 
    FormControl, InputLabel, Select, MenuItem, FormHelperText, CircularProgress, IconButton
} from "@material-ui/core";
import MediaCloud from "../../../../../../shared/components/MediaCloud/MediaCloud";
import useForm from "../../../../../../shared/helpers/forms/useForm";
import { Context } from "../../../../../../shared/stores/Store";

const EditForm = (props) => {
    const [state, dispatch] = useContext(Context);
    const [showBackToMediaListButton, setShowBackToMediaListButton] = useState(false);
    const stateSchema = {
        id: { value: props.id ? props.id : null, error: "" },
        images: { value: props.images ? props.images : null, error: "" },
        name: {value: props.name ? props.name : null, error: ""},
        description: {value: props.description ? props.description : null, error: ""}
    };

    const stateValidatorSchema = {
        images: {
            required: {
                isRequired: true,
                error: props.imagesRequiredErrorMessage
            }
        }
    }

    const onSubmitForm = (state) => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        
    }

    const {
        values, errors, dirty, disable, setFieldValue,
        handleOnChange, handleOnSubmit
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm, !props.id);

    const {images, name, description} = values;
    return (
        <section className="section section-small-padding product client-form">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div className="columns is-desktop">
                <div className="column is-half">
                    <form className="is-modern-form" onSubmit={handleOnSubmit}>
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
                                setFieldValue={setFieldValue}
                                saveMediaUrl={props.saveMediaUrl} />

                                {errors.images && dirty.images && (
                                    <FormHelperText>{errors.images}</FormHelperText>
                                )}
                        </div>
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
                            {showBackToMediaListButton ? (
                                <Button type="submit" variant="contained" color="primary">
                                    {props.backToMediaText}
                                </Button>
                            ): (
                                <Button type="submit" variant="contained" color="primary" disabled={disable || state.isLoading}>
                                    {props.saveMediaText}
                                </Button>
                            )}
                        </div>
                    </form>
                    {state.isLoading && <CircularProgress className="progressBar" />}
                </div>
            </div>
        </section>
    )
}

export default EditForm;