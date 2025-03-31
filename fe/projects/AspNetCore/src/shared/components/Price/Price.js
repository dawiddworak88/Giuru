const Price = ({
    current,
    old,
    lowestPrice,
    currency,
    lowestPriceLabel
}) => (
    <div className="price">
        <div className="price__container">
            <h3 className="price__current">{current} {currency}</h3>
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