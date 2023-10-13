import React from "react";
import PropTypes from "prop-types";
import { TextField } from "@mui/material";

function UpDownWidget(props) {

  return (
    <div className="field">
        <TextField 
          type="number"
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

UpDownWidget.propTypes = {
  value: PropTypes.oneOfType(PropTypes.number),
  id: PropTypes.string,
};

export default UpDownWidget;