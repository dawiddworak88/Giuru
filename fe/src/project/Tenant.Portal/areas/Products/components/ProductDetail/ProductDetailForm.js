import React, { useContext, useState } from 'react';
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

    var tempSchemas = [
      { name: "Narożniki bez strony", value: "1" },
      { name: "Narożniki ze stroną", value: "2" },
      { name: "Sofy", value: "3" },
      { name: "Szafy", value: "4" }
    ];

    const [tempSchema, setTempSchema] = useState(null);

    function changeSchema(schema) {
        
        if (schema.value === "1") {

            var schema1 = {
                id: "1",
                jsonSchema: {
                  "type": "object",
                  "properties": {
                    "threadColor": {
                      "type": "string",
                      "title": "Kolor nici:"
                    },
                    "primaryFabrics": {
                      "type": "string",
                      "title": "Główna tkanina:"
                    }
                  }
                }
            }

            setTempSchema(schema1);
        }

        if (schema.value === "2") {
            var schema2 = {
                id: "2",
                jsonSchema: {
                  "type": "object",
                  "properties": {
                    "side": {
                      "type": "string",
                      "title": "Strona:"
                    },
                    "threadColor": {
                      "type": "string",
                      "title": "Kolor nici:"
                    }
                  }
                }
            }

            setTempSchema(schema2);
        }
    }

    return (
        <div>
            <form className="is-modern-form" onSubmit={handleOnSubmit} method="post">
                {props.schemas && 
                    <Autocomplete
                        id="select-schema"
                        options={tempSchemas}
                        onChange={(event, schema) => {
                            changeSchema(schema);
                          }}
                        getOptionLabel={(option) => option.name}
                        renderInput={(params) => <TextField {...params} label={props.selectSchemaLabel} variant="outlined" />} />
                }
                <div className="field">
                    <TextField id="sku" name="sku" label={props.skuLabel} fullWidth={true}
                        value={sku} onChange={handleOnChange} helperText={dirty.sku ? errors.sku : ''} error={(errors.sku.length > 0) && dirty.sku} />
                </div>
                <div className="field">
                    <TextField id="name" name="name" label={props.nameLabel} fullWidth={true}
                        value={name} onChange={handleOnChange} helperText={dirty.name ? errors.name : ''} error={(errors.name.length > 0) && dirty.name} />
                </div>
                {tempSchema &&
                    <DynamicForm schema={tempSchema} formData={formData} onChange={handleOnChange} />
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