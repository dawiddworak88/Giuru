import React, { memo } from "react";

const Svg = (props) => (
    <svg
        {...props} 
        width="42" 
        height="44" 
        viewBox="0 0 86 88" 
        xmlns="http://www.w3.org/2000/svg"
    >
        <rect x="18" y="18" width="50" height="50" rx="6" fill="white" stroke="#B3B3B3" strokeWidth="2"/>
    </svg>
);

const SvgCheckbox = memo((props) => (
    <Svg 
        {...props}
        style={{...(props.style || {})}}
        width="42"
        height="44" 
    />
));

SvgCheckbox.displayName = "CheckboxIcon";

export default SvgCheckbox;