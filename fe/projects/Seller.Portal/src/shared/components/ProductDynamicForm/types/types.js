// Source: https://github.com/rjsf-team/react-jsonschema-form/blob/master/packages/core/src/types.js
import PropTypes from "prop-types";

export const registry = PropTypes.shape({
  ArrayFieldTemplate: PropTypes.elementType,
  FieldTemplate: PropTypes.elementType,
  ObjectFieldTemplate: PropTypes.elementType,
  definitions: PropTypes.object.isRequired,
  rootSchema: PropTypes.object,
  fields: PropTypes.objectOf(PropTypes.elementType).isRequired,
  formContext: PropTypes.object.isRequired,
  widgets: PropTypes.objectOf(
    PropTypes.oneOfType([PropTypes.func, PropTypes.object])
  ).isRequired,
});

export const fieldProps = {
  autofocus: PropTypes.bool,
  disabled: PropTypes.bool,
  errorSchema: PropTypes.object,
  formData: PropTypes.any,
  idSchema: PropTypes.object,
  onBlur: PropTypes.func,
  onChange: PropTypes.func,
  onFocus: PropTypes.func,
  rawErrors: PropTypes.arrayOf(PropTypes.string),
  readonly: PropTypes.bool,
  registry,
  required: PropTypes.bool,
  schema: PropTypes.object.isRequired,
  uiSchema: PropTypes.shape({
    "ui:options": PropTypes.shape({
      addable: PropTypes.bool,
      orderable: PropTypes.bool,
      removable: PropTypes.bool,
    }),
  }),
};