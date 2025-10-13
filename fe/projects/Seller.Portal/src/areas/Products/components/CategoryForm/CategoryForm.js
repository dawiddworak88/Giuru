import React, { useContext } from "react";
import { toast } from "react-toastify";
import PropTypes from "prop-types";
import { Context } from "../../../../shared/stores/Store";
import useForm from "../../../../shared/helpers/forms/useForm";
import { TextField, Select, FormControl, InputLabel, MenuItem, Button, CircularProgress } from "@mui/material";
import MediaCloud from "../../../../shared/components/MediaCloud/MediaCloud";
import AuthenticationHelper from "../../../../shared/helpers/globals/AuthenticationHelper";

function CategoryForm(props) {
    const [state, dispatch] = useContext(Context);
    const { categoryBase } = props;

    const stateSchema = {
        id: { value: props.id ? props.id : null, error: "" },
        name: { value: props.name ? props.name : "", error: "" },
        parentCategoryId: { value: props.parentCategoryId ? props.parentCategoryId : "" },
        files: { value: props.files ? props.files : [] },
        schemas: { value: props.schemas ? props.schemas : null},         
        uiSchema: { value: props.uiSchema ? JSON.parse(props.uiSchema) : null }
    };    

    const stateValidatorSchema = {
        name: {
            required: {
                isRequired: true,
                error: categoryBase.nameRequiredErrorMessage
            }
        }
    };  

    function onSubmitForm(state) {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        const requestPayload = {
            id,
            name,
            parentCategoryId,
            files,
            schemas: state.schemas
        };        
        
        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" },
            body: JSON.stringify(requestPayload)
        };

        fetch(categoryBase.saveUrl, requestOptions)
            .then(function (response) {
                dispatch({ type: "SET_IS_LOADING", payload: false });

                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {

                    if (response.ok) {
                        setFieldValue({ name: "id", value: jsonResponse.id });
                        toast.success(jsonResponse.message);
                    }
                    else {
                        toast.error(jsonResponse?.message || categoryBase.generalErrorMessage);
                    }
                });
            }).catch(() => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(categoryBase.generalErrorMessage);
            });
    }

    const {
        values, errors, dirty, disable, 
        setFieldValue, handleOnChange, handleOnSubmit
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm, !categoryBase.id);

    const { id, name, parentCategoryId, files } = values;

    return (
        <section className="section section-small-padding category">
            <h1 className="subtitle is-4">{categoryBase.title}</h1>
            <div className="columns is-desktop">
                <div className="column is-half">
                    <form className="is-modern-form" onSubmit={handleOnSubmit} method="post">
                        {id &&
                            <div className="field">
                                <InputLabel id="id-label">{categoryBase.idLabel} {id}</InputLabel>
                            </div>
                        }
                        <div className="field">
                            <TextField id="name" name="name" label={categoryBase.nameLabel} fullWidth={true} variant="standard"
                                value={name} onChange={handleOnChange} helperText={dirty.name ? errors.name : ""} error={(errors.name.length > 0) && dirty.name} />
                        </div>
                        <div className="field">
                            <FormControl fullWidth={true} variant="standard">
                                <InputLabel id="parent-category">{categoryBase.parentCategoryLabel}</InputLabel>
                                <Select
                                    labelId="parent-category"
                                    id="parentCategoryId"
                                    name="parentCategoryId"
                                    value={parentCategoryId}
                                    onChange={handleOnChange}>
                                    <MenuItem value="">&nbsp;</MenuItem>
                                    {categoryBase.parentCategories && categoryBase.parentCategories.map(category =>
                                        <MenuItem key={category.id} value={category.id}>{category.name}</MenuItem>
                                    )}
                                </Select>
                            </FormControl>
                        </div>
                        <div className="field">
                            <MediaCloud
                                id="files"
                                name="files"
                                label={categoryBase.productPicturesLabel}
                                multiple={false}
                                generalErrorMessage={categoryBase.generalErrorMessage}
                                deleteLabel={categoryBase.deleteLabel}
                                dropFilesLabel={categoryBase.dropFilesLabel}
                                dropOrSelectFilesLabel={categoryBase.dropOrSelectFilesLabel}
                                files={files}
                                setFieldValue={setFieldValue}
                                saveMediaUrl={categoryBase.saveMediaUrl} 
                                accept={{
                                    "image/*": [".png", ".jpg", ".webp"]
                                }}/>
                        </div>
                        <div className="field">
                            <Button 
                                type="submit" 
                                variant="contained" 
                                color="primary" 
                                disabled={state.isLoading || disable}>
                                {categoryBase.saveText}
                            </Button>
                            <a href={categoryBase.categoriesUrl} className="ml-2 button is-text">{categoryBase.navigateToCategoriesLabel}</a>
                        </div>
                    </form>
                </div>
            </div>
            {state.isLoading && <CircularProgress className="progressBar" />}
        </section>
    );
}

CategoryForm.propTypes = {
    id: PropTypes.string,
    name: PropTypes.string,
    parentCategoryId: PropTypes.string,
    files: PropTypes.array,
    categoryBase: PropTypes.object
};

export default CategoryForm;
