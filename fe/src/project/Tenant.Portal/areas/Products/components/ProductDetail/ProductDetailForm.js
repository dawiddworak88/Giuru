import React, { useContext } from 'react';
import PropTypes from 'prop-types';
import { Context } from '../../../../../../shared/stores/Store';
import useForm from '../../../../../../shared/helpers/forms/useForm';
import { TextField, Button, CircularProgress } from '@material-ui/core';
import Autocomplete from '@material-ui/lab/Autocomplete';
import DynamicForm from '../../../../../../shared/components/DynamicForm/DynamicForm';

function ProductDetailForm(props) {
    
    const [state] = useContext(Context);

    const stateSchema = {
        name: { value: '', error: '' },
        sku: { value: '', error: '' }
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
        handleOnChange,
        handleOnSubmit
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm);

    const { name, sku } = values;

    return (
        <div>
            <form className="is-modern-form" onSubmit={handleOnSubmit} method="post">
                {props.schemas && 
                    <Autocomplete
                        id="select-schema"
                        options={props.schemas}
                        getOptionLabel={(option) => option.name}
                        renderInput={() => {
                            return <TextField label={props.selectSchemaLabel} variant="outlined" />;
                    }} />
                }
                <div className="field">
                    <TextField id="sku" name="sku" label={props.skuLabel} fullWidth={true}
                        value={sku} onChange={handleOnChange} helperText={errors.sku && dirty.sku && errors.sku} error={dirty.sku} />
                </div>
                <div className="field">
                    <TextField id="name" name="name" label={props.nameLabel} fullWidth={true}
                        value={name} onChange={handleOnChange} helperText={errors.name && dirty.name && errors.name} error={dirty.name} />
                </div>
                <DynamicForm schema={props.schema} />
                <div className="field">
                    <Button type="submit" variant="contained" color="primary" disabled={state.isLoading}>
                        {props.saveText}
                    </Button>
                </div>
            </form>
            {state.isLoading && <CircularProgress className="progressBar" />}
        </div>
    );
}

ProductDetailForm.propTypes = {
    selectLabel: PropTypes.string
};

export default ProductDetailForm;