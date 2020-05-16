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
        sku: { value: '', error: '' },
        formData: {}
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
                        value={sku} onChange={handleOnChange} helperText={dirty.sku ? errors.sku : ''} error={(errors.sku.length > 0) && dirty.sku} />
                </div>
                <div className="field">
                    <TextField id="name" name="name" label={props.nameLabel} fullWidth={true}
                        value={name} onChange={handleOnChange} helperText={dirty.name ? errors.name : ''} error={(errors.name.length > 0) && dirty.name} />
                </div>
                {props.schema &&
                    <DynamicForm schema={props.schema} formData={formData} onChange={handleOnChange} />
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
    selectLabel: PropTypes.string,
    schema: PropTypes.object.isRequired
};

export default ProductDetailForm;