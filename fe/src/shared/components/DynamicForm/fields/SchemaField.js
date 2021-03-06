// Source: https://github.com/rjsf-team/react-jsonschema-form/blob/master/packages/core/src/components/fields/SchemaField.js
import * as types from "../types/types";
import React from "react";
import PropTypes from "prop-types";

import {
  isMultiSelect,
  isSelect,
  retrieveSchema,
  toIdSchema,
  getDefaultRegistry,
  mergeObjects,
  getUiOptions,
  isFilesArray,
  deepEquals,
  getSchemaType,
} from "../utils/utils";

const COMPONENT_TYPES = {
  array: "ArrayField",
  boolean: "BooleanField",
  integer: "NumberField",
  number: "NumberField",
  object: "ObjectField",
  string: "StringField",
  null: "NullField",
};

function getFieldComponent(schema, uiSchema, idSchema, fields) {

  const field = uiSchema["ui:field"];
  if (typeof field === "function") {
    return field;
  }
  if (typeof field === "string" && field in fields) {
    return fields[field];
  }

  const componentName = COMPONENT_TYPES[getSchemaType(schema)];

  // If the type is not defined and the schema uses "anyOf" or "oneOf", don"t
  // render a field and let the MultiSchemaField component handle the form display
  if (!componentName && (schema.anyOf || schema.oneOf)) {
    return () => null;
  }

  return componentName in fields
    ? fields[componentName]
    : () => {
        const { UnsupportedField } = fields;

        return (
          <UnsupportedField
            schema={schema}
            idSchema={idSchema}
            reason={`Unknown field type ${schema.type}`}
          />
        );
      };
}

function Help(props) {
  const { help } = props;
  if (!help) {
    return null;
  }
  if (typeof help === "string") {
    return <p className="help-block">{help}</p>;
  }
  return <div className="help-block">{help}</div>;
}

function ErrorList(props) {
  const { errors = [] } = props;
  if (errors.length === 0) {
    return null;
  }

  return (
    <div>
      <ul className="error-detail bs-callout bs-callout-info">
        {errors
          .filter(elem => !!elem)
          .map((error, index) => {
            return (
              <li className="text-danger" key={index}>
                {error}
              </li>
            );
          })}
      </ul>
    </div>
  );
}

function DefaultTemplate(props) {
  const {
    children,
    errors,
    help,
    hidden
  } = props;
  if (hidden) {
    return <div className="hidden">{children}</div>;
  }

  return (
    <WrapIfAdditional {...props}>
      {children}
      {errors}
      {help}
    </WrapIfAdditional>
  );
}

DefaultTemplate.defaultProps = {
  hidden: false,
  readonly: false,
  required: false,
  displayLabel: true,
};

function WrapIfAdditional(props) {
  const {
    classNames
  } = props;

  return (
    <div className={classNames}>
      {props.children}
    </div>
  );
}

