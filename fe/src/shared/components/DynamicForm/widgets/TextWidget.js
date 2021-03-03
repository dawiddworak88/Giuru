import React from "react";
import PropTypes from "prop-types";
import { TextField } from "@material-ui/core";

function TextWidget(props) {

  return (
    <div className="field">
        <TextField 
          id={props.id} 
          name={props.id} 
          onChange={props.onChange} 
          label={props.label}
          value={props.value ? props.value : ""}
          fullWidth={true} />
    </div>
  );
}

TextWidget.propTypes = {
  value: PropTypes.oneOfType([PropTypes.string, PropTypes.number]),
  id: PropTypes.string,
};

export default TextWidget;
