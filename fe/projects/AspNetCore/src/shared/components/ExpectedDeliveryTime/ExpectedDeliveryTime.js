import React from "react";
import PropTypes from "prop-types";
import moment from "moment";
import "./ExpectedDeliveryTime.scss";

import {
  permanentHolidays,
  movableHolidays,
} from "./HolidaysConstants";

const SHIPMENT_ON_THE_SAME_DAY_BY_HOUR = 14;
const SUNDAY = 0;
const SATURDAY = 6;

const isBusinessDay = (date) => {
  return (
    ![SUNDAY, SATURDAY].includes(date.day()) &&
    !permanentHolidays.includes(date.format("MM-DD")) &&
    !movableHolidays.includes(date.format("YYYY-MM-DD"))
  );
};

const getShipmentStartDate = (now) => {
  const shipmentDate = now.clone();

  if (now.hour() < SHIPMENT_ON_THE_SAME_DAY_BY_HOUR) {
    return shipmentDate.subtract(1, "day");
  }

  return shipmentDate;
};

export const calculateExpectedDeliveryDate = (deliveryBusinessDays, now = moment()) => {
  let currentDate = getShipmentStartDate(now);

  let addedBusinessDays = 0;

  while (addedBusinessDays < deliveryBusinessDays) {
    currentDate = currentDate.add(1, "day");

    if (isBusinessDay(currentDate)) {
      addedBusinessDays++;
    }
  }

  return currentDate;
};

const applyTemplate = (template, vars) =>
  template.replace(/\{(\w+)\}/g, (_, key) => (vars[key] !== undefined ? vars[key] : `{${key}}`));

const resolveDeliveryMessage = (deliveryMessage, deliveryBusinessDays, locale) => {
  if (!deliveryMessage) {
    return "";
  }

  if (!(deliveryBusinessDays > 0)) {
    if (moment(deliveryMessage, moment.ISO_8601, true).isValid()) {
      return moment(deliveryMessage).format("L");
    }

    return deliveryMessage;
  }

  const deliveryDate = calculateExpectedDeliveryDate(deliveryBusinessDays);
  const now = moment();
  const days = deliveryDate.diff(now, "day");
  const date = deliveryDate.locale(locale || "en").format("D.MM");

  return applyTemplate(deliveryMessage, { date, days });
};

const ExpectedDeliveryTime = ({ deliveryMessage, deliveryBusinessDays, locale }) => {
  const resolvedMessage = resolveDeliveryMessage(deliveryMessage, deliveryBusinessDays, locale);

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
  deliveryBusinessDays: PropTypes.number,
  locale: PropTypes.string,
};

export default ExpectedDeliveryTime;
