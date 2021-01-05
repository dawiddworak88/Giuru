import React, { useContext } from "react";
import { toast } from "react-toastify";
import PropTypes from "prop-types";
import Autocomplete from "@material-ui/lab/Autocomplete";
import { Context } from "../../../../../../shared/stores/Store";
import { TextField, Button, FormControl, InputLabel, Select, FormHelperText, CircularProgress } from "@material-ui/core";
import useForm from "../../../../../../shared/helpers/forms/useForm";

function OrderForm(props) {

    const clientsProps = {
        options: props.clients,
        getOptionLabel: (option) => option.name
    };

    const [state, dispatch] = useContext(Context);

    const stateSchema = {

        id: { value: props.id ? props.id : null, error: "" },
        client: { value: props.clientId ? props.clients.find((item) => item.id === props.clientId) : null, error: "" }
    };

    const stateValidatorSchema = {

        client: {
            required: {
                isRequired: true,
                error: props.clientRequiredErrorMessage
            }
        },
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
    };

    const {
        values,
        errors,
        dirty,
        disable,
        setFieldValue,
        handleOnChange,
        handleOnSubmit
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm, !props.id);

    const { id, client } = values;

    return (
        <section className="section section-small-padding client">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div className="columns is-desktop">
                <div className="column is-half">
                    <form className="is-modern-form" onSubmit={handleOnSubmit} method="post">
                        {id &&
                            <input id="id" name="id" type="hidden" value={id} />
                        }
                        <div className="field">
                            <Autocomplete
                                {...clientsProps}
                                id="client"
                                name="client"
                                fullWidth={true}
                                value={client}
                                onChange={(event, newValue) => {
                                    setFieldValue({ name: "client", value: newValue });
                                  }}
                                autoComplete
                                renderInput={(params) => <TextField {...params} label={props.selectClientLabel} margin="normal" />}
                            />
                        </div>
                        {client &&
                            <div>
                            </div>
                        }
                        <div className="field">
                            <Button type="submit" variant="contained" color="primary" disabled={state.isLoading || disable}>
                                {props.saveText}
                            </Button>
                        </div>
                    </form>
                    {state.isLoading && <CircularProgress className="progressBar" />}
                </div>
            </div>
        </section>
    );
}

OrderForm.propTypes = {
    title: PropTypes.string.isRequired,
    id: PropTypes.string,
    selectClientLabel: PropTypes.string.isRequired,
    clientRequiredErrorMessage: PropTypes.string.isRequired,
    generalErrorMessage: PropTypes.string.isRequired,
    saveText: PropTypes.string.isRequired,
    saveUrl: PropTypes.string.isRequired,
    clients: PropTypes.array
};

export default OrderForm;
