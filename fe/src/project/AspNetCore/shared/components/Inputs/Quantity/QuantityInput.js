import React from "react";
import PropTypes from "prop-types";
import {
    ArrowDropUp, ArrowDropDown
} from "@material-ui/icons"

const QuantityInput = (props) => {
    const quantity = props.value;
    const maxValue = props.maxValue;
    const minValue = props.minValue;
    const stepValue = props.stepValue;
    const setValue = props.setValue;

    const handleInputUp = () => {
        if (quantity >= maxValue){
            setValue(maxValue)
        } else{
            setValue(quantity + stepValue)
        }
    }

    const handleInputDown = () => {
        if (quantity <= minValue){
            setValue(minValue);
        } else {
            setValue(quantity - stepValue)
        }
    }
    
    return (
        <div className="quantity-input">
            <input id="input" type="number" value={quantity} disabled/>
            <div className="quantity-input__buttons">
                <button className="input-up" onClick={handleInputUp}><ArrowDropUp/></button>
                <button className="input-down" onClick={handleInputDown}><ArrowDropDown/></button>
            </div>
        </div>
    )
}

QuantityInput.propTypes = {
    value: PropTypes.number.isRequired,
    maxValue: PropTypes.number.isRequired,
    minValue: PropTypes.number.isRequired,
    stepValue: PropTypes.number.isRequired,
    setValue: PropTypes.func.isRequired
}

export default QuantityInput;