import React from 'react';
import { InfoIcon } from '../../icons';

const Price = ({
    className,
    current,
    old,
    lowestPrice,
    currency,
    lowestPriceLabel,
    taxLabel,
    onInfoClick,
}) => (
    <div className={`price ${className || ''}`}>
        <div className="price__container">
            <span className="price__current">{parseFloat(current)} {currency}</span>
            {old && (
                <span className="price__old">{old} {currency}</span>
            )}
            {taxLabel && (
                <span className="price__tax-label">{taxLabel}</span>
            )}
            <div className='price__icon' onClick={onInfoClick}>
                <InfoIcon />
            </div>
        </div>
        {lowestPrice && lowestPriceLabel && (
            <span className="price__lowest">{lowestPriceLabel} {lowestPrice} {currency}</span>
        )}
    </div>
)

export default Price;