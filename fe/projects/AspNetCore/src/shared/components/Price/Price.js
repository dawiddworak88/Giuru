import React from 'react';

const Price = ({
    current,
    old,
    lowestPrice,
    currency,
    lowestPriceLabel
}) => (
    current && (
        <div className="price">
            <div className="price__container">
                <h3 className="price__current">{new Intl.NumberFormat(null, { style: 'currency', currency }).format(current)} {currency}</h3>
                {old && (
                    <span className="price__old">{old} {currency}</span>
                )}
            </div>
            {lowestPrice && lowestPriceLabel && (
                <span className="price__lowest">{lowestPriceLabel} {lowestPrice} {currency}</span>
            )}
        </div>
    )
)

export default Price;