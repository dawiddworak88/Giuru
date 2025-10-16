import React, { memo } from "react";

const Svg = (props) => (
    <svg 
        {...props}
        width="42" 
        height="44" 
        viewBox="0 0 86 88" 
        xmlns="http://www.w3.org/2000/svg"
    >
        <rect x="18" y="18" width="50" height="50" rx="4" fill="#064254"/>
        <path d="M32 42L40 50L54 34" stroke="white" strokeWidth="4" fill="none" strokeLinecap="round" strokeLinejoin="round"/>
    </svg>
);

const SvgCheckboxChecked = memo((props) => (
    <Svg 
        {...props}
        style={{...(props.style || {})}}
        width="42"
        height="44" 
    />
));

SvgCheckboxChecked.displayName = "CheckboxCheckedIcon";

export default SvgCheckboxChecked;