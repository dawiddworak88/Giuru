# Assign Discount Codes to Clients: implementation plan

Terms follow [CONTEXT.md](../../CONTEXT.md). The architectural decision is recorded in [ADR 0001](../adr/0001-discount-code-eligibility-owned-by-giuru.md).

## Goal

The Seller manages Discount Codes in the Seller portal and assigns each one to none, one or more Clients. Every place a code can be applied, by a buyer, a Team Member or the Seller on a Client's behalf, accepts it only if it is a Valid Discount Code for that Client. Grula still computes the discount. Giuru never writes to Grula and never validates against it.

## Rules

### Gate
- The whole feature (tile, pages, management API calls, validation) exists only when `IsGrulaConfigured` is true (`GRULA_ACCESS_TOKEN` + `GRULA_ENVIRONMENT_ID`). There is no new flag.
- With Grula off, the tile is hidden, the Seller pages and API return 404, and the existing behaviour stays: incoming code changes are ignored and the stored code is kept.

### A Discount Code
- Fields: code, optional description, active/inactive switch, Assigned Clients.
- No validity dates and no usage limits.
- Code text: letters, digits, `-`, `_`, 3–50 characters, trimmed.
- Unique per Seller, ignoring case and ignoring deleted codes.
- The code text cannot be changed after creation. To rename, delete and create.
- No Assigned Clients means nobody can use the code. Assignment never widens access implicitly.
- Inactive means nobody can use the code. It stays listed and editable, and its text stays reserved.
- Delete is a soft delete. Past Orders keep the code text, which is plain text on the Order and never a reference.

### Validation
- **Valid Discount Code for Client C:** the code exists (case-insensitive match), is active, and C is an active Client among its Assigned Clients.
- **Who counts as Client C:**
  - Buyer portal: the Client resolved from the signed-in user's `OrganisationId`, so Team Members count as their Client.
  - Seller portal: the Client selected on the order.
- **Seller override:** none. Seller staff follow the same rule as buyers.
- **Text stored and sent to Grula:** the code's stored spelling, never the text as typed.
- **Rejection message:** one generic "This discount code is not valid" whether the code doesn't exist or isn't assigned, so nobody can probe which codes exist.
- **No cache:** every check is a live call to Client.Api, so revocation takes effect on the next request.

| Where | Valid | Invalid |
|-------|-------|---------|
| Basket save, new code (JSON and file upload, buyer and seller) | Stored with its stored spelling, lines repriced | 400 with the generic message, Basket unchanged |
| Basket save, code already stored that has become invalid | Kept | Dropped, lines repriced without it, "code removed" message shown |
| Catalog / product price lookups carrying `discountCode` (buyer and seller) | Priced with the code | Priced without the code, no error |
| Server-rendered pages showing the Applied Discount Code | Shown | Not shown |
| Checkout (buyer and seller) | Proceeds | Rejected. The user saves the Basket again, which drops the code and reprices, and confirms the new prices. |
| Buyer checkout whose `ClientId` doesn't match the signed-in Client | n/a | Rejected |

### Seller portal
- "Discount codes" tile placed right after "Orders".
- Pages live in the `Clients` area, next to Client groups.
- List: code, description, Active/Inactive chip, number of Assigned Clients, search by code, delete.
- Form: code (read-only after create), description, active switch, Assigned Clients via MUI `Autocomplete` with `multiple`, with Clients preloaded the same way as the `OrderForm` Client picker.
- Localised in en/de/pl.

### Out of scope
- Showing a Client's codes on the Client page.
- Showing buyers which codes they can use.
- Usage limits and validity dates.
- Any Grula write or check.
- Backfilling codes from past Orders.

## Pull requests

Every PR targets `develop`, stays behind the Grula gate, and leaves `develop` releasable. Code PRs include unit tests in `Giuru.UnitTests`. Where noted, integration tests are extended and run locally (they are excluded from CI), and the result goes in the PR description.

1. **Docs + Client.Api storage and management API** (branch `chore/Assign-discount-codes-to-clients`)
   - This plan, `CONTEXT.md` and ADR 0001.
   - Discount Code and Assigned Client entities, plus the EF migration.
   - Endpoints: paged list with search, get by id, save (code, description, active, client ids), soft delete.
   - Validators for format, uniqueness and immutable code text.
   - Seller repository and API controller proxying to Client.Api, returning 404 when Grula is off.
   - Integration tests, through the Seller API into Client.Api.
2. **Seller: list page + tile**
   - List page, delete.
   - Tile after Orders, hidden/404 when Grula is off.
   - Localisation.
3. **Seller: create/edit form**
   - Form with the Assigned Clients multiselect.
   - Save/validation errors shown on the form.
4. **Buyer: Team Members act as their Client**
   - Claims enrichment resolves the Client by `OrganisationId` instead of by email, which gives Team Members all of the Client's price drivers.
   - Fixes the missing `ClientId` claim on a claims-cache miss.
   - Independent of the other PRs. It changes the prices Team Members see.
5. **Enforce on apply**
   - Client.Api "is this code valid for this Client" endpoint, which returns the stored spelling.
   - A shared validator in `Foundation.Pricing`, used by `BasketDiscountCodeCoordinator`.
   - Covers all four Basket save paths: buyer and seller, JSON and file upload.
   - FE handling of the "code removed" message.
   - Integration tests.
6. **Enforce on reads + checkout**
   - `IPriceClientResolver` drops invalid codes on every price lookup.
   - Server-rendered pages show only a Valid Applied Discount Code.
   - Checkout re-validates the code on buyer and seller.
   - Buyer checkout rejects a `ClientId` that doesn't match the signed-in Client.
   - Integration tests.

## Release

1. Release PRs 1–4 to `main`.
2. The Seller creates the codes in use and assigns their Clients, and configures the matching rules in Grula if they are not already there.
3. Only then release PRs 5–6.

Any code left unconfigured is dropped from Baskets, with the "code removed" message, from that point on.
