import React from "react";
import PropTypes from "prop-types";
import { TextField } from "@mui/material";

function TextWidget(props) {

  return (
    <div className="field">
        <TextField 
          id={props.id} 
          name={props.id} 
          onChange={props.onChange} 
          label={props.label}
          variant="standard"
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
