import React, { useContext, useCallback, useEffect } from "react";
import { toast } from "react-toastify";
import PropTypes from "prop-types";
import { UploadCloud } from "react-feather";
import { Context } from "../../../../../../shared/stores/Store";
import useForm from "../../../../../../shared/helpers/forms/useForm";
import IconConstants from "../../../../../../shared/constants/IconConstants";
import { TextField, Select, FormControl, InputLabel, MenuItem, Button, CircularProgress } from "@material-ui/core";
import { useDropzone } from "react-dropzone";

function CategoryDetailForm(props) {

    const [state, dispatch] = useContext(Context);

    const stateSchema = {

        id: { value: props.id ? props.id : null, error: "" },
        name: { value: props.name ? props.name : "", error: "" },
        parentCategoryId: { value: props.parentCategoryId ? props.parentCategoryId : "" },
        files: { value: props.files ? props.files : [] }
    };

    const stateValidatorSchema = {

        name: {
            required: {
                isRequired: true,
                error: props.nameRequiredErrorMessage
            }
        }
    };

    function onSubmitForm(state) {

        dispatch({ type: "SET_IS_LOADING", payload: true });

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(state)
        };

        fetch(props.saveUrl, requestOptions)
            .then(function (response) {

                dispatch({ type: "SET_IS_LOADING", payload: false });

                return response.json().then(jsonResponse => {

                    if (response.ok) {

                        setFieldValue({ name: "id", value: jsonResponse.data.id });
                        toast.success(jsonResponse.message);
                    }
                    else {
                        toast.error(props.generalErrorMessage);
                    }
                });
            }).catch(() => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(props.generalErrorMessage);
            });
    }

    const {
        values,
        errors,
        dirty,
        disable,
        setFieldValue,
        handleOnChange,
        handleOnSubmit
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm, !props.id);

    const { id, name, parentCategoryId, files } = values;


    function deleteMedia(id) {

        console.log(id);
    }

      useEffect(() => () => {
        files.forEach(file => URL.revokeObjectURL(file.url));
      }, [files]);

      const onDrop = useCallback(acceptedFiles => {

        dispatch({ type: "SET_IS_LOADING", payload: true });

        acceptedFiles.forEach((file) => {

            const formData = new FormData();

            formData.append("file", file);

            const requestOptions = {
                method: "POST",
                body: formData
            };

            fetch(props.saveMediaUrl, requestOptions)
                .then(function (response) {

                    dispatch({ type: "SET_IS_LOADING", payload: false });

                    return response.json().then(() => {

                        if (response.ok) {
                            dispatch({ type: "SET_IS_LOADING", payload: false });
                        }
                        else {
                            toast.error(props.generalErrorMessage);
                        }
                    });
                }).catch(() => {
                    dispatch({ type: "SET_IS_LOADING", payload: false });
                    toast.error(props.generalErrorMessage);
                });
        });
    }, [dispatch, state, props.saveMediaUrl, props.generalErrorMessage]);

    const { getRootProps, getInputProps, isDragActive } = useDropzone({
        onDrop,
        accept: ".png, .jpg",
        multiple: false
    });

    return (
        <section className="section section-small-padding">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div className="columns is-desktop">
                <div className="column is-half">
                    <form className="is-modern-form" onSubmit={handleOnSubmit} method="post">
                        <div className="field">
                            <TextField id="name" name="name" label={props.nameLabel} fullWidth={true}
                                value={name} onChange={handleOnChange} helperText={dirty.name ? errors.name : ""} error={(errors.name.length > 0) && dirty.name} />
                        </div>
                        <div className="field">
                            <FormControl fullWidth={true}>
                                <InputLabel id="parent-category">{props.parentCategoryLabel}</InputLabel>
                                <Select
                                    labelId="parent-category"
                                    id="parentCategory"
                                    name="parentCategory"
                                    value={parentCategoryId}
                                    onChange={handleOnChange}>
                                        <MenuItem value="">&nbsp;</MenuItem>
                                        {props.parentCategories && props.parentCategories.map(category => 
                                            <MenuItem key={category.id} value={category.id}>{category.name}</MenuItem>
                                        )}
                                </Select>
                            </FormControl>
                        </div>
                        <div className={"field"}>
                            <div className="dropzone">
                                {props.categoryPictureLabel &&
                                    <label className="dropzone__title" for="media">{props.categoryPictureLabel}</label>
                                }
                                <div className="dropzone__pond-container" {...getRootProps()}>
                                    <input id="media" name="media" {...getInputProps()} />
                                    <div className={isDragActive ? "dropzone__pond dropzone--active" : "dropzone__pond"}>
                                        <p>
                                            <UploadCloud size={IconConstants.defaultSize()} />
                                        </p>
                                        <p>{isDragActive ? props.dropOrSelectFilesLabel : props.dropFilesLabel}</p>
                                    </div>
                                </div>
                                {files &&
                                    <aside className="dropzone__preview">
                                        {files.map((file) =>
                                            <div className="dropzone__preview-thumbnail">
                                                <div>
                                                    <img src={file.url} />
                                                </div>
                                                <div className="is-flex is-flex-centered has-text-cenetered">
                                                    <Button type="button" type="contained" color="primary" onClick={() => deleteMedia(file.id)}>
                                                        {props.deleteLabel}
                                                    </Button>
                                                </div>
                                            </div>
                                        )}
                                    </aside>
                                }
                            </div>
                        </div>
                        <div className="field">
                            <Button type="submit" variant="contained" color="primary" disabled={state.isLoading || disable}>
                                {props.saveText}
                            </Button>
                        </div>
                    </form>
                </div>
            </div>
            {state.isLoading && <CircularProgress className="progressBar" />}
        </section>
    );
}

CategoryDetailForm.propTypes = {
    selectCategoryLabel: PropTypes.string.isRequired,
    parentCategoryLabel: PropTypes.string.isRequired,
    title: PropTypes.string.isRequired,
    nameLabel: PropTypes.string.isRequired,
    saveText: PropTypes.string.isRequired,
    generalErrorMessage: PropTypes.string.isRequired,
    parentCategories: PropTypes.array.isRequired,
    dropOrSelectFilesLabel: PropTypes.string.isRequired,
    dropFilesLabel: PropTypes.string.isRequired,
    saveMediaUrl: PropTypes.string.isRequired,
    deleteLabel: PropTypes.string.isRequired,
    categoryPictureLabel: PropTypes.string.isRequired
};

export default CategoryDetailForm;
