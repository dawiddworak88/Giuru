import React from 'react';
import PropTypes from 'prop-types';
import SchemaField from '../DynamicForm/fields/SchemaField';
import { getDefaultRegistry } from "./utils/utils";

function DynamicForm(props) {

    function getRegistry() {
        // For BC, accept passed SchemaField and TitleField props and pass them to
        // the "fields" registry one.
        const { fields, widgets } = getDefaultRegistry();

        return {
            fields: { ...fields },
            widgets: { ...widgets },
            definitions: props.schema.jsonSchema.definitions || {},
            rootSchema: props.schema.jsonSchema,
            formContext: props.formContext || {},
        };
    }

    return (
        <div>
            {props.schema.jsonSchema &&
                <SchemaField
                schema={props.schema.jsonSchema}
                uiSchema={props.schema.uiSchema}
                formData={props.formData}
                onChange={props.onChange}
                registry={getRegistry()} />
            }
        </div>
    );
}

DynamicForm.propTypes = {
    schema: PropTypes.object.isRequired,
    formData: PropTypes.object
}

export default DynamicForm;