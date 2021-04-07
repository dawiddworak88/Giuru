import React from "react";
import Switch from "@material-ui/core/Switch";
import FormControlLabel from "@material-ui/core/FormControlLabel";
import NoSsr from '@material-ui/core/NoSsr';
import { schemaRequiresTrueValue } from "../utils/utils";

const CheckboxWidget = (props) => {

    const {
        schema,
        id,
        value,
        disabled,
        readonly,
        label,
        autofocus,
        onChange,
    } = props;

    // Because an unchecked checkbox will cause html5 validation to fail, only add
    // the "required" attribute if the field value must be "true", due to the
    // "const" or "enum" keywords
    const required = schemaRequiresTrueValue(schema);

    return (
        <div className="field">
            <NoSsr>
                <FormControlLabel
                    control={
                        <Switch
                            id={id}
                            name={id}
                            checked={(typeof value === "undefined" || value === "false") ? false : value}
                            required={required}
                            disabled={disabled || readonly}
                            autoFocus={autofocus}
                            onChange={(event, checked) => {
                                event.target.value = checked;
                                onChange(event);
                            }} />
                    }
                    label={label} />
            </NoSsr>
        </div>
    );
};

export default CheckboxWidget;
