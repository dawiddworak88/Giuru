import React, { useContext } from "react";
import { toast } from "react-toastify";
import { Context } from "../../../../../../shared/stores/Store";
import useForm from "../../../../../../shared/helpers/forms/useForm";
import AuthenticationHelper from "../../../../../../shared/helpers/globals/AuthenticationHelper";
import NavigationHelper from "../../../../../../shared/helpers/globals/NavigationHelper";
import PropTypes from "prop-types";
import { 
    TextField, InputLabel, Button, CircularProgress, Autocomplete
} from "@mui/material";

const DownloadCenterItemForm = (props) => {
    const [state, dispatch] = useContext(Context);
    const stateSchema = {
        id: { value: props.id ? props.id : null, error: "" },
        categoryId: { value: props.categoryId ? props.categories.find((item) => item.id === props.categoryId) : null, error: "" },
        order: { value: props.order ? props.order : null }
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
        categoryId: {
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

    const { id, categoryId, order } = values;
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
                            <Autocomplete
                                options={props.categories}
                                getOptionLabel={(option) => option.name}
                                id="categoryId"
                                name="categoryId"
                                fullWidth={true}
                                value={categoryId}
                                variant="standard"
                                onChange={(event, newValue) => {
                                    setFieldValue({name: "categoryId", value: newValue.id});
                                }}
                                autoComplete={true}
                                renderInput={(params) => (
                                    <TextField 
                                        {...params} 
                                        label={props.categoryLabel}
                                        helperText={dirty.categoryId ? errors.categoryId : ""} 
                                        error={(errors.categoryId.length > 0) && dirty.categoryId}
                                        variant="standard"
                                        margin="normal"/>
                                )}/>
                        </div>
                        <div className="field">
                            <TextField 
                                id="order" 
                                name="order" 
                                label={props.orderLabel} 
                                fullWidth={true}
                                value={order}
                                inputProps={{ 
                                    min: 0, 
                                    step: 1,
                                }}
                                type="number"
                                onChange={handleOnChange} 
                                variant="standard"/>
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
                                    NavigationHelper.redirect(props.downloadCenterUrl);
                                }}>
                                {props.navigateToDownloadCenterLabel}
                            </Button> 
                        </div>
                    </form>
                </div>
            </div>
            {state.isLoading && <CircularProgress className="progressBar" />}
        </section>
    )
}

DownloadCenterItemForm.propTypes = {
    idLabel: PropTypes.string,
    categoryLabel: PropTypes.string.isRequired,
    orderLabel: PropTypes.string.isRequired,
    saveUrl: PropTypes.string.isRequired,
    saveText: PropTypes.string.isRequired,
    order: PropTypes.number,
    navigateToDownloadCenterLabel: PropTypes.string.isRequired,
    downloadCenterUrl: PropTypes.string.isRequired,
    fieldRequiredErrorMessage: PropTypes.string.isRequired,
    categoryId: PropTypes.string,
    id: PropTypes.string,
    title: PropTypes.string.isRequired,
    generalErrorMessage: PropTypes.string.isRequired,
    categories: PropTypes.array
}

export default DownloadCenterItemForm;