import React, { useContext } from "react";
import { toast } from "react-toastify";
import PropTypes from "prop-types";
import { Context } from "../../../../../../shared/stores/Store";
import useForm from "../../../../../../shared/helpers/forms/useForm";
import { TextField, Select, FormControl, InputLabel, MenuItem, Button, CircularProgress } from "@material-ui/core";
import MediaCloud from "../../../../../../shared/components/MediaCloud/MediaCloud";
import AuthenticationHelper from "../../../../../../shared/helpers/globals/AuthenticationHelper";

function CategoryForm(props) {

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
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" },
            body: JSON.stringify(state)
        };

        fetch(props.saveUrl, requestOptions)
            .then(function (response) {

                dispatch({ type: "SET_IS_LOADING", payload: false });

                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {

                    if (response.ok) {

                        setFieldValue({ name: "id", value: jsonResponse.id });
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

    return (
        <section className="section section-small-padding category">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div className="columns is-desktop">
                <div className="column is-half">
                    <form className="is-modern-form" onSubmit={handleOnSubmit} method="post">
                        {id &&
                            <input id="id" name="id" type="hidden" value={id} />
                        }
                        <div className="field">
                            <TextField id="name" name="name" label={props.nameLabel} fullWidth={true}
                                value={name} onChange={handleOnChange} helperText={dirty.name ? errors.name : ""} error={(errors.name.length > 0) && dirty.name} />
                        </div>
                        <div className="field">
                            <FormControl fullWidth={true}>
                                <InputLabel id="parent-category">{props.parentCategoryLabel}</InputLabel>
                                <Select
                                    labelId="parent-category"
                                    id="parentCategoryId"
                                    name="parentCategoryId"
                                    value={parentCategoryId}
                                    onChange={handleOnChange}>
                                    <MenuItem value="">&nbsp;</MenuItem>
                                    {props.parentCategories && props.parentCategories.map(category =>
                                        <MenuItem key={category.id} value={category.id}>{category.name}</MenuItem>
                                    )}
                                </Select>
                            </FormControl>
                        </div>
                        <div className="field">
                        <MediaCloud
                                id="files"
                                name="files"
                                label={props.productPicturesLabel}
                                accept=".png, .jpg"
                                multiple={false}
                                generalErrorMessage={props.generalErrorMessage}
                                deleteLabel={props.deleteLabel}
                                dropFilesLabel={props.dropFilesLabel}
                                dropOrSelectFilesLabel={props.dropOrSelectFilesLabel}
                                files={files}
                                setFieldValue={setFieldValue}
                                saveMediaUrl={props.saveMediaUrl} />
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

CategoryForm.propTypes = {
    id: PropTypes.string,
    name: PropTypes.string,
    parentCategoryId: PropTypes.string,
    files: PropTypes.array,
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
    categoryPictureLabel: PropTypes.string.isRequired,
    saveUrl: PropTypes.string.isRequired
};

export default CategoryForm;
