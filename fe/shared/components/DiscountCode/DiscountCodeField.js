import React from "react";
import PropTypes from "prop-types";
import { TextField, Button } from "@mui/material";

function DiscountCodeField(props) {

    return (
        <div className="field">
            <div className="columns is-vcentered">
                <div className="column is-4">
                    <TextField
                        id="discountCode"
                        name="discountCode"
                        type="text"
                        label={props.discountCodeLabel}
                        variant="standard"
                        fullWidth={true}
                        value={props.value}
                        disabled={props.disabled}
                        onChange={(e) => props.onChange(e.target.value)}
                    />
                </div>
                <div className="column is-narrow">
                    <Button
                        type="button"
                        variant="contained"
                        color="primary"
                        onClick={props.onApply}
                        disabled={props.disabled || !props.canApply || !props.value.trim()}
                    >
                        {props.applyDiscountCodeLabel}
                    </Button>
                </div>
                {props.appliedCode &&
                    <div className="column is-narrow">
                        <Button
                            type="button"
                            variant="text"
                            color="error"
                            onClick={props.onRemove}
                            disabled={props.disabled}
                        >
                            {props.removeDiscountCodeLabel}
                        </Button>
                    </div>
                }
            </div>
        </div>
    );
}

DiscountCodeField.propTypes = {
    value: PropTypes.string.isRequired,
    onChange: PropTypes.func.isRequired,
    onApply: PropTypes.func.isRequired,
    onRemove: PropTypes.func.isRequired,
    appliedCode: PropTypes.string,
    disabled: PropTypes.bool,
    canApply: PropTypes.bool,
    discountCodeLabel: PropTypes.string.isRequired,
    applyDiscountCodeLabel: PropTypes.string.isRequired,
    removeDiscountCodeLabel: PropTypes.string.isRequired
};

export default DiscountCodeField;
