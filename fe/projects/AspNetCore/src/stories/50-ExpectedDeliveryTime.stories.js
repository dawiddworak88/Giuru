import React from "react";
import { calculateFinalDeliveryDay } from "../shared/components/ExpectedDeliveryTime/ExpectedDeliveryTime";

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
  singleResult: {
    fontFamily: "'Nunito', sans-serif",
    padding: "2rem",
  },
};

const DeliveryRow = ({ days, locale, labels }) => (
  <div style={styles.row}>
    <span style={styles.label}>{days} business day{days > 1 ? "s" : ""}:</span>
    <span style={styles.result}>{calculateFinalDeliveryDay(days, locale, labels)}</span>
  </div>
);

// Built-in translations (no labels passed)
const BuiltInStory = () => (
  <div style={styles.container}>
    {locales.map((locale) => (
      <div key={locale} style={styles.section}>
        <div style={styles.localeTitle}>Locale: {locale} (built-in)</div>
        {businessDayScenarios.map((days) => (
          <DeliveryRow key={days} days={days} locale={locale} />
        ))}
      </div>
    ))}
  </div>
);

// SSR labels passed from backend
const SsrLabelsStory = () => (
  <div style={styles.container}>
    {locales.map((locale) => (
      <div key={locale} style={styles.section}>
        <div style={styles.localeTitle}>Locale: {locale} (SSR labels)</div>
        {businessDayScenarios.map((days) => (
          <DeliveryRow key={days} days={days} locale={locale} labels={ssrLabels[locale]} />
        ))}
      </div>
    ))}
  </div>
);

export const AllLocalesBuiltIn = () => <BuiltInStory />;
AllLocalesBuiltIn.story = { name: "All locales — built-in translations" };

export const AllLocalesSsrLabels = () => <SsrLabelsStory />;
AllLocalesSsrLabels.story = { name: "All locales — SSR labels from backend" };

export const Polish = () => (
  <div style={styles.singleResult}>
    <strong>Result:</strong> {calculateFinalDeliveryDay(3, "pl", ssrLabels.pl)}
  </div>
);
Polish.story = { name: "Polish (pl) — 3 business days" };

export const German = () => (
  <div style={styles.singleResult}>
    <strong>Result:</strong> {calculateFinalDeliveryDay(3, "de", ssrLabels.de)}
  </div>
);
German.story = { name: "German (de) — 3 business days" };

export const English = () => (
  <div style={styles.singleResult}>
    <strong>Result:</strong> {calculateFinalDeliveryDay(3, "en", ssrLabels.en)}
  </div>
);
English.story = { name: "English (en) — 3 business days" };

export const LongDelivery = () => (
  <div style={styles.singleResult}>
    <strong>Result:</strong> {calculateFinalDeliveryDay(14, "en", ssrLabels.en)}
  </div>
);
LongDelivery.story = { name: "Long delivery (> 7 days)" };

const ExpectedDeliveryTimeStories = {
  title: "Shared/ExpectedDeliveryTime",
  component: BuiltInStory,
};

export default ExpectedDeliveryTimeStories;
