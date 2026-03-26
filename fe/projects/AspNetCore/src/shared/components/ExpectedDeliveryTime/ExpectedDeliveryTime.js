import React from "react";
import PropTypes from "prop-types";
import moment from "moment";
import "moment/locale/pl";
import "moment/locale/de";
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

const applyTemplate = (template, vars) =>
  template.replace(/\{(\w+)\}/g, (_, key) => (vars[key] !== undefined ? vars[key] : `{${key}}`));

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

const calculateDeliveryMessage = (deliveryBusinessDays, locale, labels) => {
  const now = moment();
  const currentDate = calculateExpectedDeliveryDate(deliveryBusinessDays, now);

  const deliveryLengthInDays = currentDate.diff(now, "day");
  const dayIndex = currentDate.day();
  const formattedDate = currentDate.locale(locale).format("D MMMM");
  const dayName = labels.weekdaysAccusative
    ? labels.weekdaysAccusative[dayIndex]
    : currentDate.locale(locale).format("dddd");

  if (deliveryLengthInDays <= 7) {
    if (!labels.withinWeekLabel) {
      return null;
    }

    const template =
      dayIndex === 2 && labels.withinWeekWednesdayLabel
        ? labels.withinWeekWednesdayLabel
        : labels.withinWeekLabel;

    return applyTemplate(template, { dayName, date: formattedDate });
  }

  if (!labels.moreThanWeekLabel) {
    return null;
  }

  return applyTemplate(labels.moreThanWeekLabel, { days: deliveryLengthInDays });
};

const ExpectedDeliveryTime = (props) => {
  const message = calculateDeliveryMessage(
    props.deliveryBusinessDays,
    props.locale,
    props.labels || {}
  );

  if (!message) {
    return null;
  }

  return (
    <div className="expected-delivery-time">
      <div className="expected-delivery-time__dot-status">
        <div className="expected-delivery-time__dot" />
        <span className="expected-delivery-time__message">{message}</span>
      </div>
    </div>
  );
};

ExpectedDeliveryTime.propTypes = {
  deliveryBusinessDays: PropTypes.number.isRequired,
  locale: PropTypes.string,
  labels: PropTypes.shape({
    withinWeekLabel: PropTypes.string,
    withinWeekWednesdayLabel: PropTypes.string,
    moreThanWeekLabel: PropTypes.string,
    weekdaysAccusative: PropTypes.arrayOf(PropTypes.string),
  }),
};

ExpectedDeliveryTime.defaultProps = {
  locale: "en",
  labels: {},
};

export default ExpectedDeliveryTime;
