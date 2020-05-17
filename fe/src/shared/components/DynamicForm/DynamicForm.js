import React from 'react';
import PropTypes from 'prop-types';
import SchemaField from '../DynamicForm/fields/SchemaField';

function DynamicForm(props) {

    return (
        <div>
            <SchemaField 
              schema={props.schema.jsonSchema} 
              uiSchema={props.schema.uiSchema} 
              formData={props.formData}
              onChange={props.onChange} />
        </div>
    );
}

DynamicForm.propTypes = {
    schema: PropTypes.object.isRequired,
    formData: PropTypes.object
}

export default DynamicForm;