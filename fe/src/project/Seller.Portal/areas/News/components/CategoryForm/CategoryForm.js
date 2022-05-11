import React, { useContext, useState } from "react";
import { toast } from "react-toastify";
import PropTypes from "prop-types";
import { Context } from "../../../../../../shared/stores/Store";
import useForm from "../../../../../../shared/helpers/forms/useForm";
import AuthenticationHelper from "../../../../../../shared/helpers/globals/AuthenticationHelper";
import NavigationHelper from "../../../../../../shared/helpers/globals/NavigationHelper";
import { 
    TextField, Select, FormControl, InputLabel, MenuItem, Button, CircularProgress 
} from "@material-ui/core";

const CategoryForm = (props) => {
    const [state, dispatch] = useContext(Context);
    const stateSchema = {
        id: { value: props.id ? props.id : null, error: "" },
        name: { value: props.name ? props.name : "", error: "" },
        parentCategoryId: { value: props.parentCategoryId ? props.parentCategoryId : null, error: "" },
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
                error: props.nameRequiredErrorMessage
            }
        }
    };

    const {
        values, errors, dirty, disable,
        setFieldValue, handleOnChange, handleOnSubmit
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm, !props.id);

    const { id, name, parentCategoryId } = values;
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
                                helperText={dirty.name ? errors.name : ""} 
                                error={(errors.name.length > 0) && dirty.name} 
                            />
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
                                    <MenuItem key={0} value="">{props.selectCategoryLabel}</MenuItem>
                                    {props.parentCategories && props.parentCategories.map(category =>
                                        <MenuItem key={category.id} value={category.id}>{category.name}</MenuItem>
                                    )}
                                </Select>
                            </FormControl>
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
    selectCategoryLabel: PropTypes.string.isRequired,
    parentCategoryLabel: PropTypes.string.isRequired,
    title: PropTypes.string.isRequired,
    nameLabel: PropTypes.string.isRequired,
    saveText: PropTypes.string.isRequired,
    generalErrorMessage: PropTypes.string.isRequired,
    parentCategories: PropTypes.array.isRequired,
    saveUrl: PropTypes.string.isRequired,
    idLabel: PropTypes.string
};

export default CategoryForm;