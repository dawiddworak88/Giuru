# Spec: Assign Discount Codes to Clients

Vocabulary follows [CONTEXT.md](../../CONTEXT.md). The governing decision is [ADR 0001](../adr/0001-discount-code-eligibility-owned-by-giuru.md). The PR breakdown and release order are in [plan.md](./plan.md).

## Problem Statement

Today a Discount Code is free text. Any buyer can type any string into the Basket, and Giuru passes it to Grula as a Price Driver without checking anything. The Seller cannot see which codes exist, cannot decide which Clients may use a code, and cannot withdraw a code. The only way to stop a code is to change rules in Grula, which affects everyone at once.

The gaps are wider than the Basket form:
- Catalog and product price lookups accept any code as a request parameter, so discounted prices can be fetched without the code ever being applied.
- Buyer checkout takes the Client id from the request body instead of from the signed-in user.
- Team Members of a Client are not recognised as that Client at all. The buyer portal looks the Client up by the signed-in user's email, and a Team Member's email matches no Client. They are priced as an anonymous user, with no Client-specific Price Drivers, and they see no prices when prices are restricted to listed Clients.
- On a claims-cache miss the signed-in Client's id is missing from that request, so the first request every 30 minutes is priced as if nobody were signed in.

## Solution

The Seller manages Discount Codes in the Seller portal, through a "Discount codes" tile placed right after "Orders":
- The Seller creates a code with an optional description and an active/inactive switch.
- The Seller assigns it to none, one or more Clients with a searchable multiselect.
- The Seller can later edit the description, switch the code on and off, change its Assigned Clients, or delete it.
- The feature exists only when Grula is configured.

Wherever a code can be applied, Giuru accepts it only if it is a Valid Discount Code for the Client concerned. That covers:
- a buyer's Basket, by form or by order-file upload;
- a Team Member acting for their Client;
- the Seller placing an order on a Client's behalf;
- catalog and product price lookups;
- checkout.

An invalid new code is rejected with one generic message. A code that stops being valid while it sits on a Basket is dropped on the next save, with a message, and the lines are repriced. Checkout refuses a Basket whose code is no longer valid. Price lookups ignore an invalid code instead of failing.

Grula keeps computing the discount itself. Giuru never writes to Grula and never asks Grula whether a code is valid. Grula only ever receives a code that passed Giuru's check, spelled exactly as the Seller stored it.

Team Members are treated as their Client everywhere pricing is concerned. They get the Client's Price Drivers and can use the Client's Discount Codes.

## User Stories

### Managing Discount Codes (Seller)

1. As a Seller, I want a "Discount codes" tile directly after "Orders" in the Seller portal, so that I find code management next to the orders codes are applied to.
2. As a Seller, I want the tile and the Discount Code pages to be absent when Grula is not configured, so that I never manage codes that cannot affect any price.
3. As a Seller, I want a list of all Discount Codes showing the code, description, Active/Inactive status and number of Assigned Clients, so that I can see at a glance what is on offer and to whom.
4. As a Seller, I want to search the list by code, so that I can find a code quickly when a Client asks about it.
5. As a Seller, I want the list to be paged like the other Seller lists, so that it stays fast as codes accumulate.
6. As a Seller, I want to create a Discount Code by entering its code text, an optional description, whether it is active, and its Assigned Clients, so that I can issue a code in one step.
7. As a Seller, I want the code text to be trimmed and restricted to letters, digits, `-` and `_`, 3–50 characters long, with a clear error otherwise, so that codes are easy to type and to match in Grula.
8. As a Seller, I want to be stopped from creating a code whose text matches an existing code regardless of letter case, so that `SUMMER10` and `summer10` can never be two different codes.
9. As a Seller, I want the text of a deleted code to be reusable for a new code, so that a mistake can be corrected by deleting and recreating.
10. As a Seller, I want the text of an Inactive Discount Code to stay reserved, so that switching a code back on is never blocked by a newer duplicate.
11. As a Seller, I want the code text to be read-only once the code exists, so that Baskets, past Orders and Grula rules keyed on that text never silently drift apart.
12. As a Seller, I want to edit a code's description at any time, so that I can keep internal notes about its purpose current.
13. As a Seller, I want to switch a code between active and inactive without losing its Assigned Clients, so that I can pause a promotion and resume it later.
14. As a Seller, I want to pick Assigned Clients from a multiselect I can type into to filter by Client name, so that assigning a code to several Clients is quick even with many Clients.
15. As a Seller, I want to remove individual Clients from a code's Assigned Clients, so that I can withdraw a code from one Client without affecting the others.
16. As a Seller, I want to save a code with no Assigned Clients, so that I can prepare a code before deciding who gets it, knowing nobody can use it meanwhile.
17. As a Seller, I want a code with no Assigned Clients to be usable by nobody, so that deleting a Client or removing the last assignment never turns a private code into a public one.
18. As a Seller, I want to delete a code after confirming, so that obsolete codes disappear from the list.
19. As a Seller, I want deleted codes to remain visible as text on the Orders that used them, so that order history stays accurate.
20. As a Seller, I want deactivating, unassigning or deleting a code to take effect on the very next request, so that a withdrawn code cannot keep being used.
21. As a Seller, I want the Discount Code pages in English, German and Polish like the rest of the Seller portal, so that every Seller user can work with them.
22. As a Seller, I want clear save errors (invalid format, duplicate code, unknown Client) shown on the form, so that I can fix them without guessing.

