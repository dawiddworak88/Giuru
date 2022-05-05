import React, {useContext} from "react";
import PropTypes from "prop-types";
import { toast } from "react-toastify";
import { Context } from "../../../../../../shared/stores/Store";
import { TextField, Button, InputLabel } from "@mui/material";
import useForm from "../../../../../../shared/helpers/forms/useForm";
import NavigationHelper from "../../../../../../shared/helpers/globals/NavigationHelper";

const WarehouseForm = (props) => {

    const [state, dispatch] = useContext(Context);
    const stateSchema = {
        id: { value: props.id ? props.id : null, error: "" },
        name: { value: props.name ? props.name : null, error: "" },
        location: { value: props.location ? props.location : null, error: "" },
    };

    const stateValidatorSchema = {
        name: {
            required: {
                isRequired: true,
                error: props.nameRequiredErrorMessage
            }
        },
        location: {
            required: {
                isRequired: true,
                error: props.locationRequiredErrorMessage
            }
        }
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
    };

    const {
        values, errors, dirty, disable,
        setFieldValue, handleOnChange, handleOnSubmit
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm, !props.id);

    const {id, name, location} = values;
    return (
        <section className="section section-small-padding inventory-add">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div className="column is-half inventory-add-content">
                <form onSubmit={handleOnSubmit} className="is-modern-form" method="post">
                    {id &&
                        <div className="field">
                            <InputLabel id="id-label">{props.idLabel} {id}</InputLabel>
                        </div>}
                    <div className="field">
                        <TextField 
                           id="name" 
                           name="name" 
                           type="text" 
                           label={props.nameLabel} 
                           fullWidth={true} 
                           value={name} 
                           onChange={handleOnChange}
                           helperText={dirty.name ? errors.name : ""} 
                           error={(errors.name.length > 0) && dirty.name}/>
                    </div>
                    <div className="field">
                        <TextField 
                           id="location" 
                           name="location" 
                           type="text" 
                           label={props.locationLabel} 
                           fullWidth={true} 
                           value={location} 
                           onChange={handleOnChange}
                           helperText={dirty.location ? errors.location : ""} 
                           error={(errors.location.length > 0) && dirty.location}/>
                    </div>
                    <div className="field">
                        <Button 
                            type="subbmit" 
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
                                NavigationHelper.redirect(props.warehouseUrl);
                            }}>
                            {props.navigateToWarehouseListText}
                        </Button> 
                    </div>
                </form>
            </div>
        </section>
    );
};

WarehouseForm.propTypes = {
    title: PropTypes.string.isRequired,
    name: PropTypes.string,
    location: PropTypes.string,
    saveUrl: PropTypes.string,
    saveText: PropTypes.string,
    id: PropTypes.string,
    generalErrorMessage: PropTypes.string,
    nameRequiredErrorMessage: PropTypes.string,
    locationRequiredErrorMessage: PropTypes.string,
    nameLabel: PropTypes.string,
    addText: PropTypes.string,
    navigateToWarehouseListText: PropTypes.string,
    warehouseUrl: PropTypes.string,
    locationLabel: PropTypes.string,
    idLabel: PropTypes.string
};

export default WarehouseForm;
