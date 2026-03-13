import moment from "moment";
import "moment/locale/pl";
import "moment/locale/de";

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

/**
 * @param {number} deliveryBusinessDays
 * @param {string} [locale="en"]
 * @param {object} labels - Translations passed from SSR/backend.
 * @param {string} labels.withinWeekLabel          - e.g. "We will deliver on {dayName}, {date}"
 * @param {string} [labels.withinWeekWednesdayLabel] - Optional separate template for Wednesday (Polish grammar).
 *                                                     Falls back to withinWeekLabel when not provided.
 * @param {string} labels.moreThanWeekLabel        - e.g. "We will deliver within {days} days"
 * @param {string[]} [labels.weekdaysAccusative]   - Optional array of day names in accusative case (index 0=Sun).
 */
export const calculateFinalDeliveryDay = (deliveryBusinessDays, locale = "en", labels = {}) => {
  const now = moment();
  let currentDate = now.clone();

  if (now.hour() < SHIPMENT_ON_THE_SAME_DAY_BY_HOUR) {
    currentDate = currentDate.subtract(1, "day");
  }

  let addedBusinessDays = 0;

  while (addedBusinessDays < deliveryBusinessDays) {
    currentDate = currentDate.add(1, "day");

    if (isBusinessDay(currentDate)) {
      addedBusinessDays++;
    }
  }

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
