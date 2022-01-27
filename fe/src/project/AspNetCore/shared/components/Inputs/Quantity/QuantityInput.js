import React from "react";
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

export default QuantityInput;