### Applying a code (Client in the buyer portal)

23. As a Client, I want to apply a Discount Code assigned to me to my Basket and see the lines repriced, so that I get the discount I was given.
24. As a Client, I want my code to be accepted whatever letter case I type it in and with surrounding spaces ignored, so that small typing differences don't block me.
25. As a Client, I want the Basket to show the code as the Seller spelled it, so that I can tell it was recognised.
26. As a Client, I want an invalid code rejected with a clear "This discount code is not valid" message and my Basket left unchanged, so that a typo never costs me my current Basket or prices.
27. As a Client, I want the same message whether the code does not exist, is inactive or belongs to someone else, so that the rule is simple to understand.
28. As a Client, I want to still be told that a code needs items in the Basket before it can be applied, so that the existing guidance keeps working.
29. As a Client, I want to remove an applied code at any time, even one that is no longer valid, so that I am never stuck with it.
30. As a Client, I want a code that stopped being valid to be removed from my Basket on its next save, with a message saying so and the prices updated, so that I understand why my prices changed.
31. As a Client, I want checkout refused with a clear message if my Applied Discount Code is no longer valid, so that I never place an Order at prices I wasn't entitled to.
32. As a Client, I want to recover from that refusal by saving my Basket again and confirming the new prices, so that I can still place my Order.
33. As a Client, I want catalog, search, outlet, available-products and product pages to show discounted prices only when my code is valid, and normal prices otherwise without any error, so that browsing never breaks.
34. As a Client, I want the catalog and product pages to show my Applied Discount Code only while it is valid, so that I'm not told a code applies when it doesn't.
35. As a Client, I want codes in an uploaded order file to be checked exactly like codes typed into the Basket, so that both ways of ordering behave the same.
36. As a Client, I want my prices and my code check to be correct on every request, including the first one after a period of inactivity, so that a valid code is never dropped and prices never disappear at random.

### Team Members

37. As a Team Member, I want to be treated as my Client for Discount Codes, so that I can use every code assigned to my Client.
38. As a Team Member, I want to see my Client's prices, priced with my Client's currency, country, delivery address, extra packing and palette loading, so that my Basket matches what my Client would see.
39. As a Team Member, I want codes not assigned to my Client to be rejected exactly as they would be for my Client, so that the rules are the same for everyone in my organisation.
40. As a Team Member, I want checkout to check my Applied Discount Code against my Client, so that orders I place are priced correctly.
41. As a Client, I want my Team Members to be able to apply my Discount Codes without me sharing my own login, so that my colleagues can place discounted orders themselves.

### Placing orders on a Client's behalf (Seller)

