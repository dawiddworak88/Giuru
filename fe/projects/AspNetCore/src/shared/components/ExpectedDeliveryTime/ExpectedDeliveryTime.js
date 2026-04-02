import React from "react";
import PropTypes from "prop-types";
import "./ExpectedDeliveryTime.scss";

const ExpectedDeliveryTime = ({ deliveryMessage }) => {
  if (!deliveryMessage) {
    return null;
  }

  return (
    <div className="expected-delivery-time">
      <div className="expected-delivery-time__dot-status">
        <div className="expected-delivery-time__dot" />
        <span className="expected-delivery-time__message">{deliveryMessage}</span>
      </div>
    </div>
  );
};

ExpectedDeliveryTime.propTypes = {
  deliveryMessage: PropTypes.string,
};

export default ExpectedDeliveryTime;
