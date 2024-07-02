import moment from "moment";

import {
  datesOfPermanentHolidays,
  movableHolidays as datesOfMovableHolidays,
} from "../../constants/HolidaysConstants";

const SHIPMENT_ON_THE_SAME_DAY_BY_HOUR = 14;
const MOVE_DATE_BY_1_DAY = 1;
const MOVE_DATE_BY_2_DAYS = 2;
const DAYJS_SATURDAY_INDEX = 6;
const DAYJS_SUNDAY_INDEX = 0;

const allHolidaysInYear = [
  ...datesOfMovableHolidays,
  ...datesOfPermanentHolidays,
];

const holidaysDuringDeliveryTimeCount = (startTime, endTime) => {
  return allHolidaysInYear
    .map((holiday) => {
      const momentHoliday = moment(holiday);
      const currentHoliday = momentHoliday.day();

      return currentHoliday === DAYJS_SATURDAY_INDEX ||
        currentHoliday === DAYJS_SUNDAY_INDEX
        ? false
        : momentHoliday.isBetween(startTime, endTime);
    })
    .filter((isHoliday) => isHoliday).length;
};

export const calculateFinalDeliveryDay = (deliveryBusinessDays, shortDeliveryText, longDeliveryText, locale) => {
  const now = moment();
  const currentWeekDay = now.day();
  const orderWasPlacedBeforeNoon =
    now.hour() < SHIPMENT_ON_THE_SAME_DAY_BY_HOUR &&
      currentWeekDay !== DAYJS_SATURDAY_INDEX &&
      currentWeekDay !== DAYJS_SUNDAY_INDEX
      ? -1
      : 0;

  const isWeekendDayIndex = (dayIndex) => {
    return dayIndex === DAYJS_SUNDAY_INDEX || dayIndex === DAYJS_SATURDAY_INDEX;
  };

  const calculateWeekendDaysInTimeRange = (startDate, endDate) => {
    let totalWeekendDaysInTimeRange = 0;
    const differenceBetweenDates = endDate.diff(startDate, "day");

    for (let i = 1; i <= differenceBetweenDates; i++) {
      const currentDayIndex = startDate.add(i, "day").day();
      if (isWeekendDayIndex(currentDayIndex)) {
        totalWeekendDaysInTimeRange++;
      }
    }

    return totalWeekendDaysInTimeRange;
  };

  const weekendDays = calculateWeekendDaysInTimeRange(
    now.add(orderWasPlacedBeforeNoon, "day"),
    now.add(deliveryBusinessDays + orderWasPlacedBeforeNoon, "day")
  );

  const calculateRawDeliveryDate = () => {
    const numberOfDeliveryDays = deliveryBusinessDays + weekendDays + orderWasPlacedBeforeNoon;
    const tentativeDeliveryDate = now.add(numberOfDeliveryDays, "day");

    return tentativeDeliveryDate;
  };

  const rawDeliveryDate = calculateRawDeliveryDate();

  const rawDeliveryDateMovedByHoliday = rawDeliveryDate.add(MOVE_DATE_BY_1_DAY, "day");

  const numberOfHolidayDaysInDeliveryTime = holidaysDuringDeliveryTimeCount(now, rawDeliveryDateMovedByHoliday);

  const deliveryDateIncludingHolidays = rawDeliveryDate.add(numberOfHolidayDaysInDeliveryTime, "day");

  const calculateFinalDeliveryDate = (deliveryDate) => {
    const dateExcludingHolidayFallingOnDeliveryDate =
      allHolidaysInYear.includes(deliveryDateIncludingHolidays.format("YYYY-MM-DD"))
        ? deliveryDate.add(MOVE_DATE_BY_1_DAY, "day")
        : deliveryDate;

    if (dateExcludingHolidayFallingOnDeliveryDate.day() === DAYJS_SATURDAY_INDEX) {
      return dateExcludingHolidayFallingOnDeliveryDate.add(MOVE_DATE_BY_2_DAYS, "day");
    }

    if (dateExcludingHolidayFallingOnDeliveryDate.day() === DAYJS_SUNDAY_INDEX) {
      return dateExcludingHolidayFallingOnDeliveryDate.add(MOVE_DATE_BY_1_DAY, "day");
    }

    if (allHolidaysInYear.includes(
      dateExcludingHolidayFallingOnDeliveryDate.format("YYYY-MM-DD")) ||
      dateExcludingHolidayFallingOnDeliveryDate.day() === DAYJS_SATURDAY_INDEX ||
      dateExcludingHolidayFallingOnDeliveryDate.day() === DAYJS_SUNDAY_INDEX) {
      calculateFinalDeliveryDate(
        dateExcludingHolidayFallingOnDeliveryDate.add(MOVE_DATE_BY_1_DAY, "day")
      );
    }
    else {
      return dateExcludingHolidayFallingOnDeliveryDate;
    }
    return dateExcludingHolidayFallingOnDeliveryDate;
  };

  const lengthOfDeliveryTime = calculateFinalDeliveryDate(deliveryDateIncludingHolidays)?.diff(now, "day");

  return generateDeliveryInfo(lengthOfDeliveryTime, shortDeliveryText, longDeliveryText, locale, deliveryDateIncludingHolidays, calculateFinalDeliveryDate);
};

const generateDeliveryInfo = (days, shortDeliveryText, longDeliveryText, locale, deliveryDateIncludingHolidays, calculateFinalDeliveryDate) => {
  let deliveryInfo;
  
  if (days <= 7) {
    if (locale === "pl") {
      deliveryInfo = `${shortDeliveryText} 
      ${calculateFinalDeliveryDate(deliveryDateIncludingHolidays)?.day() === 2 ? "we " : "w "} 
      ${calculateFinalDeliveryDate(deliveryDateIncludingHolidays)?.locale(locale).format("dddd D MMMM")}`
    }
    else {
      deliveryInfo = `${shortDeliveryText} ${calculateFinalDeliveryDate(deliveryDateIncludingHolidays)?.locale(locale).format("dddd D MMMM")}`;
    }
  }
  else {
    deliveryInfo = `${longDeliveryText} ${days}`;

    if (locale === "pl") {
      deliveryInfo += " dni";
    }
    else if (locale === "en") {
      deliveryInfo += " days";
    }
    else {
      deliveryInfo += " Tagen"
    }
  }

  return deliveryInfo;
}