import React from "react";
import PropTypes from "prop-types";
import SchemaField from "../DynamicForm/fields/SchemaField";
import { getDefaultRegistry } from "./utils/utils";

function DynamicForm(props) {

    function getRegistry() {
        // For BC, accept passed SchemaField and TitleField props and pass them to
        // the "fields" registry one.
        const { fields, widgets } = getDefaultRegistry();

        return {
            fields: { ...fields },
            widgets: { ...widgets },
            definitions: (props.jsonSchema && props.jsonSchema.definitions) ? props.jsonSchema.definitions : {},
            rootSchema: props.jsonSchema,
            formContext: props.formContext || {},
        };
    }
    
    return (
        <div>
            <SchemaField
                schema={props.jsonSchema}
                uiSchema={props.uiSchema}
                formData={props.formData}
                onChange={props.onChange}
                registry={getRegistry()} />
        </div>
    );
}

DynamicForm.propTypes = {
    jsonSchema: PropTypes.object.isRequired,
    uiSchema: PropTypes.object,
    formData: PropTypes.object
};

export default DynamicForm;
