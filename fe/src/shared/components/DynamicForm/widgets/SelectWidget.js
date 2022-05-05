import React from "react";

import { FormControl, MenuItem, Select, InputLabel } from "@mui/material"
import { asNumber, guessType } from "../utils/utils";
const nums = new Set(["number", "integer"]);

/**
 * This is a silly limitation in the DOM where option change event values are
 * always retrieved as strings.
 */
const processValue = (schema, value) => {

    // "enum" is a reserved word, so only "type" and "items" can be destructured
    const { type, items } = schema;
    if (value === "") {
        return undefined;
    } else if (type === "array" && items && nums.has(items.type)) {
        return value.map(asNumber);
    } else if (type === "boolean") {
        return value === "true";
    } else if (type === "number") {
        return asNumber(value);
    }

    // If type is undefined, but an enum is present, try and infer the type from
    // the enum values
    if (schema.enum) {
        if (schema.enum.every((x) => guessType(x) === "number")) {
            return asNumber(value);
        } else if (schema.enum.every((x) => guessType(x) === "boolean")) {
            return value === "true";
        }
    }

    return value;
};

const SelectWidget = ({
    schema,
    id,
    options,
    label,
    required,
    disabled,
    readonly,
    value,
    multiple,
    autofocus,
    onChange,
    onBlur,
    onFocus
}) => {

    const { enumOptions, enumDisabled } = options;

    const emptyValue = multiple ? [] : "";

    return (
        <div className="field">
            <FormControl
                fullWidth={true}
                required={required}>
                <InputLabel shrink={true} htmlFor={id}>
                    {label || schema.title}
                </InputLabel>
                <Select
                    id={id}
                    name={id}
                    multiple={typeof multiple === "undefined" ? false : multiple}
                    value={typeof value === "undefined" ? emptyValue : value}
                    required={required}
                    disabled={disabled || readonly}
                    autoFocus={autofocus}
                    onBlur={
                        onBlur &&
                        (event => {
                            onBlur(id, processValue(schema, event.target.value));
                        })
                    }
                    onFocus={
                        onFocus &&
                        (event => {
                            onFocus(id, processValue(schema, event.target.value));
                        })
                    }
                    onChange={event => {
                        event.target.value = processValue(schema, event.target.value);
                        onChange(event);
                    }}>
                     {!multiple && schema.default === undefined &&
                        <MenuItem value="">
                            &nbsp;
                        </MenuItem>
                    }
                    {(enumOptions).map(({ value, label }, i) => {
                        const disabled =
                            enumDisabled && (enumDisabled).indexOf(value) !== -1;
                        return (
                            <MenuItem key={i} value={value} disabled={disabled}>
                                {label}
                            </MenuItem>
                        );
                    })}
                </Select>
            </FormControl>
        </div>
    );
};

export default SelectWidget;