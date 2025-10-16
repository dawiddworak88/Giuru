import React, { useContext } from "react";
import { toast } from "react-toastify";
import PropTypes from "prop-types";
import { Context } from "../../../../shared/stores/Store";
import useForm from "../../../../shared/helpers/forms/useForm";
import AuthenticationHelper from "../../../../shared/helpers/globals/AuthenticationHelper";
import { 
    TextField, InputLabel, Button, CircularProgress, 
    NoSsr, FormControlLabel, Switch, Autocomplete
} from "@mui/material";

const DownloadCenterCategoryForm = (props) => {
    const [state, dispatch] = useContext(Context);
    const stateSchema = {
        id: { value: props.id ? props.id : null },
        name: { value: props.name ? props.name : "", error: "" },
        parentCategory: { value: props.parentCategoryId ? props.parentCategories.find((item) => item.id === props.parentCategoryId) : null },
        isVisible: { value: props.isVisible ? props.isVisible : false }
    };

    const onSubmitForm = (state) => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        const requestPayload = { 
            id, 
            name, 
            parentCategoryId: parentCategory ? parentCategory.id : null, 
            isVisible
        };

        const requestOptions = {
            method: "POST",
            headers: { 
                "Content-Type": "application/json" 
            },
            body: JSON.stringify(requestPayload)
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
            }).catch(() => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(props.generalErrorMessage);
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

    const { id, name, parentCategory, isVisible } = values;

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
                                id="parentCategory "
                                name="parentCategory"
                                fullWidth={true}
                                value={parentCategory}
                                variant="standard"
                                onChange={(event, newValue) => {
                                    setFieldValue({name: "parentCategory", value: newValue});
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
                            <a href={props.categoriesUrl} className="ml-2 button is-text">{props.navigateToCategoriesLabel}</a>
                        </div>
                    </form>
                </div>
            </div>
            {state.isLoading && <CircularProgress className="progressBar" />}
        </section>
    )
}

DownloadCenterCategoryForm.propTypes = {
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
    filesLabel: PropTypes.string.isRequired,
    fieldRequiredErrorMessage: PropTypes.string.isRequired
};

export default DownloadCenterCategoryForm;