42. As a Seller placing an order for a Client, I want to apply a Discount Code assigned to that Client, so that I can give the Client their discount over the phone or by email.
43. As a Seller placing an order for a Client, I want a code not assigned to that Client to be rejected exactly as for the Client, so that staff cannot hand out discounts outside the assignments.
44. As a Seller placing an order, I want the Applied Discount Code cleared when I switch to another Client, so that one Client's code never carries over to another.
45. As a Seller uploading an order file for a Client, I want its code checked against that Client, so that file uploads follow the same rule.
46. As a Seller checking out an order, I want the Applied Discount Code checked against the Client I'm checking out for, so that a Basket priced for one Client can't be ordered for another with the discount.
47. As a Seller viewing product prices for a selected Client, I want a code to affect those prices only if it is valid for that Client, so that the quotes I give are accurate.
48. As a Seller viewing an existing Order, I want to keep seeing the code it was placed with, even if that code has since been changed or deleted, so that I can answer questions about past Orders.

### Security and integrity

49. As a Seller, I want Clients unable to find out which codes exist or who holds them, so that private codes stay private.
50. As a Seller, I want an invalid code never sent to Grula, so that Grula rules can only be triggered by codes I issued to that Client.
51. As a Seller, I want buyer checkout refused when the Client id in the request doesn't match the signed-in Client, so that nobody can place orders under another Client's identity.
52. As a Seller, I want discount-code checks to apply equally to Basket saves, file uploads, price lookups and checkout, so that there is no side door around the rules.

### Grula and rollout

53. As a Seller, I want Giuru to keep sending the code to Grula as the "Discount Code" Price Driver after it passes the check, so that the discounts I configured in Grula keep working.
54. As a Seller, I want Giuru never to create, change or check anything in Grula, so that pricing rules stay solely under my control in Grula.
55. As a Seller, I want the current behaviour kept when Grula is not configured (incoming code changes ignored, stored code kept), so that turning Grula off doesn't lose data.
56. As a Seller, I want the management pages released before enforcement, so that I can create the codes in use and assign their Clients before anyone loses a code.
57. As a reviewer, I want the work delivered as six small pull requests, each releasable on its own, so that each one can be reviewed carefully.

## Implementation Decisions

### Ownership and boundaries
- **Client.Api** owns Discount Codes and their Assigned Clients (ADR 0001). The code↔Client relation lives next to Clients, so removing a Client and its assignments happens in one database.
- **Ordering** is unchanged. An Order keeps the code as plain text and never references a Discount Code record.
- **Grula** is untouched. The code is still sent as the existing "Discount Code" Price Driver, now always with its stored spelling.
- **The gate** is the existing "Grula configured" setting (access token + environment id). There is no new flag. With Grula off:
  - the tile is omitted;
  - the Seller pages and management API respond 404;
  - no eligibility check runs;
  - the existing "ignore incoming code changes, keep stored code" behaviour stays.

### Schema (Client.Api)
- **Discount Code entity:**
  - Fields: code text (trimmed, stored as entered), optional description, an active switch, scoped to the Seller like Clients are, plus the standard audit fields.
  - The active switch is its own column, separate from the base entity's soft-delete flag, because the base flag means "not deleted" throughout the codebase. It follows the Client "disabled" convention.
- **Assigned Client entity:** links one Discount Code to one Client, modelled like the existing Client↔Client group link.
- **Uniqueness:** among non-deleted codes of a Seller, ignoring case. It is enforced by the service validator and backed by a filtered unique index on non-deleted rows. The database's case-insensitive collation provides the case-insensitivity.
- One EF migration.

### Client.Api contract
- **List:** paged, with a search term matching code text and the usual ordering parameter. Each item carries id, code, description, active, number of Assigned Clients and the audit dates.
- **Get by id:** the code's fields plus its Assigned Client ids.
- **Save:** creates or updates. Input is id (absent on create), code, description, active and Assigned Client ids.
  - On update the code text must equal the stored one, otherwise the save is rejected.
  - Unknown or deleted Client ids are rejected.
  - Format and uniqueness violations are rejected with localised messages.