function SchemaFieldRender(props) {
  
  const {
    uiSchema,
    formData,
    errorSchema,
    idPrefix,
    name,
    onKeyChange,
    onDropPropertyClick,
    required,
    registry = getDefaultRegistry(),
    wasPropertyKeyModified = false,
  } = props;
  const { rootSchema, fields, formContext } = registry;
  const FieldTemplate =
    uiSchema["ui:FieldTemplate"] || registry.FieldTemplate || DefaultTemplate;
  let idSchema = props.idSchema;
  const schema = retrieveSchema(props.schema, rootSchema, formData);
  idSchema = mergeObjects(
    toIdSchema(schema, null, rootSchema, formData, idPrefix),
    idSchema
  );
  const FieldComponent = getFieldComponent(schema, uiSchema, idSchema, fields);
  const { DescriptionField } = fields;
  const disabled = Boolean(props.disabled || uiSchema["ui:disabled"]);
  const readonly = Boolean(
    props.readonly ||
      uiSchema["ui:readonly"] ||
      props.schema.readOnly ||
      schema.readOnly
  );
  const autofocus = Boolean(props.autofocus || uiSchema["ui:autofocus"]);
  if (Object.keys(schema).length === 0) {
    return null;
  }

  const uiOptions = getUiOptions(uiSchema);
  let { label: displayLabel = true } = uiOptions;
  if (schema.type === "array") {
    displayLabel =
      isMultiSelect(schema, rootSchema) ||
      isFilesArray(schema, uiSchema, rootSchema);
  }
  if (schema.type === "object") {
    displayLabel = false;
  }
  if (schema.type === "boolean" && !uiSchema["ui:widget"]) {
    displayLabel = false;
  }
  if (uiSchema["ui:field"]) {
    displayLabel = false;
  }

  const { __errors, ...fieldErrorSchema } = errorSchema;

  // See #439: uiSchema: Don"t pass consumed class names to child components
  const field = (
    <FieldComponent
      {...props}
      idSchema={idSchema}
      schema={schema}
      uiSchema={{ ...uiSchema, classNames: undefined }}
      disabled={disabled}
      readonly={readonly}
      autofocus={autofocus}
      errorSchema={fieldErrorSchema}
      formContext={formContext}
      rawErrors={__errors}
    />
  );

  const id = idSchema.$id;

  // If this schema has a title defined, but the user has set a new key/label, retain their input.
  let label;
  if (wasPropertyKeyModified) {
    label = name;
  } else {
    label = uiSchema["ui:title"] || props.schema.title || schema.title || name;
  }

  const description =
    uiSchema["ui:description"] ||
    props.schema.description ||
    schema.description;
  const errors = __errors;
  const help = uiSchema["ui:help"];
  const hidden = uiSchema["ui:widget"] === "hidden";
  const classNames = [
    errors && errors.length > 0 ? "field-error has-error has-danger" : "",
    uiSchema.classNames,
  ]
    .join(" ")
    .trim();

  const fieldProps = {
    description: (
      <DescriptionField
        id={id + "__description"}
        description={description}
        formContext={formContext}
      />
    ),
    rawDescription: description,
    help: <Help help={help} />,
    rawHelp: typeof help === "string" ? help : undefined,
    errors: <ErrorList errors={errors} />,
    rawErrors: errors,
    id,
    label,
    hidden,
    onKeyChange,
    onDropPropertyClick,
    required,
    disabled,
    readonly,
    displayLabel,
    classNames,
    formContext,
    fields,
    schema,
    uiSchema,
  };

  const AnyOfField = registry.fields.AnyOfField;
  const OneOfField = registry.fields.OneOfField;

  return (
    <FieldTemplate {...fieldProps}>
      <React.Fragment>
        {field}

        {/*
        If the schema `anyOf` or "oneOf" can be rendered as a select control, don"t
        render the selection and let `StringField` component handle
        rendering
      */}
        {schema.anyOf && !isSelect(schema) && (
          <AnyOfField
            disabled={disabled}
            errorSchema={errorSchema}
            formData={formData}
            idPrefix={idPrefix}
            idSchema={idSchema}
            onBlur={props.onBlur}
            onChange={props.onChange}
            onFocus={props.onFocus}
            options={schema.anyOf}
            baseType={schema.type}
            registry={registry}
            schema={schema}
            uiSchema={uiSchema}
          />
        )}

        {schema.oneOf && !isSelect(schema) && (
          <OneOfField
            disabled={disabled}
            errorSchema={errorSchema}
            formData={formData}
            idPrefix={idPrefix}
            idSchema={idSchema}
            onBlur={props.onBlur}
            onChange={props.onChange}
            onFocus={props.onFocus}
            options={schema.oneOf}
            baseType={schema.type}
            registry={registry}
            schema={schema}
            uiSchema={uiSchema}
          />
        )}
      </React.Fragment>
    </FieldTemplate>
  );
}

class SchemaField extends React.Component {
  shouldComponentUpdate(nextProps, nextState) {
    return !deepEquals(this.props, nextProps);
  }

  render() {
    return SchemaFieldRender(this.props);
  }
}

SchemaField.defaultProps = {
  uiSchema: {},
  errorSchema: {},
  idSchema: {},
  disabled: false,
  readonly: false,
  autofocus: false,
};

SchemaField.propTypes = {
  schema: PropTypes.object.isRequired,
  uiSchema: PropTypes.object,
  idSchema: PropTypes.object,
  formData: PropTypes.any,
  errorSchema: PropTypes.object,
  registry: types.registry
};

export default SchemaField;