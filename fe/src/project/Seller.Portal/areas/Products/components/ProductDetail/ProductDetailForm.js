import React, { useContext } from 'react';
import { toast } from 'react-toastify';
import PropTypes from 'prop-types';
import Autocomplete from '@material-ui/lab/Autocomplete';
import { Context } from '../../../../../../shared/stores/Store';
import useForm from '../../../../../../shared/helpers/forms/useForm';
import { TextField, Button, CircularProgress } from '@material-ui/core';
import DynamicForm from '../../../../../../shared/components/DynamicForm/DynamicForm';
import FetchErrorHandler from '../../../../../../shared/helpers/errorHandlers/FetchErrorHandler';

function ProductDetailForm(props) {

    const defaultProps = {
        options: props.categories,
        getOptionLabel: (option) => option.name
    };

    const [state, dispatch] = useContext(Context);

    const [category, setCategory] = useState(null);

    const jsonSchema = props.schema && props.schema.jsonSchema ? JSON.parse(props.schema.jsonSchema) : {};
    const uiSchema = props.schema && props.schema.uiSchema ? JSON.parse(props.schema.uiSchema) : {};

    const stateSchema = {

        id: { value: props.product ? props.product.id : null, error: '' },
        name: { value: props.product ? props.product.name : '', error: '' },
        sku: { value: props.product ? props.product.sku : '', error: '' },
        schemaId: { value: props.schema ? props.schema.id : null, error: '' },
        formData: props.product && props.product.formData ? JSON.parse(props.product.formData) : {}
    };

    const stateValidatorSchema = {

        name: {
            required: {
                isRequired: true,
                error: props.nameRequiredErrorMessage
            }
        },
        sku: {
            required: {
                isRequired: true,
                error: props.skuRequiredErrorMessage
            }
        }
    };

    function onSubmitForm(state) {

        dispatch({ type: 'SET_IS_LOADING', payload: true });

        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(state)
        };

        fetch(props.saveUrl, requestOptions)
            .then(function (response) {

                dispatch({ type: 'SET_IS_LOADING', payload: false });

                FetchErrorHandler.handleUnauthorizedResponse(response);

                return response.json().then(jsonResponse => {

                    if (response.ok) {

                        setFieldValue({ name: "id", value: jsonResponse.data.id });
                        toast.success(jsonResponse.message);
                    }
                    else {
                        FetchErrorHandler.consoleLogResponseDetails(state, response, jsonResponse);
                        toast.error(props.generalErrorMessage);
                    }
                })
            }).catch(error => {

                console.log(error);
                dispatch({ type: 'SET_IS_LOADING', payload: false });
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
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm);

    const { name, sku, schemaId, formData } = values;

    return (
        <div>
            <form className="is-modern-form" onSubmit={handleOnSubmit} method="post">
                <input type="hidden" value={schemaId} />
                <div className="field">
                    <Autocomplete
                        {...defaultProps}
                        id="category"
                        name="category"
                        fullWidth={true}
                        value={category}
                        onChange={(event, newValue) => {
                        }}
                        autoComplete
                        renderInput={(params) => <TextField {...params} label={props.selectCategoryLabel} margin="normal" />}
                    />
                </div>
                <div className="field">
                    <TextField id="sku" name="sku" label={props.skuLabel} fullWidth={true}
                        value={sku} onChange={handleOnChange} helperText={dirty.sku ? errors.sku : ''} error={(errors.sku.length > 0) && dirty.sku} />
                </div>
                <div className="field">
                    <TextField id="name" name="name" label={props.nameLabel} fullWidth={true}
                        value={name} onChange={handleOnChange} helperText={dirty.name ? errors.name : ''} error={(errors.name.length > 0) && dirty.name} />
                </div>
                {jsonSchema &&
                    <DynamicForm jsonSchema={jsonSchema} uiSchema={uiSchema} formData={formData} onChange={handleOnChange} />
                }
                <div className="field">
                    <Button type="submit" variant="contained" color="primary" disabled={state.isLoading || disable}>
                        {props.saveText}
                    </Button>
                </div>
            </form>
            {state.isLoading && <CircularProgress className="progressBar" />}
        </div>
    );
}

ProductDetailForm.propTypes = {
    skuLabel: PropTypes.string.isRequired,
    nameLabel: PropTypes.string.isRequired,
    saveText: PropTypes.string.isRequired,
    categories: PropTypes.array.isRequired,
    generalErrorMessage: PropTypes.string.isRequired
};

export default ProductDetailForm;