const Price = ({
    current,
    old,
    lowestPrice,
    currency,
    lowPriceLabel
}) => (
    <div className="price">
        <div className="price__container">
            <h3 className="price__current">{current} {currency}</h3>
            {old && (
                <span className="price__old">{old} {currency}</span>
            )}
        </div>
        {lowestPrice && lowPriceLabel && (
            <span className="price__lowest">{lowPriceLabel} {lowestPrice} {currency}</span>
        )}
    </div>
)

export default Price;