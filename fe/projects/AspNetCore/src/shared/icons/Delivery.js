import React, { memo } from "react";

const Svg = (props) => (
    <svg 
        {...props}
        width="21" 
        height="20" 
        viewBox="0 0 21 20" 
        fill="none" 
        xmlns="http://www.w3.org/2000/svg"
    >
        <g clipPath="url(#clip0_89_2076)">
            <path d="M1.33325 5V15L10.4999 19.1667L19.6666 15V5" stroke="#373F49" strokeWidth="1.2" strokeLinejoin="round"/>
            <path d="M1.33325 5.00016L10.4999 9.16683L19.6666 5.00016L10.4999 0.833496L1.33325 5.00016Z" stroke="#373F49" strokeLinejoin="round"/>
            <path d="M10.5 9.1665V19.1665" stroke="#373F49" strokeLinejoin="round"/>
            <path fillRule="evenodd" clipRule="evenodd" d="M5.57178 3.67548L14.2499 7.6201V11.4426L15.9166 10.6849V7.08351C15.9166 6.75672 15.7256 6.46009 15.4281 6.32487L6.26144 2.1582L5.57178 3.67548Z" fill="#373F49"/>
        </g>
        <defs>
            <clipPath id="clip0_89_2076">
                <rect width="20" height="20" fill="white" transform="translate(0.5)"/>
            </clipPath>
        </defs>
    </svg>
);

const SvgDelivery = memo((props) => (
    <Svg 
        {...props}
        style={{...(props.style || {})}}
        width="21"
        height="20" 
    />
));

SvgDelivery.displayName = "DeliveryIcon";

export default SvgDelivery;