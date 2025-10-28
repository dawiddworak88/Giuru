import React, { memo } from "react";

const Svg = (props) => (
    <svg
        {...props}
        width="12"
        height="12"
        viewBox="0 0 10 10"
        fill="none"
        xmlns="http://www.w3.org/2000/svg"
    >
        <path
            fillRule="evenodd"
            clipRule="evenodd"
            d="M8.78064 0.209209C9.05958 -0.0697365 9.51184 -0.0697365 9.79079 0.209209C10.0697 0.488155 10.0697 0.940416 9.79079 1.21936L6.01015 5L9.79079 8.78064C10.0697 9.05958 10.0697 9.51184 9.79079 9.79079C9.51184 10.0697 9.05958 10.0697 8.78064 9.79079L5 6.01015L1.21936 9.79079C0.940416 10.0697 0.488155 10.0697 0.20921 9.79079C-0.0697366 9.51184 -0.0697366 9.05958 0.20921 8.78064L3.98985 5L0.20921 1.21936C-0.0697362 0.940416 -0.0697362 0.488155 0.20921 0.209209C0.488156 -0.0697365 0.940416 -0.0697365 1.21936 0.209209L5 3.98985L8.78064 0.209209Z"
            fill="white"
        />
    </svg>
);

const SvgRemove = memo((props) => (
    <Svg 
        {...props}
        style={{...(props.style || {})}}
        width="12"
        height="12" 
    />
));

SvgRemove.displayName = "RemoveIcon";

export default SvgRemove;