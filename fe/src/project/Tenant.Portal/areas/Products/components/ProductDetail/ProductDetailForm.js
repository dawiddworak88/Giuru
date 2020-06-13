import React, { useContext } from 'react';
import PropTypes from 'prop-types';
import { Context } from '../../../../../../shared/stores/Store';
import useForm from '../../../../../../shared/helpers/forms/useForm';
import { TextField, Button, CircularProgress } from '@material-ui/core';
import DynamicForm from '../../../../../../shared/components/DynamicForm/DynamicForm';

function ProductDetailForm(props) {

    const [state] = useContext(Context);

    const jsonSchema = props.schema && props.schema.jsonSchema ? JSON.parse(props.schema.jsonSchema) : {};
    const uiSchema = props.schema && props.schema.uiSchema ? JSON.parse(props.schema.uiSchema) : {};

    const stateSchema = {

        id: { value: props.product ? props.product.id : null, error: '' },
        name: { value: props.product ? props.product.name : '', error: '' },
        sku: { value: props.product ? props.product.sku : '', error: '' },
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
        console.log(state);
    }

    const {
        values,
        errors,
        dirty,
        disable,
        handleOnChange,
        handleOnSubmit
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm);

    const { name, sku, formData } = values;

    return (
        <div>
            <form className="is-modern-form" onSubmit={handleOnSubmit} method="post">
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
    schema: PropTypes.object
};

export default ProductDetailForm;