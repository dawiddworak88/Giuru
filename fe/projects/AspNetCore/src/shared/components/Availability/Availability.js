import React from "react";
import { DeliveryIcon } from "../../icons";
import PropTypes from "prop-types";

const Availability = ({
    className,
    label,
    availableQuantity
}) => {
    return (
        availableQuantity > 0 && (
            <div className={`availability ${className || ""}`}>
                <div className="availability__delivery-icon">
                    <DeliveryIcon />
                </div>
                <div className="availability__label">
                    {label} {availableQuantity}
                </div>
            </div>
        )
    )
}

Availability.propTypes = {
    className: PropTypes.string,
    label: PropTypes.string.isRequired,
    availableQuantity: PropTypes.number.isRequired
}

export default Availability;