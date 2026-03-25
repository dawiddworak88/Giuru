import React from "react";
import ExpectedDeliveryTime from "../shared/components/ExpectedDeliveryTime/ExpectedDeliveryTime";
import "../shared/layouts/index.scss";
import "../shared/components/ExpectedDeliveryTime/ExpectedDeliveryTime.scss";

const locales = ["pl", "de", "en"];
const businessDayScenarios = [1, 2, 3, 5, 7, 10, 14];

// Simulated SSR labels as would come from the backend
const ssrLabels = {
  pl: {
    withinWeekLabel: "Dostarczymy do Ciebie w {dayName} {date}",
    withinWeekWednesdayLabel: "Dostarczymy do Ciebie we {dayName} {date}",
    moreThanWeekLabel: "Dostarczymy do Ciebie do {days} dni",
    weekdaysAccusative: [
      "niedzielę",
      "poniedziałek",
      "wtorek",
      "środę",
      "czwartek",
      "piątek",
      "sobotę",
    ],
  },
  de: {
    withinWeekLabel: "Wir liefern am {dayName}, {date}",
    moreThanWeekLabel: "Wir liefern innerhalb von {days} Tagen",
  },
  en: {
    withinWeekLabel: "We will deliver on {dayName}, {date}",
    moreThanWeekLabel: "We will deliver within {days} days",
  },
};

const styles = {
  container: {
    fontFamily: "'Nunito', sans-serif",
    padding: "2rem",
    maxWidth: "800px",
  },
  section: {
    marginBottom: "2rem",
  },
  localeTitle: {
    fontSize: "1.2rem",
    fontWeight: "bold",
    borderBottom: "2px solid #1B5A6E",
    paddingBottom: "0.5rem",
    marginBottom: "1rem",
    color: "#1B5A6E",
    textTransform: "uppercase",
  },
  row: {
    display: "flex",
    justifyContent: "space-between",
    alignItems: "center",
    padding: "0.5rem 0.75rem",
    borderRadius: "4px",
    marginBottom: "0.25rem",
    backgroundColor: "#f5f5f5",
  },
  label: {
    fontWeight: "600",
    color: "#555",
    minWidth: "180px",
  },
  result: {
    color: "#171717",
  },
};

const DeliveryRow = ({ days, locale, labels }) => (
  <div style={styles.row}>
    <span style={styles.label}>{days} business day{days > 1 ? "s" : ""}:</span>
    <span style={styles.result}>
      <ExpectedDeliveryTime deliveryBusinessDays={days} locale={locale} labels={labels} />
    </span>
  </div>
);

const DeliveryStory = () => (
  <div style={styles.container}>
    {locales.map((locale) => (
      <div key={locale} style={styles.section}>
        <div style={styles.localeTitle}>Locale: {locale}</div>
        {businessDayScenarios.map((days) => (
          <DeliveryRow key={days} days={days} locale={locale} labels={ssrLabels[locale]} />
        ))}
      </div>
    ))}
  </div>
);

export const AllLocales = () => <DeliveryStory />;
AllLocales.story = { name: "All locales" };

export const Polish = () => (
  <ExpectedDeliveryTime deliveryBusinessDays={3} locale="pl" labels={ssrLabels.pl} />
);
Polish.story = { name: "Polish (pl) — 3 business days" };

export const German = () => (
  <ExpectedDeliveryTime deliveryBusinessDays={3} locale="de" labels={ssrLabels.de} />
);
German.story = { name: "German (de) — 3 business days" };

export const English = () => (
  <ExpectedDeliveryTime deliveryBusinessDays={3} locale="en" labels={ssrLabels.en} />
);
English.story = { name: "English (en) — 3 business days" };

export const LongDelivery = () => (
  <ExpectedDeliveryTime deliveryBusinessDays={14} locale="en" labels={ssrLabels.en} />
);
LongDelivery.story = { name: "Long delivery (> 7 days)" };

const ExpectedDeliveryTimeStories = {
  title: "Shared/ExpectedDeliveryTime",
  component: ExpectedDeliveryTime,
};

export default ExpectedDeliveryTimeStories;
