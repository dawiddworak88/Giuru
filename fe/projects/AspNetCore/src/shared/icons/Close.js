import React, { memo } from "react";

const Svg = (props) => (
    <svg 
        {...props}
        width="32" 
        height="33" 
        viewBox="0 0 32 33" 
        fill="none" 
        xmlns="http://www.w3.org/2000/svg"
    >
        <path fill-rule="evenodd" clip-rule="evenodd" d="M20.5368 10.7511C20.8715 10.4163 21.4142 10.4163 21.7489 10.7511C22.0837 11.0858 22.0837 11.6285 21.7489 11.9632L17.2122 16.5L21.7489 21.0368C22.0837 21.3715 22.0837 21.9142 21.7489 22.2489C21.4142 22.5837 20.8715 22.5837 20.5368 22.2489L16 17.7122L11.4632 22.2489C11.1285 22.5837 10.5858 22.5837 10.2511 22.2489C9.91632 21.9142 9.91632 21.3715 10.2511 21.0368L14.7878 16.5L10.2511 11.9632C9.91632 11.6285 9.91632 11.0858 10.2511 10.7511C10.5858 10.4163 11.1285 10.4163 11.4632 10.7511L16 15.2878L20.5368 10.7511Z" fill="#171717"/>
    </svg>
);

const SvgClose = memo((props) => (
    <Svg 
        {...props}
        style={{...(props.style || {})}}
        width="32"
        height="33" 
    />
));

SvgClose.displayName = "CloseIcon";

export default SvgClose;