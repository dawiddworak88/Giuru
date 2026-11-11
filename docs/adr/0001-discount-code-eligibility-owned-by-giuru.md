# Discount Code eligibility is owned by Giuru, not Grula

Discount Codes started as free text passed straight to Grula as the `Discount Code` price driver, so anyone could try any code. We decided that Giuru decides who may use a code: Client.Api stores the codes, whether each is active, and its Assigned Clients (Team Members count as their Client). Every web app checks the code against the Client before pricing, saving a Basket or checking out. Grula only ever receives a code that passed that check, and it stays the sole authority on what discount a code gives.

## Considered Options

- **Validate against Grula**: rejected. Grula has no notion of which Client may use a code, and asking it on every request would couple eligibility to an external service we only use for prices.
- **Store codes in Ordering.Api**: rejected. The core relation is code ↔ Client, so it lives next to Clients. Deleting a Client and its assignments happens in one database, and Orders keep only the code text, so past Orders never depend on a code still existing.

## Consequences

- Creating a code in Giuru does not create a discount. The Seller still has to configure the matching rule in Grula, and Giuru never writes to Grula.
- An Applied Discount Code that stops being valid is dropped from the Basket on its next save (with a message), blocks checkout until the Basket is saved again, and is ignored by catalog price lookups.
- Eligibility is checked live with no cache, so unassigning or deactivating a code takes effect on the next request.
