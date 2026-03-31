import React from "react";
import moment from "moment";

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