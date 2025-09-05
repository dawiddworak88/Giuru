import React from 'react';

const currencyMapping = {
    PLN: "zł",
    EUR: "€",
};

const Price = ({
    className,
    current,
    old,
    lowestPrice,
    currency,
    lowestPriceLabel
}) => (
    <div className={`price ${className || ''}`}>
        <div className="price__container">
            <h3 className="price__current">{parseFloat(current).toFixed(2)} {currencyMapping[currency]}</h3>
            {old && (
                <span className="price__old">{old} {currency}</span>
            )}
        </div>
        {lowestPrice && lowestPriceLabel && (
            <span className="price__lowest">{lowestPriceLabel} {lowestPrice} {currency}</span>
        )}
    </div>
)

export default Price;