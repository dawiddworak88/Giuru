import React from "react";
import ExpectedDeliveryTime from "../shared/components/ExpectedDeliveryTime/ExpectedDeliveryTime";
import "../shared/layouts/index.scss";
import "../shared/components/ExpectedDeliveryTime/ExpectedDeliveryTime.scss";

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

const DeliveryRow = ({ label, deliveryMessage, deliveryBusinessDays }) => (
  <div style={styles.row}>
    <span style={styles.label}>{label}</span>
    <span style={styles.result}>
      <ExpectedDeliveryTime deliveryMessage={deliveryMessage} deliveryBusinessDays={deliveryBusinessDays} locale="pl" />
    </span>
  </div>
);

const DeliveryStory = () => (
  <div style={styles.container}>
    <div style={styles.section}>
      <div style={styles.sectionTitle}>Własny transport</div>
      <DeliveryRow label="Na stanie" deliveryMessage="Produkt na magazynie, odbierz w dniu: {date}" deliveryBusinessDays={1} />
      <DeliveryRow label="Poza stockiem" deliveryMessage="Czas realizacji {days} dni, tj. {date}" deliveryBusinessDays={5} />
    </div>
    <div style={styles.section}>
      <div style={styles.sectionTitle}>Transport Eltap</div>
      <DeliveryRow label="Standardowy" deliveryMessage="Czas realizacji z dostawą {days} dni, tj. {date}" deliveryBusinessDays={14} />
    </div>
    <div style={styles.section}>
      <div style={styles.sectionTitle}>Brak komunikatu</div>
      <DeliveryRow label="Brak wiadomości" deliveryMessage={null} deliveryBusinessDays={0} />
    </div>
  </div>
);

export const AllScenarios = () => <DeliveryStory />;
AllScenarios.story = { name: "All scenarios" };

export const OwnTransportInStock = () => (
  <ExpectedDeliveryTime deliveryMessage="Produkt na magazynie, odbierz w dniu: {date}" deliveryBusinessDays={1} locale="pl" />
);
OwnTransportInStock.story = { name: "Własny — na stanie" };

export const OwnTransportOutOfStock = () => (
  <ExpectedDeliveryTime deliveryMessage="Czas realizacji {days} dni, tj. {date}" deliveryBusinessDays={5} locale="pl" />
);
OwnTransportOutOfStock.story = { name: "Własny — poza stockiem" };

export const EltapTransport = () => (
  <ExpectedDeliveryTime deliveryMessage="Czas realizacji z dostawą {days} dni, tj. {date}" deliveryBusinessDays={14} locale="pl" />
);
EltapTransport.story = { name: "Transport Eltap" };

const ExpectedDeliveryTimeStories = {
  title: "Shared/ExpectedDeliveryTime",
  component: ExpectedDeliveryTime,
};

export default ExpectedDeliveryTimeStories;
