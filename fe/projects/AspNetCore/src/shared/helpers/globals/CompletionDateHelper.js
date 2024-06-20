import dayjs from "dayjs";
import isBetween from "dayjs/plugin/isBetween";
import updateLocale from "dayjs/plugin/updateLocale";

import {
    datesOfPermanentHolidays,
    movableHolidays as datesOfMovableHolidays,
  } from "../../constants/HolidaysConstants";

const SHIPMENT_ON_THE_SAME_DAY_BY_HOUR = 12;
const MOVE_DATE_BY_1_DAY = 1;
const MOVE_DATE_BY_2_DAYS = 2;
const DAYJS_SATURDAY_INDEX = 6;
const DAYJS_SUNDAY_INDEX = 0;

dayjs.extend(isBetween);
dayjs.extend(updateLocale);

const allHolidaysInYear = [
    ...datesOfMovableHolidays,
    ...datesOfPermanentHolidays,
  ];

  const holidaysDuringDeliveryTimeCount = (startTime, endTime) => {
    return allHolidaysInYear
      .map((holiday) => {
        const dayJsHoliday = dayjs(holiday);
        const currentHoliday = dayJsHoliday.day();
  
        return currentHoliday === DAYJS_SATURDAY_INDEX ||
          currentHoliday === DAYJS_SUNDAY_INDEX
          ? false
          : dayJsHoliday.isBetween(startTime, endTime);
      })
      .filter((isHoliday) => isHoliday).length;
  };

  export const calculateFinalDeliveryDay = (deliveryBusinessDays) => {
    const now = dayjs();
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
      const numberOfDeliveryDays =
        deliveryBusinessDays + weekendDays + orderWasPlacedBeforeNoon;
      const tentativeDeliveryDate = now.add(numberOfDeliveryDays, "day");
      return tentativeDeliveryDate;
    };
  
    const rawDeliveryDate = calculateRawDeliveryDate();
    const rawDeliveryDateMovedByHoliday = rawDeliveryDate.add(
      MOVE_DATE_BY_1_DAY,
      "day"
    );
    const numberOfHolidayDaysInDeliveryTime = holidaysDuringDeliveryTimeCount(
      now,
      rawDeliveryDateMovedByHoliday
    );
  
    const deliveryDateIncludingHolidays = rawDeliveryDate.add(
      numberOfHolidayDaysInDeliveryTime,
      "day"
    );
  
    const calculateFinalDeliveryDate = (deliveryDate) => {
      const dateExcludingHolidayFallingOnDeliveryDate =
        allHolidaysInYear.includes(
          deliveryDateIncludingHolidays.format("YYYY-MM-DD")
        )
          ? deliveryDate.add(MOVE_DATE_BY_1_DAY, "day")
          : deliveryDate;
  
      if (
        dateExcludingHolidayFallingOnDeliveryDate.day() === DAYJS_SATURDAY_INDEX
      ) {
        return dateExcludingHolidayFallingOnDeliveryDate.add(
          MOVE_DATE_BY_2_DAYS,
          "day"
        );
      }
  
      if (
        dateExcludingHolidayFallingOnDeliveryDate.day() === DAYJS_SUNDAY_INDEX
      ) {
        return dateExcludingHolidayFallingOnDeliveryDate.add(
          MOVE_DATE_BY_1_DAY,
          "day"
        );
      }
  
      if (
        allHolidaysInYear.includes(
          dateExcludingHolidayFallingOnDeliveryDate.format("YYYY-MM-DD")
        ) ||
        dateExcludingHolidayFallingOnDeliveryDate.day() ===
          DAYJS_SATURDAY_INDEX ||
        dateExcludingHolidayFallingOnDeliveryDate.day() === DAYJS_SUNDAY_INDEX
      ) {
        calculateFinalDeliveryDate(
          dateExcludingHolidayFallingOnDeliveryDate.add(MOVE_DATE_BY_1_DAY, "day")
        );
      } else {
        return dateExcludingHolidayFallingOnDeliveryDate;
      }
  
      return dateExcludingHolidayFallingOnDeliveryDate;
    };
  
    const lengthOfDeliveryTime = calculateFinalDeliveryDate(
      deliveryDateIncludingHolidays
    )?.diff(now, "day");
  
    if (lengthOfDeliveryTime <= 7) {
      const deliveryInfo = `Dostarczymy do Ciebie ${
        calculateFinalDeliveryDate(deliveryDateIncludingHolidays)?.day() === 2
          ? "we"
          : "w"
      }
          ${calculateFinalDeliveryDate(deliveryDateIncludingHolidays)
            ?.locale("pl")
            .format("dddd D MMMM")}`;
      return deliveryInfo;
    } else {
      const deliveryInfo = `Dostarczymy do Ciebie do ${lengthOfDeliveryTime} dni`;
      return deliveryInfo;
    }
  };
  
  dayjs.updateLocale("pl", {
    months: [
      "stycznia",
      "lutego",
      "marca",
      "kwietnia",
      "maja",
      "czerwca",
      "lipca",
      "sierpnia",
      "września",
      "października",
      "listopada",
      "grudnia",
    ],
    weekdays: [
      "niedzielę",
      "poniedziałek",
      "wtorek",
      "środę",
      "czwartek",
      "piątek",
      "sobotę",
    ]
  });