- **Delete:** soft delete.
- **Eligibility check:** input is a Client id and code text.
  - When the code is a Valid Discount Code for that Client, it returns the code's stored spelling.
  - Otherwise it returns no content with no reason, so callers cannot tell a missing code from an unassigned or inactive one. It is not "not found", so a caller can tell "not valid" from a missing route.
  - Valid means: a case-insensitive match on trimmed text, the code is not deleted and is active, and the Client is among the Assigned Clients and not deleted.
  - The caller's organisation must be the Client's own (the Client itself or one of its Team Members) or the Client's Seller. Any other caller gets the same empty answer, so the check cannot be used to probe another Client's codes.
  - No caching anywhere: revocation takes effect on the next request.

### Shared eligibility module (Foundation.Pricing)
- **New eligibility abstraction:** "given a Client and a requested code, return the stored spelling if valid, otherwise nothing". Each web app implements it with a repository calling the Client.Api eligibility check. That repository is the one new test seam.
- **The Basket-save coordinator** is extended to take the Client and use the eligibility abstraction. It now produces one of these outcomes:
  - **apply** a valid new code, using its stored spelling;
  - **reject** an invalid new code with the generic localised message, leaving the Basket unchanged;
  - **keep** a still-valid stored code;
  - **drop** a stored code that is no longer valid, reprice without it, and return a localised "code removed" message;
  - **clear** on explicit removal. Removal never needs a check.
- The existing rules are kept: an omitted or blank code means "unchanged", an explicit null means "remove", and applying a new code to an empty Basket is rejected.
- **Both price-client resolvers** (the buyer resolver based on claims, and the Seller resolver based on Client lookup) set the discount code on the price request only when it is valid for that Client. Otherwise they leave it empty and never fail. This one change covers every catalog, search, outlet, available-products, product-variant and single-price lookup on both portals.

### Web apps
- **Client used for checks:**
  - Buyer portal: always the Client from the signed-in user's claims.
  - Seller portal: the Client id the request carries for Basket save, file upload, price lookup and checkout.
- **Basket save** uses the extended coordinator on all four paths (buyer and Seller × form and order-file upload). The save response gains a field carrying the "code removed" message. The front-end discount-code hooks show it and sync the displayed code.
- **Client.Api unavailable:** a Basket save that needs the eligibility check fails with an error. The Basket is neither priced with an unverified code nor stripped of a code that may still be valid. Saves that need no check (no code, removal, Grula off) are unaffected. A save that keeps a stored code does need it, so while Client.Api is down even adding or deleting a line fails on a Basket that holds a code.
- **Server-rendered pages** (catalog, product detail, order form) show the Applied Discount Code only when it is still valid.
- **Checkout** (buyer and Seller) checks the Basket's Applied Discount Code against the checkout Client before handing the Basket to Basket.Api. An invalid code rejects checkout with a localised message, and nothing is checked out.
- **Buyer checkout** additionally rejects a request whose Client id differs from the signed-in Client.

### Buyer claims enrichment (Team Members)
- The Client is resolved from the signed-in user's organisation, using the existing Client.Api lookup by organisation, instead of by email. Team Members, who share their Client's organisation, therefore get the Client's id and all Client-derived claims (name, currency, country, delivery address, extra packing, palette loading, delivery type).
- The Client id claim is added to the current request on a cache miss as well, not only written to the cache.
- This change stands alone and changes the prices Team Members see.

### Seller portal
- **Tile:** inserted immediately after "Orders" and omitted when Grula is not configured.
- **Pages:** live in the Clients area next to Client groups, following the existing list-page/form-page/API-controller pattern. The Seller web app proxies to Client.Api.
- **List:** code, description, the existing Active/Inactive chip, number of Assigned Clients, search, delete with confirmation.
- **Form:**
  - code (read-only after create), description, active switch;
  - Assigned Clients via MUI Autocomplete in multiple mode with chips, with all Clients preloaded server-side from the same source as the order form's Client picker.
- All labels and messages are localised in en/de/pl.

## Testing Decisions

### What a good test is
- It drives behaviour through the highest available seam: a web app's HTTP API in integration tests, or a controller action or resolver's public method in unit tests.
- It asserts only on what's observable there: the response status and message, the Basket as persisted and read back, and the discount code handed to the price service.
- It never asserts on internal calls, call order or private helpers.
- Each scenario names the rule it pins.

