import React, { useContext } from "react";
import { toast } from "react-toastify";
import PropTypes from "prop-types";
import { Context } from "../../../../../../shared/stores/Store";
import useForm from "../../../../../../shared/helpers/forms/useForm";
import AuthenticationHelper from "../../../../../../shared/helpers/globals/AuthenticationHelper";
import NavigationHelper from "../../../../../../shared/helpers/globals/NavigationHelper";
import MediaCloud from "../../../../../../shared/components/MediaCloud/MediaCloud";
import { 
    TextField, InputLabel, Button, CircularProgress, 
    NoSsr, FormControlLabel, Switch, Autocomplete
} from "@mui/material";

const CategoryForm = (props) => {
    const [state, dispatch] = useContext(Context);
    const stateSchema = {
        id: { value: props.id ? props.id : null },
        name: { value: props.name ? props.name : "", error: "" },
        parentCategoryId: { value: props.parentCategoryId ? props.parentCategories.find((item) => item.id === props.parentCategoryId) : null },
        files: { value: props.files ? props.files : [] },
        isVisible: { value: props.isVisible ? props.isVisible : false }
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
            .then((res) => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                
                AuthenticationHelper.HandleResponse(res);
                
                return res.json().then(jsonRes => {
                    if (res.ok) {
                        toast.success(jsonRes.message);
                        setFieldValue({ name: "id", value: jsonRes.id });
                    }
                    else {
                        toast.error(props.generalErrorMessage);
                    }
                });
            });
    }

    const stateValidatorSchema = {
        name: {
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

    const { id, name, parentCategoryId, files, isVisible } = values;
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
                            <TextField 
                                id="name" 
                                name="name" 
                                label={props.nameLabel} 
                                fullWidth={true}
                                value={name} 
                                onChange={handleOnChange} 
                                variant="standard"
                                helperText={dirty.name ? errors.name : ""} 
                                error={(errors.name.length > 0) && dirty.name} 
                            />
                        </div>
                        <div className="field">
                            <Autocomplete
                                options={props.parentCategories}
                                getOptionLabel={(option) => option.name}
                                id="parentCategoryId"
                                name="parentCategoryId"
                                fullWidth={true}
                                value={parentCategoryId}
                                variant="standard"
                                onChange={(event, newValue) => {
                                    setFieldValue({name: "parentCategoryId", value: newValue.id});
                                }}
                                autoComplete={true}
                                renderInput={(params) => (
                                    <TextField 
                                        {...params} 
                                        label={props.parentCategoryLabel} 
                                        variant="standard"
                                        margin="normal"/>
                                )}/>
                        </div>
                        <div className="field">
                            <MediaCloud
                                id="files"
                                name="files"
                                label={props.filesLabel}
                                accept=".png, .jpg, .webp, .zip, .pdf, .docx, .xls, .xlsx"
                                multiple={true}
                                generalErrorMessage={props.generalErrorMessage}
                                deleteLabel={props.deleteLabel}
                                dropFilesLabel={props.dropFilesLabel}
                                dropOrSelectFilesLabel={props.dropOrSelectFilesLabel}
                                files={files}
                                setFieldValue={setFieldValue}
                                saveMediaUrl={props.saveMediaUrl} />
                        </div>
                        <div className="field">
                            <NoSsr>
                                <FormControlLabel
                                    control={
                                        <Switch
                                            onChange={e => {
                                                setFieldValue({ name: "isVisible", value: e.target.checked });
                                            }}
                                            checked={isVisible}
                                            id="isVisible"
                                            name="isVisible"
                                            color="secondary" 
                                        />
                                    }
                                    label={props.visibleLabel} />
                            </NoSsr>
                        </div>
                        <div className="field">
                            <Button 
                                type="submit"
                                variant="contained" 
                                color="primary"
                                disabled={state.isLoading || disable}>
                                {props.saveText}
                            </Button>
                            <Button
                                className="ml-2"
                                type="button" 
                                variant="contained" 
                                color="secondary" 
                                onClick={(e) => {
                                    e.preventDefault();
                                    NavigationHelper.redirect(props.categoriesUrl);
                                }}>
                                {props.navigateToCategoriesLabel}
                            </Button> 
                        </div>
                    </form>
                </div>
            </div>
            {state.isLoading && <CircularProgress className="progressBar" />}
        </section>
    )
}

CategoryForm.propTypes = {
    id: PropTypes.string,
    name: PropTypes.string,
    parentCategoryId: PropTypes.string,
    files: PropTypes.array,
    parentCategoryLabel: PropTypes.string.isRequired,
    title: PropTypes.string.isRequired,
    nameLabel: PropTypes.string.isRequired,
    saveText: PropTypes.string.isRequired,
    generalErrorMessage: PropTypes.string.isRequired,
    parentCategories: PropTypes.array,
    saveUrl: PropTypes.string.isRequired,
    idLabel: PropTypes.string,
    navigateToCategoriesLabel: PropTypes.string.isRequired,
    categoriesUrl: PropTypes.string.isRequired,
    visibleLabel: PropTypes.string.isRequired,
    saveMediaUrl: PropTypes.string.isRequired,
    dropOrSelectFilesLabel: PropTypes.string.isRequired,
    dropFilesLabel: PropTypes.string.isRequired,
    deleteLabel: PropTypes.string.isRequired,
    filesLabel: PropTypes.string.isRequired,
    fieldRequiredErrorMessage: PropTypes.string.isRequired
};

export default CategoryForm;