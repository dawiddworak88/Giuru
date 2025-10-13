import React, { useContext } from "react";
import { toast } from "react-toastify";
import { Context } from "../../../../shared/stores/Store";
import useForm from "../../../../shared/helpers/forms/useForm";
import AuthenticationHelper from "../../../../shared/helpers/globals/AuthenticationHelper";
import PropTypes from "prop-types";
import MediaCloud from "../../../../shared/components/MediaCloud/MediaCloud";
import { 
    Select, FormControl, InputLabel, MenuItem,
    Button, CircularProgress, FormHelperText
} from "@mui/material";

const DownloadCenterItemForm = (props) => {
    const [state, dispatch] = useContext(Context);
    const stateSchema = {
        id: { value: props.id ? props.id : null, error: "" },
        categoriesIds: { value: props.categoriesIds ? props.categoriesIds : [], error: "" },
        files: { value: props.files ? props.files : [] }
    };

    const onSubmitForm = (state) => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        const requestOptions = {
            method: "POST",
            headers: { 
                "Content-Type": "application/json" 
            },
            body: JSON.stringify(state)
        };

        fetch(props.saveUrl, requestOptions)
            .then((response) => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                
                AuthenticationHelper.HandleResponse(response);
                
                return response.json().then(jsonResponse => {
                    if (response.ok) {
                        toast.success(jsonResponse.message);
                        setFieldValue({ name: "id", value: jsonResponse.id });
                    }
                    else {
                        toast.error(jsonResponse?.message || props.generalErrorMessage);
                    }
                });
            });
    }

    const stateValidatorSchema = {
        categoriesIds: {
            required: {
                isRequired: true,
                error: props.fieldRequiredErrorMessage
            }
        }
    };

    const {
        values, errors, dirty, disable,
        setFieldValue, handleOnChange, handleOnSubmit
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm, !props.id);

    const { id, categoriesIds, files } = values;

    return (
        <section className="section section-small-padding category">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div className="columns is-desktop">
                <div className="column is-half">
                    <form className="is-modern-form" onSubmit={handleOnSubmit}>
                        {id &&
                            <div className="field">
                                <InputLabel id="id-label">{props.idLabel} {id}</InputLabel>
                            </div>
                        }
                        <div className="field">
                            <FormControl fullWidth={true} variant="standard" error={(errors.categoriesIds.length > 0) && dirty.categoryId}>
                                <InputLabel id="categoriesIds-label">{props.categoriesLabel}</InputLabel>
                                <Select
                                    labelId="categoriesIds-label"
                                    id="categoriesIds"
                                    name="categoriesIds"
                                    value={categoriesIds}
                                    multiple={true}
                                    onChange={handleOnChange}>
                                    {props.categories && props.categories.map(category =>
                                        <MenuItem key={category.id} value={category.id}>{category.name}</MenuItem>
                                    )}
                                </Select>
                                {errors.categoriesIds && dirty.categoriesIds && (
                                    <FormHelperText>{errors.categoriesIds}</FormHelperText>
                                )}
                            </FormControl>
                        </div>
                        <div className="field">
                            <MediaCloud
                                id="files"
                                name="files"
                                label={props.filesLabel}
                                multiple={true}
                                generalErrorMessage={props.generalErrorMessage}
                                deleteLabel={props.deleteLabel}
                                dropFilesLabel={props.dropFilesLabel}
                                dropOrSelectFilesLabel={props.dropOrSelectFilesLabel}
                                files={files}
                                setFieldValue={setFieldValue}
                                saveMediaUrl={props.saveMediaUrl}
                                isUploadInChunksEnabled={props.isUploadInChunksEnabled}
                                chunkSize={props.chunkSize}
                                saveMediaChunkUrl={props.saveMediaChunkUrl}
                                saveMediaChunkCompleteUrl={props.saveMediaChunkCompleteUrl} 
                                accept={{
                                    "image/*": [".png", ".jpg", ".webp"],
                                    "application/*": [".pdf", ".docx", ".doc", ".zip", ".xls", ".xlsx"],
                                    "video/*": [".mp4"]
                                }}/>
                        </div>
                        <div className="field">
                            <Button 
                                type="submit" 
                                variant="contained" 
                                color="primary"
                                disabled={state.isLoading || disable || files.length == 0}>
                                {props.saveText}
                            </Button>
                            <a href={props.downloadCenterUrl} className="ml-2 button is-text">{props.navigateToDownloadCenterLabel}</a>
                        </div>
                    </form>
                </div>
            </div>
            {state.isLoading && <CircularProgress className="progressBar" />}
        </section>
    )
}

DownloadCenterItemForm.propTypes = {
    navigateToDownloadCenterLabel: PropTypes.string.isRequired,
    downloadCenterUrl: PropTypes.string.isRequired,
    saveText: PropTypes.string.isRequired,
    saveMediaUrl: PropTypes.string.isRequired,
    dropOrSelectFilesLabel: PropTypes.string.isRequired,
    dropFilesLabel: PropTypes.string.isRequired,
    deleteLabel: PropTypes.string.isRequired,
    generalErrorMessage: PropTypes.string.isRequired,
    filesLabel: PropTypes.string.isRequired,
    categoriesIds: PropTypes.array,
    categories: PropTypes.array,
    categoriesLabel: PropTypes.string,
    idLabel: PropTypes.string,
    id: PropTypes.string,
    title: PropTypes.string.isRequired,
    files: PropTypes.array,
    isUploadInChunksEnabled: PropTypes.bool.isRequired,
    chunkSize: PropTypes.string.isRequired,
    saveMediaChunkUrl: PropTypes.string.isRequired,
    saveMediaChunkCompleteUrl: PropTypes.string.isRequired
}

export default DownloadCenterItemForm;