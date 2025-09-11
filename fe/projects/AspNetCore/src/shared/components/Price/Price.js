import React from 'react';
import { InfoIcon } from '../../icons';

const currencyMapping = {
    PLN: "zł",
    EUR: "€",
};

const Price = ({
    className,
    current,
    old = 1200,
    lowestPrice = 900,
    currency,
    lowestPriceLabel = "Najniższa cena w ciągu ostatnich 30 dni:",
    taxLabel = "netto (bez VAT)",
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
            <div className='' onClick={onInfoClick}>
                <InfoIcon />
            </div>
        </div>
        {lowestPrice && lowestPriceLabel && (
            <span className="price__lowest">{lowestPriceLabel} {lowestPrice} {currency}</span>
        )}
    </div>
)

export default Price;