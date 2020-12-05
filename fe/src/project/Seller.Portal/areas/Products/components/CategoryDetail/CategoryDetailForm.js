import React, { useContext } from "react";
import { toast } from "react-toastify";
import PropTypes from "prop-types";
import { Context } from "../../../../../../shared/stores/Store";
import useForm from "../../../../../../shared/helpers/forms/useForm";
import { TextField, Select, FormControl, InputLabel, MenuItem, Button, CircularProgress } from "@material-ui/core";

function CategoryDetailForm(props) {

    const [state, dispatch] = useContext(Context);

    const stateSchema = {

        id: { value: props.category ? props.category.id : null, error: "" },
        name: { value: props.category ? props.category.name : "", error: "" },
        parentCategory: { value: props.category ? props.category.parentCategory: "" }
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
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm, !props.category);

    const { name, parentCategory } = values;

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
                                    value={parentCategory}
                                    onChange={handleOnChange}>
                                        <MenuItem value="">&nbsp;</MenuItem>
                                        {props.parentCategories && props.parentCategories.map(category => 
                                            <MenuItem key={category.id} value={category.id}>{category.name}</MenuItem>
                                        )}
                                </Select>
                            </FormControl>
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
    parentCategories: PropTypes.array.isRequired
};

export default CategoryDetailForm;
