import React from "react";
import moment from "moment";
import ExpectedDeliveryTime from "../shared/components/ExpectedDeliveryTime/ExpectedDeliveryTime";
import "../shared/layouts/index.scss";
import "../shared/components/ExpectedDeliveryTime/ExpectedDeliveryTime.scss";

const futureDate = (days) => moment().add(days, "days").toISOString();

const styles = {
  container: {
    fontFamily: "'Nunito', sans-serif",
    padding: "2rem",
    maxWidth: "800px",
  },
  section: {
    marginBottom: "2rem",
  },
  sectionTitle: {
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

const DeliveryRow = ({ label, deliveryMessage, expectedDeliveryDate }) => (
  <div style={styles.row}>
    <span style={styles.label}>{label}</span>
    <span style={styles.result}>
      <ExpectedDeliveryTime deliveryMessage={deliveryMessage} expectedDeliveryDate={expectedDeliveryDate} locale="pl" />
    </span>
  </div>
);

const DeliveryStory = () => (
  <div style={styles.container}>
    <div style={styles.section}>
      <div style={styles.sectionTitle}>Własny transport</div>
      <DeliveryRow label="Na stanie" deliveryMessage="Produkt na magazynie, odbierz w dniu: {date}" expectedDeliveryDate={futureDate(1)} />
      <DeliveryRow label="Poza stockiem" deliveryMessage="Czas realizacji {days} dni, tj. {date}" expectedDeliveryDate={futureDate(7)} />
    </div>
    <div style={styles.section}>
      <div style={styles.sectionTitle}>Transport Eltap</div>
      <DeliveryRow label="Standardowy" deliveryMessage="Czas realizacji z dostawą {days} dni, tj. {date}" expectedDeliveryDate={futureDate(20)} />
    </div>
    <div style={styles.section}>
      <div style={styles.sectionTitle}>Brak komunikatu</div>
      <DeliveryRow label="Brak wiadomości" deliveryMessage={null} expectedDeliveryDate={null} />
    </div>
  </div>
);

export const AllScenarios = () => <DeliveryStory />;
AllScenarios.story = { name: "All scenarios" };

export const OwnTransportInStock = () => (
  <ExpectedDeliveryTime deliveryMessage="Produkt na magazynie, odbierz w dniu: {date}" expectedDeliveryDate={futureDate(1)} locale="pl" />
);
OwnTransportInStock.story = { name: "Własny — na stanie" };

export const OwnTransportOutOfStock = () => (
  <ExpectedDeliveryTime deliveryMessage="Czas realizacji {days} dni, tj. {date}" expectedDeliveryDate={futureDate(7)} locale="pl" />
);
OwnTransportOutOfStock.story = { name: "Własny — poza stockiem" };

export const EltapTransport = () => (
  <ExpectedDeliveryTime deliveryMessage="Czas realizacji z dostawą {days} dni, tj. {date}" expectedDeliveryDate={futureDate(20)} locale="pl" />
);
EltapTransport.story = { name: "Transport Eltap" };

const ExpectedDeliveryTimeStories = {
  title: "Shared/ExpectedDeliveryTime",
  component: ExpectedDeliveryTime,
};

export default ExpectedDeliveryTimeStories;