### Seam 1: web app HTTP APIs, integration tests (run locally, excluded from CI)
- Seller.Web and Buyer.Web run in-process against real Client.Api, Basket.Api, Ordering, SQL Server and Redis containers. Grula is configured but unreachable, so Baskets persist with prices cleared.
- **PR 1 (management, through Seller.Web into Client.Api):**
  - create, read, update and list with search;
  - format errors;
  - uniqueness ignoring case;
  - reuse of a deleted code's text but not an inactive one's;
  - immutable code text;
  - unknown Client ids rejected;
  - soft delete.
- **PR 5 (apply):**
  - Seller Basket save and order-file upload: a valid code is stored with its stored spelling; an invalid code returns 400 and leaves the Basket unchanged; a stale stored code is dropped with a message after its assignment is removed or it is deactivated or deleted.
  - Buyer Basket save as a Team Member: the test creates a Client whose organisation equals the test user's organisation and whose email differs, so the test user is that Client's Team Member, and the Client's codes apply.
- **PR 6 (checkout):**
  - checkout refused for a code that became invalid;
  - Seller checkout for a different Client than the code's;
  - buyer checkout with a mismatching Client id.
- The run result is noted in each PR description.

### Seam 2: controller and resolver unit tests with a substituted price service (CI)
- These cover every outcome of the extended coordinator on all four save paths:
  - for each, the persisted code and the code handed to the price service;
  - proof that an invalid code never reaches the price service.
- Both price-client resolvers: the price request carries the code only when the eligibility check returns it, and nothing fails otherwise.
- Checkout: refusal on an invalid code, and on a Client id mismatch for the buyer.

### Seam 3: the eligibility repository per web app
- Substituted in Seam 2. Real in Seam 1.

### Claims enrichment
- Unit tests with the existing substituted Clients repository:
  - the Client is resolved by organisation;
  - a Team Member gets the Client's claims;
  - the Client id claim is present on a cache miss and on a cache hit.

### Prior art
- **Integration:** Seller web tests (Client create/edit through Seller.Web into Client.Api), Basket discount-code tests, Basket pricing tests, buyer web tests.
- **Unit:** Basket API controller flow tests, order-file API controller flow tests, Basket checkout tests, price-client resolver tests, discount-code resolver tests, claims-principal extension tests, Client lookup service tests.

### Front end
- No new front-end test infrastructure; the repo only has a helper-level `node --test`.
- The tile, list, form, multiselect and "code removed" message are verified by a front-end build plus a manual run of both portals.

## Out of Scope

- Validity dates and usage limits (per Client or in total) for Discount Codes.
- Any write to or check against Grula, including creating Grula rules for new codes.
- Showing a Client's Discount Codes on the Seller's Client page.
- Showing buyers which codes they can use.
- Public codes available to every Client.
- Backfilling Discount Codes from codes found on past Orders or Baskets.
- A Seller override that applies a code to a Client it isn't assigned to.
- Caching eligibility results.
- New front-end test infrastructure.

## Further Notes

### Rollout
1. Release PRs 1–4 to `main` first.
2. The Seller creates the codes currently in use, assigns their Clients, and checks the matching rules exist in Grula.
3. Only then release PRs 5–6.

From that point, any code on a Basket that hasn't been set up is dropped with the "code removed" message. Prices never change silently.

### Pull requests
Each PR targets `develop`. Details are in [plan.md](./plan.md).
1. Docs + Client.Api storage and management API.
2. Seller list page and tile.
3. Seller create/edit form.
4. Buyer: Team Members act as their Client.
5. Enforce on apply.
6. Enforce on reads and checkout.

### Other notes
- **Operational:** creating a code in Giuru does not create a discount. The discount amount and conditions live only in Grula, keyed on the code's exact spelling.
- **Assumption:** Clients disabled by the Seller still appear in the Assigned Clients multiselect and keep their assignments. Eligibility requires only that the Client is not deleted, so a disabled Client's code still works on orders the Seller places for them.
- **Behaviour change in PR 4:** Team Members' prices change, because they are now priced with their Client's Price Drivers.
