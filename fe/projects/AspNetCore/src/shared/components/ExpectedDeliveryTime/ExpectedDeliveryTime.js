import React from "react";
import PropTypes from "prop-types";
import moment from "moment";
import "./ExpectedDeliveryTime.scss";

const applyTemplate = (template, vars) =>
  template.replace(/\{(\w+)\}/g, (_, key) => (vars[key] !== undefined ? vars[key] : `{${key}}`));

const resolveDeliveryMessage = (deliveryMessage, expectedDeliveryDate, locale) => {
  if (!deliveryMessage) {
    return "";
  }

  if (!expectedDeliveryDate) {
    if (moment(deliveryMessage, moment.ISO_8601, true).isValid()) {
      return moment(deliveryMessage).format("L");
    }

    return deliveryMessage;
  }

  const deliveryDate = moment(expectedDeliveryDate);
  const now = moment();
  const days = deliveryDate.diff(now, "day");
  const date = deliveryDate.locale(locale || "en").format("D.MM");

  return applyTemplate(deliveryMessage, { date, days });
};

const ExpectedDeliveryTime = ({ deliveryMessage, expectedDeliveryDate, locale }) => {
  const resolvedMessage = resolveDeliveryMessage(deliveryMessage, expectedDeliveryDate, locale);

  if (!resolvedMessage) {
    return null;
  }

  return (
    <div className="expected-delivery-time">
      <div className="expected-delivery-time__dot-status">
        <div className="expected-delivery-time__dot" />
        <span className="expected-delivery-time__message">{resolvedMessage}</span>
      </div>
    </div>
  );
};

ExpectedDeliveryTime.propTypes = {
  deliveryMessage: PropTypes.string,
  expectedDeliveryDate: PropTypes.string,
  locale: PropTypes.string,
};

export default ExpectedDeliveryTime;
