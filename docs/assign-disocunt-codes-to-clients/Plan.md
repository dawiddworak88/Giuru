# Plan: discount codes - management, client assignment and validation (v2)

Branch `feature/Added-assinging-discount-codes-to-clients-v2`, based on `develop` at `dcbf8b3d8`. Written and reviewed 2026-10-09.

Paths are relative to `be/src` unless they start with `fe/` or `misc/`. This document describes planned work; implementation tests have not been run as part of this review. Current production GitOps configuration has not been inspected.

## 1. Goal

1. Sellers manage discount codes in Seller.Web + Seller.Portal: list, create, edit, activate/deactivate and delete.
2. Assign zero, one or many clients on the discount code form. A code assigned to nobody cannot be newly applied by anybody.
3. Buyer.Web and Seller.Web validate **new or replacement** basket codes and codes supplied to fresh product price calculations: the code must exist, be active and be assigned to the pricing client.
4. A code already stored in a basket is retained through edits/uploads without revalidation and may proceed to checkout. Basket repricing continues using that stored code. Checkout keeps today's stored-price behaviour.
5. Client team members can use their client's codes and receive that client's pricing context.
6. Web management and application are available only when `AppSettings.IsGrulaConfigured` is true (`GrulaAccessToken` set and `GrulaEnvironmentId` a valid Guid). Client.Api itself has no Grula feature gate.
7. Giuru never creates a code in Grula or checks its existence there. Grula continues deciding its price effect through the existing `Discount Code` price driver.

These guarantees apply to the web entry points. Direct authenticated Basket.Api calls remain outside the enforcement boundary by explicit decision C8.

## 2. Confirmed decisions

| # | Topic | Decision | Source |
|---|-------|----------|--------|
| C1 | Zero assigned clients | Nobody can newly apply the code | Existing answer, 2026-10-09 |
| C2 | Applicability | Exists, active, assigned to the client. No validity dates or usage limits. Applies to new/replacement codes and fresh product pricing, with the stored-basket exception in C9 | Existing answer, refined in this review |
| C3 | Assignment UI | Discount code form only; leave the client form unchanged | Existing answer, 2026-10-09 |
| C4 | Existing deployed codes | None deployed or used. One release, no seeding or legacy migration | Existing answer, 2026-10-09 |
| C5 | Grula independence | No writes to Grula and no code existence check against Grula | Brief |
| C6 | Feature switch | No new flag; use `IsGrulaConfigured` in the web apps | Brief |
| C7 | Client team members | Include organisation lookup and its associated client prices, currency and price visibility. Codes assigned to the client also apply to its team members | Answered in this review |
| C8 | API access | Leave Basket.Api and deployment access as they are for now. Do not assume APIs are private; accept that direct Basket.Api calls bypass web validation | Answered: "Leave as is for now" |
| C9 | Stored basket codes | Validate only new/replacement codes on saves; keep accepted stored codes through later edits/uploads without revalidation and allow checkout. Fresh product price queries still check applicability | Answered and clarified in this review |
| C10 | Stored prices and races | Accept existing stored-price behaviour and the validation-to-checkout race. No basket pricing-client binding or version checks | Answered in this review |
| C11 | Editable/reusable text | Allow renaming, deletion and reuse. Old baskets may use a replacement code with the same text | Answered in this review |
| C12 | Organisation mapping | Each buyer organisation has one active client record. Retain the existing organisation lookup | Answered in this review |

C9 deliberately permits an already stored code to continue after deactivation, unassignment, deletion, renaming or a client switch. Revocation prevents new application and fresh product pricing; it does not revoke existing baskets.

## 3. Resolved questions and implementation assumptions

There are no remaining product or architecture questions blocking this plan. The review resolved the decisions that would change basket contracts, checkout scope, team-member identity or code lifetime.

| Previous question | Resolution |
|-------------------|------------|
| Q1: team members | C7; implement organisation fallback and fix the first-request client claim |
| Q2: seller override | No override for new/replacement codes or fresh product pricing, consistent with C2. Existing basket codes have the same C9 exemption in both apps |
| Q3: revoked stored code | C9; retain it, reprice basket edits using it, and allow checkout. No automatic removal notice or checkout refusal |
| Q4: Client.Api outage | A new/replacement code cannot be verified, so reject that save with a retry message. Fresh product calculations omit an unverified code. Stored-code saves, explicit removal and checkout do not require discount validation |
| Q5: no Grula effect | Consequence of C5: an applicable Giuru code can have no monetary effect. Explain this in the admin hint; do not promise a saving in buyer feedback |
| Q6: API boundary | C8; unchanged access, explicit direct-API bypass limitation |
| Q7: client binding and basket version | C10; retain stored prices and races, including cross-client prices |
| Q8: rename/reuse | C11; baskets identify codes by text and retain the original stored spelling until explicitly replaced |
| Q9: ambiguous buyer organisation | C12; one active client record per buyer organisation |

Remaining defaults are explicit so implementation does not invent additional business rules:

| # | Assumption |
|---|------------|
| A1 | New application matches case-insensitively and ignores surrounding spaces. Store the administrator's trimmed spelling for newly accepted codes and send that value to Grula |
| A2 | Compare a requested code with the stored code after trimming, using `OrdinalIgnoreCase`. The same text is an unchanged code, even when the frontend explicitly resends it. Preserve its stored spelling without validation. Applying a renamed or replacement value requires a different text or removal followed by application |
| A3 | Deleting a code is a soft delete, allowed while clients are assigned. Remove its assignment rows. Already stored baskets continue under C9 |
| A4 | Optional description, max 256 characters, for operator notes only |
| A5 | Buyers get one generic rejection for unknown, inactive, unassigned or unknown-client codes. Sellers get the specific reason |
| A6 | Client.Api CRUD/options require a seller role and a valid organisation claim. Validation permits authenticated buyers and sellers. These endpoints have no Grula gate, including direct calls while the web feature is off |
| A7 | Codes are scoped by `SellerId` derived from the seller's organisation claim. Buyer validation verifies the client's organisation and uses that client's seller |
| A8 | Seller order form retains its free-text field. A dropdown of assigned codes is out of scope |
| A9 | With Grula off, preserve stored basket codes and ignore incoming mutations, as today's coordinator does. No discount validation occurs |
| A10 | Client `IsDisabled` is not an applicability rule. Client `IsActive` means not deleted; assignment options and new validation require it |
| A11 | Protect admin form edits/deletes with the existing entity `RowVersion`, so two open forms do not silently replace each other's assignment lists. This is separate from the basket version checks excluded by C10 |

## 4. Repository verification

Verified against `dcbf8b3d8` and the current working tree:

- Codes are strings, max 64 characters, saved in Redis by Basket.Api and copied to `Order.DiscountCode`. `PriceService.CreatePriceDrivers` sends them to Grula through `PriceDriversConstants.DiscountCodeDriver`.
- There is no current existence, activity or assignment check.
- `PriceClient` is constructed by Buyer `ClaimsPriceClientResolver` and Seller `ClientRepositoryPriceClientResolver`. Product price APIs and catalog/product pricing builders use `IPriceClientResolver`.
- JSON basket saves and order-file uploads in both apps use `BasketDiscountCodeCoordinator` and reprice through a client resolver.
- Checkout neither reprices nor validates codes. Basket.Api's `BasketService.CheckoutAsync` copies stored prices and the stored code into the checkout event. C9/C10 leave this behaviour unchanged.
- Buyer checkout takes the order client from the request body and checks stock using the request-body basket id, while checking out the cookie basket. This existing discrepancy is not fixed in this release (C10).
- Buyer `Shared/Services/Baskets/BasketService.GetBasketAsync` copies stored prices into the order form/sidebar. A product-pricing decorator does not reprice these basket reads.
- `ClaimsEnrichmentMiddleware` finds the client by email and caches claims for 30 minutes. On an uncached request it puts `ClientId` in the cache list but **does not add it to the current principal**. Fix this in Phase 7 for primary contacts and team members alike.
- Buyer `IClientsRepository.GetClientAsync` uses the organisation endpoint and throws `CustomException` on non-success; `OrderFormModelBuilder` already uses it. Client.Api's organisation lookup returns the first active match, relying on C12.
- `ClientsService.Get(GetClientsServiceModel)` does **not filter by seller**, despite receiving `OrganisationId`. Seller `GetAllClientsAsync` follows that endpoint in sequential pages. Do not use it for the new assignment picker.
- `ClientContext` has no `OnModelCreating`; registration ignores `PendingModelChangesWarning`. The migration/snapshot must be reviewed for unrelated schema drift.
- `Entity` already has `RowVersion`, dates and `IsActive`. Use `FillCommonProperties()` for new code/link entities; otherwise `IsActive` defaults to false. Seller.Portal's shared `Catalog` currently sends only `id` when deleting; versioned deletes need the small adapter in Phase 5.
- Seller conventional routes require `SellerOnly`; Buyer routes require authentication.
- `misc/helm/Giuru/templates/ingress.yaml` exposes Client.Api and Basket.Api. API bearer authentication uses the shared `all` audience and Basket endpoints use plain `[Authorize]`. Production CD workflows update `Team-Eltap/giuru-gitops`, whose current ingress/network policy is not in this workspace. C8 leaves access unchanged.

### Entry points and intended enforcement

| Path | Client source | Code source | Target behaviour |
|------|---------------|-------------|------------------|
| Buyer `BasketsApiController.Index`, `OrderFileApiController.Index` | Principal claims | Request or stored basket | Validate new/replacement text; preserve matching stored text |
| Seller equivalents | `model.ClientId` | Request or stored basket | Same rule; specific rejection messages |
| Buyer product APIs: `ProductsApiController` (`GetPrice`, `GetProductVariants`, `GetProductsQuantities`), `AvailableProductsApiController`, `OutletApiController`, `SearchProductsApiController` | Principal claims | Query string | Decorator validates, omits invalid/unverified codes |
| Buyer pricing builders: `AvailableProductsCatalogModelBuilder`, `OutletCatalogModelBuilder`, `SearchProductsCatalogModelBuilder`, `ProductDetailModelBuilder` | Principal claims | Stored basket | Fresh product calculations validate; stored basket totals are unaffected |
| Seller `ProductsApiController.GetPrice/GetProductsQuantities` | Query `clientId` | Query `discountCode` | Decorator validates |
| Both `BasketCheckoutApiController.Checkout` | Existing checkout logic | Stored basket | Unchanged; no discount validation or repricing |

## 5. Target design

Client.Api owns `DiscountCodes` and `ClientsDiscountCodes`. Grula owns the price effect. Web apps own the feature gate.

There are **two enforcement points**, not three:

1. Fresh product pricing: a decorator around `IPriceClientResolver` validates the code and leaves it out when inapplicable or unverifiable.
2. Basket write: the coordinator checks only a new/replacement code. It preserves stored codes and rejects unverifiable new ones.

**Basket repricing must bypass the fresh-product decorator.** Otherwise the decorator would revalidate a stored code and undo C9. Introduce `IBasketPriceClientResolver : IPriceClientResolver`; the two existing concrete resolvers implement this interface and are exposed through it for basket callers. The ordinary `IPriceClientResolver` registration is the decorator. Only the four save/upload paths may use the basket resolver, after coordinator resolution.

```text
Product price read -> verified IPriceClientResolver -> Client.Api applicability -> Grula
Basket save/upload -> coordinator:
                      new/replacement -> Client.Api applicability
                      stored/removal  -> no discount lookup
                   -> IBasketPriceClientResolver -> Grula -> Basket.Api
Checkout           -> existing Basket.Api flow (stored code and prices)
```

The request-scoped validator shares a lookup result within one request. Do not add cross-request caching; it would delay rejection on fresh application/pricing.

## 6. What to reuse from v1

v1 is on `origin/feature/Added-assigning-discount-codes-to-clients` (`34790aeea`, `2baef06c0`), roughly 10,900 lines across 181 files. Use it as a file-by-file reference; do not cherry-pick the branch wholesale.

| v1 element | v2 |
|------------|----|
| Client.Api code ownership, CRUD, applicability rule | Reuse structure; add seller-scoped assignment options and assignment on code form |
| Grula code service/cache/spike and missing-Grula-code rejection | Omit (C5) |
| `DiscountCodeEnforcementEnabled` and enforcement settings extensions | Omit (C6) |
| Two releases and order-history seeding | Omit (C4) |
| Client form assignment changes | Omit (C3) |
| Immutable code text | Editable and reusable (C11) |
| Basket pricing-client binding/version contracts and checkout validation | Omit (C9/C10) |
| Automatic removal of stale stored codes and frontend notices | Omit (C9) |
| Team-member lookup, `RequireGrula` filter, generic buyer rejection | Keep; also fix first-request `ClientId` and use distinct product/basket resolver registrations |

## 7. Implementation steps

One PR, with a commit per phase. Build affected projects and run applicable unit tests as each phase is wired. Run the full integration matrix after both web apps and frontend are complete.

### Phase 1 - Client.Api data model

1. Add entities under `Project/Services/Client/Client.Api/Infrastructure/DiscountCodes/Entities/`:
   - `DiscountCode : Entity`: required `Code` (max 64), optional `Description` (max 256), `IsDisabled`, required `Guid SellerId`.
   - `ClientsDiscountCode : Entity`: required `Guid ClientId` and `Guid DiscountCodeId`.
2. Add their DbSets to `ClientContext`. Add `OnModelCreating`, call the base method, and configure only the new model:
   - explicit case-insensitive `Latin1_General_100_CI_AS` collation on `Code`;
   - unique `DiscountCodes(SellerId, Code)` index filtered by `[IsActive] = 1`, allowing reuse after soft deletion;
   - unique `ClientsDiscountCodes(DiscountCodeId, ClientId)` and a `ClientId` index;
   - foreign keys to clients/codes with `DeleteBehavior.Restrict`, without navigation properties (context uses lazy-loading proxies).
3. Generate `AddedDiscountCodes` in `Infrastructure/Migrations` from Client.Api. Review migration, designer and snapshot. Only the two tables/indexes/keys may change. Resolve existing snapshot drift separately before accepting generated changes to existing tables.
4. Verify startup migration on an empty and an existing database. Do not change the existing warning suppression as part of this feature.

The 64-character limit matches existing basket validators and `OrderingContext`.

### Phase 2 - Client.Api CRUD, assignment and validation

Use the `ClientRoles` slice for structure, with explicit seller scoping.

1. Add models/validators in `ServicesModels/DiscountCodes` and `Validators/DiscountCodes`:
   - create/update: trimmed nonblank `Code` max 64, optional `Description` max 256, `IsDisabled`, non-null `ClientIds` (empty valid);
   - get/list/delete/validation models, paged client-option model;
   - detail/list response: id, code, description, disabled, assigned client ids/count, dates, base64 `Version` from `RowVersion`;
   - update/delete require a valid version. Caller organisation must be present and not `Guid.Empty`.
2. Add `IDiscountCodesService`/`DiscountCodesService`:
   - CRUD/list/options filter `SellerId ==` caller organisation; active means `IsActive`, not `!IsDisabled`.
   - Create trims text and initializes common properties on code and links. Duplicate active text for the seller gives 409 `DiscountCodeExists`.
   - Update supports rename, description, activity and full assignment replacement. Validate all distinct client ids against active clients of this seller **before mutating anything**; foreign/unknown/deleted ids give 422 `DiscountCodeClientsInvalid`.
   - Diff assignment rows: remove only deselected links and add only new ones. Do not delete/reinsert unchanged pairs against the unique index.
   - Save code and links in one `SaveChangesAsync`. Catch code-index uniqueness violations (2601/2627) for both create and rename, including concurrent writes; do not mislabel other constraint failures.
   - Use the submitted `RowVersion` as the original value on update/delete. Always update the code row/date for an assignment-only edit; return 409 `DiscountCodeChanged` for a concurrency conflict. Return the new version after saving.
   - Delete soft-deletes the code, updates its date and removes links in one save.
   - Detail/client count includes only still-active clients. Search by `Code.StartsWith`, reuse paging/sorting conventions with stable ordering.
3. In `ClientsService.DeleteAsync`, remove that client's code links in the same save as the existing soft delete. Leave the client form and all other assignment contracts unchanged.
4. Validation:
   - Trim code. Load an active client by id.
   - Seller must own it (`client.SellerId == OrganisationId`); buyer must belong to it (`client.OrganisationId == OrganisationId`), which permits team members.
   - Find active code for that client's seller; return `NotFound`, `Disabled`, `NotAssigned`, `ClientUnknown`, or `Valid` with stored spelling.
   - For buyers, collapse non-valid business results to `NotApplicable` without a code. Derive seller role and organisation from the principal, never request fields.
5. Add authenticated `v1/Controllers/DiscountCodesController` at `/api/v1/discountcodes`:
   - Seller-only list, detail `{id:guid}`, POST create/update, DELETE `{id:guid}?version=` and `GET clients` (paged assignment options). Non-seller gets 403. The options return only active clients of this seller, with id/name; include disabled clients under A10.
   - `GET validation?code=&clientId=`: authenticated buyers/sellers. 200 for business results; 422 for malformed/blank/overlong input or missing/empty identity. Require a valid non-empty organisation claim. The GUID constraint prevents static routes being treated as ids.
   - Return status strings; Client.Api must not reference Foundation.Pricing.
6. Register dependencies and `ApiConstants.Client.DiscountCodesApiEndpoint`. Add localized resources (section 9).

### Phase 3 - Foundation.Pricing policy and basket resolution

1. Add `DiscountCodes/` contracts:
   - `DiscountCodeValidationStatus`: `Valid, NotFound, Disabled, NotAssigned, ClientUnknown, NotApplicable, Unavailable`.
   - Result with status, canonical `DiscountCode` when valid, and `IsValid`.
   - `IDiscountCodeLookup.ValidateAsync(Guid clientId, string code, string token)` implemented per web app.
   - Scoped `IDiscountCodeValidator.ValidateAsync(Guid? clientId, string code, string token)`.
2. Validator: missing/empty client returns `ClientUnknown` without lookup; invalid text returns `NotFound`. HTTP failure, exception, timeout, unknown status or invalid canonical value returns `Unavailable`, logged without tokens. Check valid returned text is nonblank and within 64 characters. Cache result tasks by token, client id and trimmed case-insensitive text behind a lock, sharing concurrent calls.
3. Add `VerifiedDiscountCodePriceClientResolver` around the existing resolver. Resolve the inner client with a null code; when Grula is configured and text is present, validate **the resolved client's id**. Set canonical code only when valid; otherwise null. Buyer rejects an explicit client id, so do not validate the caller's argument instead.
4. Add `IBasketPriceClientResolver : IPriceClientResolver` for the existing concrete resolvers. It retains the code supplied by the coordinator and does not revalidate it. Keep `IPriceService` unchanged.
5. Extend `BasketDiscountCodeCoordinator.ResolveAsync` with validation and localized rejection callbacks. Read the persisted code through one lazy task, at most once, including explicit nonblank codes so they can be compared with storage. Keep `DiscountCodeResolver` and its current tests unchanged.

| Situation | Outcome |
|-----------|---------|
| Grula off | Preserve stored code, ignore request, no validation |
| Explicit removal (null), or no resolved code | Continue with null, no validation |
| Resolved code matches nonblank stored code after trim/case comparison | Continue with stored spelling, no validation |
| New/replacement code on an empty basket | Reject with existing empty-basket message before validation |
| New/replacement code valid | Continue with canonical spelling |
| New/replacement code inapplicable | Reject with buyer generic/seller specific message; basket unsaved |
| New/replacement code unverifiable | Reject with retry message; basket unsaved |

Before calling the unchanged resolver, normalize a matching explicit request to the stored spelling. This preserves the "emptying a discounted basket" rule even when the frontend changes letter case. A blank request continues meaning "unchanged"; null explicitly removes.

Do not add `RemovedDiscountCode`, response removal fields or stale-code notices.

### Phase 4 - Seller.Web admin backend

Mirror the ClientRole slice in `Project/Web/Seller/Seller.Web/Areas/Clients`.

1. Add `Shared/Filters/RequireGrulaAttribute`, a resource filter returning 404 with Grula off. Apply to all three new controllers; authorization uses existing `SellerOnly` routes.
2. Add discount code domain/request/response models and repository: paged list, detail, save, delete, validation and paged client options. URL-encode code/version values. Preserve HTTP failures instead of converting them to "not found".
3. Add `DiscountCodesApiController` (paged Get, POST Index, Delete), `DiscountCodesController.Index` and `DiscountCodeController.Edit`; matching view models, form/page/catalog builders and views. Prerender module names: `DiscountCodesPage` and `DiscountCodePage`.
4. List columns: code, description, client count, active, last modified, created. Follow the existing activity-tag convention.
5. Form builder gets selectable clients from the **new seller-scoped options endpoint**, following all pages in stable order. Do not call `IClientsRepository.GetAllClientsAsync`. Fail the page load on failed, malformed or incomplete option data; never allow a load failure to render empty assignments that a save would clear. Carry assigned ids and version.
6. Add localized menu/drawer entries conditionally when `IsGrulaConfigured`; inject settings into `DrawerMenuModelBuilder`. Add conditional entries after existing collection initializers.
7. Register repository and the four builders in `Areas/Clients/DependencyInjection/CompositionRoot`.

### Phase 5 - Seller.Portal admin frontend

1. Add `fe/projects/Seller.Portal/src/areas/Clients/pages/DiscountCodesPage/` and `DiscountCodePage/`, modeled on ClientRoles pages. The shared `Catalog` currently builds delete parameters as `{ id }`; add an optional `getDeleteParameters(item)` prop with that existing default, and supply `{ id, version }` from the new discount-code page. Forward both parameters through Seller.Web's Delete action to Client.Api. Keep the row on 409 and show the server reload message.
2. Add `components/DiscountCodeForm/DiscountCodeForm.js`:
   - required code max 64; optional description max 256; active switch backed by `isDisabled`;
   - MUI multiple Autocomplete using scoped client options, searchable by name;
   - always submit `clientIds`, including empty array, and the current version for edits;
   - show the no-client hint and Grula independence/no-effect hint;
   - show server errors; retain the user's draft on conflict and request reload. Update id/version from successful save.
3. Register pages in `webpack.config.js` (`discountcodepage`, `discountcodespage`) and `server/middleware/renderer.js`.

### Phase 6 - Seller.Web enforcement

1. Add `ClientApiDiscountCodeLookup` around repository validation; map status strings strictly. Register lookup and scoped validator in `Shared/DependencyInjection/CompositionRoot`.
2. Register `ClientRepositoryPriceClientResolver` as itself and `IBasketPriceClientResolver` as an alias to it. Register `IPriceClientResolver` via a factory building the decorator around that concrete resolver. Do not inject the decorated interface into its own decorator.
3. `BasketsApiController.Index` and `OrderFileApiController.Index` pass validation using `model.ClientId` to the coordinator, with specific seller messages. For new codes, missing client is rejected. Their repricing uses `IBasketPriceClientResolver`; the coordinator's accepted/stored text is authoritative.
4. Product API methods retain `IPriceClientResolver`, covering query-string codes. Checkout is unchanged.

### Phase 7 - Buyer.Web enforcement and team-member identity

1. Add validation-only discount repository, lookup and scoped validator.
2. Register `ClaimsPriceClientResolver` as itself and the basket alias; register decorated `IPriceClientResolver` as in Phase 6.
3. Save/upload controllers validate new/replacement codes using `User.GetClientId()`. Use one generic buyer business rejection and a separate unavailable/retry message. Basket repricing uses `IBasketPriceClientResolver`, with no explicit client id (Buyer resolver derives identity from claims).
4. In `Shared/Middlewares/ClaimsEnrichmentMiddleware`, when email lookup finds no client, use `IClientsRepository.GetClientAsync(token, language)` for organisation lookup (C7/C12). Treat a failed fallback as no client and cache nothing; distinguish/log failure without credentials.
5. On a successful uncached lookup, add `ClientId` to the current principal as well as the cache. Test cold and warm paths for primary contacts and team members. Scope the cache key to organisation plus email and change its prefix so old email-only entries are not reused.
6. Keep product APIs and pricing builders on decorated `IPriceClientResolver`. Leave stored basket reads, order-form code display and checkout unchanged.
7. Never cache assigned codes in claims: fresh validation must observe assignment/activity changes.

### Phase 8 - Order form feedback and final verification

Existing error handling should suffice; no removal notifier is needed. Verify server `message` is toasted by Seller `OrderForm.js`, Buyer `NewOrderForm.js` and basket/upload helpers for rejected new codes. Verify stored codes keep their applied state and edits/uploads/checkout do not show revocation errors.

## 8. Validation matrix

| Scenario | Buyer.Web | Seller.Web |
|----------|-----------|------------|
| New active assigned code | Save canonical spelling, reprice with code | Same |
| New code in different case/with surrounding spaces | Accepted after normalization | Same |
| New unknown/inactive/unassigned/zero-client code | 400 generic message, basket unsaved | 400 specific message, basket unsaved |
| New code, no client | 400 generic message | 400 select-client message |
| New code on empty basket | 400 existing empty-basket rule | Same |
| Explicit removal | Null, no validation, reprice without code | Same |
| Omitted/blank code or matching explicit stored code | Preserve stored spelling without validation | Same |
| Stored code revoked, deleted, renamed or assignment changed; edit/upload | Keep stored code, reprice using it, no revocation notice | Same |
| Stored code, Client.Api discount validation unavailable | No discount validation needed; existing other dependencies still apply | Same |
| Stored code; checkout | Existing stored code/prices proceed without validation | Same |
| Different valid code replaces a stored code | Validate replacement and store canonical spelling | Same |
| Invalid/unverifiable replacement | Reject, keep existing basket intact | Same |
| Fresh price query/build with invalid or unverifiable code | Omit code from Grula request | Same for Seller price APIs |
| Stored basket/order-form totals | Existing saved totals; no revalidation/repricing on read | Existing behaviour |
| Seller UI selects another client | Not applicable | Existing UI starts a new empty basket and resets the code |
| Direct save reuses a basket for another client with unchanged text | Retain stored code under C9 | Same; cross-client stored prices accepted under C10 |
| Team member of assigned client newly applies code | Accepted on first and subsequent requests | Not applicable |
| New-code validation unavailable | 400 retry message, no save | Same |
| Grula configured but unreachable, applicable new code | Code can be stored; prices cleared as today | Same |
| Grula off | Hidden field, no validation, preserve stored code | Same; hidden menu, admin web URLs 404 |
| Rename/delete then reuse text | Fresh application validates current code; old baskets continue independently | Same |
| Direct Basket.Api call | Existing API behaviour outside web enforcement | Same |

Fresh product prices can differ from stored basket prices after revocation. This is an intentional consequence of C9/C10, not a failed implementation.

## 9. Resource keys

Add `ClientResources` translations in `en`, `pl`, `de` under `Infrastructure/Localization/Foundation.Localization/Resources`.

| Key | English text |
|-----|--------------|
| DiscountCodes | Discount codes |
| NewDiscountCode | New discount code |
| EditDiscountCode | Edit discount code |
| DiscountCodeDescriptionLabel | Description |
| DiscountCodeClientsLabel | Clients |
| DiscountCodeGrulaHint | Must match the "Discount Code" driver item in Grula exactly. Giuru does not verify its Grula configuration; applying a code may leave prices unchanged |
| DiscountCodeNoClientsHint | No client can newly apply this code until at least one is assigned |
| NavigateToDiscountCodesText | Back to discount codes |
| DiscountCodeRequiredErrorMessage | Enter a discount code |
| DiscountCodeSavedSuccessfully | Discount code saved |
| DiscountCodeDeletedSuccessfully | Discount code deleted |
| DiscountCodeNotFound | Discount code not found |
| DiscountCodeExists | A discount code with this value already exists |
| DiscountCodeClientsInvalid | One or more selected clients are unavailable for assignment |
| DiscountCodeChanged | This discount code changed since you opened it. Reload before saving or deleting |

Add `OrderResources` translations in the same cultures:

| Key | Audience | English text |
|-----|----------|--------------|
| DiscountCodeInvalid | Buyer; seller unknown code | This discount code cannot be applied |
| DiscountCodeDisabled | Seller | This discount code is inactive |
| DiscountCodeNotAssignedToClient | Seller | This discount code is not assigned to the selected client |
| DiscountCodeClientRequired | Seller | Select a client before applying a discount code |
| DiscountCodeCouldNotBeVerified | Both | The discount code could not be verified. Try again |

Reuse existing `DiscountCodeLabel`, applied/empty-basket messages, active/inactive/date labels. Do not add stale-code removal or checkout-refusal resources.

## 10. Tests and checks

### Unit tests

Use `Tests/Giuru.UnitTests` (xUnit + NSubstitute).

- Validator: missing/empty client and invalid text avoid lookup; failures/malformed payload become unavailable; repeated/concurrent same client/code/token share one call; different identity/token does not.
- Product resolver decorator: valid canonical code, invalid/unavailable omitted; no validation with Grula off/no code; inner called with null code; validate inner client's id.
- Coordinator: every row of Phase 3; unchanged stored code, including case/whitespace variants, never validates; persisted read at most once; explicit removal never validates; invalid replacement does not save; empty-basket precedence.
- Both concrete basket resolver registrations preserve coordinator-supplied codes without lookup. DI with scope validation resolves the product decorator and basket alias correctly.
- Lookup adapters: strict status mapping, malformed canonical value, HTTP failure and code URL encoding.
- Four save/upload controller flows: new code uses correct client; stored code survives revocation and unavailable discount lookup; repricing receives stored spelling; rejected new code leaves basket unsaved; buyer generic vs seller-specific messages.
- Claims middleware: email contact and organisation team member, uncached and cached; first request already has `ClientId`; failed/no fallback caches nothing; organisation-specific cache keys.
- `RequireGrulaAttribute`: off returns 404; on passes through.

Update `BasketsApiControllerFlowTests`, `OrderFileApiControllerFlowTests` and `PriceClientResolverTests` for new contracts/DI. Keep `DiscountCodeResolverTests` unchanged and green. Checkout tests retain existing behaviour; no new discount-validation constructor dependencies.

### Integration tests

Use `Tests/Giuru.IntegrationTests`. CI workflows exclude `IntegrationTests`; run locally. Fixture uses fixed container names/ports, so run once at a time. Budget for image builds and startup.

- Fixture Grula is configured but unreachable. Use this for application rules; verify outgoing product driver omission with decorator tests.
- Extend `Giuru.MockAuth`/fixture to issue seller A/B, buyer A/B and a team-member token (same organisation as buyer A, distinct email, no Seller role). Configure authentication for both bearer calls and web test principals.
- Expose an authenticated Client.Api test client for management/options/validation scenarios.
- Real SQL Server: create/rename duplicates including concurrent writes; case matching; active/disabled/delete/recreate; none/one/many assignments; invalid/foreign client leaves code and links unchanged; deletion cleans links; stale admin version gives 409 without partial assignment writes; second seller cannot see/edit/assign first seller's data; options never contain foreign clients; buyer CRUD/options gets 403.
- Buyer validation uses organisation, including team members, and returns generic non-valid statuses. Seller validation checks ownership.
- Web saves/uploads: valid new code; invalid new/replacement rejected; retained code continues after deactivation/unassignment/deletion/rename or discount-lookup failure; explicit removal; old basket text survives recreation; product query validation; first-request team-member use.
- Stored-code checkout proceeds after revocation and during discount-validation failure, demonstrating C9; fresh product requests omit the revoked code.
- Rewrite `BasketDiscountCodeTests.SaveBuyerBasket_WithDiscountCode_WhenConfiguredGrulaIsUnavailable_StillStoresTheCode`: unregistered new code rejected; registered/assigned new code stored despite Grula outage.
- Direct Basket.Api tests retain their meaning and explicitly remain outside web enforcement.
- Use dedicated clients/codes/baskets per test; do not rely on shared totals or execution order.

### Frontend and manual verification

- Run the existing frontend test scripts; verify the optional Catalog delete adapter sends the version and retains the row on conflict, while existing catalogs still send only id. No standalone notifier test is needed because no notifier is added.
- Build Seller.Portal and AspNetCore bundles to scratch directories, preserving `wwwroot/dist`; verify the two new pages render through SSR. Use a temporary build configuration that also puts extracted CSS inside the scratch directory and disables the cleanup plugin. Changing only `--output-path` is insufficient: both checked-in configs use CSS filenames containing `../../dist/css` and allow cleanup outside the project.
- Grula off: hidden menu/discount fields, admin web URLs 404.
- Manage codes with zero/many clients; verify scoped options, version conflicts, duplicate/invalid-client messages.
- Apply as client A, unrelated client B and team member A. New codes follow assignment rules.
- Revoke a stored code: fresh product query omits it; basket edit/upload retains it and checkout proceeds.
- Confirm a Giuru code unknown to Grula applies with potentially unchanged prices, as the hint explains.

## 11. Deployment

No new Grula settings, flags, Basket.Api/Ordering.Api contracts or ingress changes. Shared infrastructure changes may trigger other existing CI/CD workflows; "no basket change" does not guarantee those workflows will skip redeployment.

Deploy Client.Api with startup migration, then Seller.Web/Buyer.Web with SSR bundles. Current production workflows are independent; their order is not automatically guaranteed. Verify Client.Api's new endpoints and migration before enabling operator creation/testing through web apps.

With a web app ahead of Client.Api, fresh application fails with retry feedback and fresh product pricing omits the code. Stored-code flows do not make validation calls. C4 means there is no deployed code data to seed.

Rollback of the web apps restores today's unrestricted free-text behaviour. New tables can remain.

## 12. Risks and accepted limitations

| Risk | Impact | Treatment |
|------|--------|-----------|
| Direct Basket.Api access | Bypasses web checks and can supply stored prices | Accepted C8; deployment access left unchanged |
| Stored code revoked or no longer assigned | Existing basket still uses it | Intentional C9; affects edits/uploads and checkout |
| Fresh product and stored basket prices differ after revocation | Different visible prices in one session | Intentional C9/C10; communicate this to operators |
| Cross-client prices, checkout body/cookie discrepancy, concurrent basket mutations | Existing price/identity inconsistencies can carry into orders | Accepted C10; no basket binding/version changes |
| Giuru code has no Grula effect | Applied with no monetary reduction | C5; explicit admin hint and manual verification |
| Case-only rename | Existing baskets retain older spelling and therefore older exact Grula driver value | Accepted C9/C11; remove/reapply to adopt the new spelling |
| Scoped options fetch many clients over multiple pages | Slow admin form at scale | Match existing form-loading pattern but scoped; use searched paging later if measured load requires it |
| Admin stale form | Could overwrite assignments | Existing RowVersion carried through update/delete; 409 on conflict |
| Model snapshot drift | Unintended existing-schema changes | Migration review and empty/existing-database checks |
| Buyer client claims cached 30 minutes | Membership/client-context changes may lag | Existing cache retained; organisation-aware key, no assigned-code cache |
| Organisation uniqueness not database-enforced | Lookup ambiguous if C12 stops holding | C12 is an explicit product invariant; multiple-client organisation support requires revisiting lookup |

## 13. Out of scope

- Grula code creation, existence checks or policy reads.
- Validity dates, usage limits, one-time codes and usage reporting.
- Assigning codes on the client form or to groups.
- Assigned-code dropdown on seller order form.
- Revoking codes already in baskets, automatic removal notices or checkout validation.
- Basket pricing-client binding, atomic basket-version checks and checkout repricing.
- Fixing direct-API access, Buyer checkout identity discrepancies or multiple-client organisation support.
- Basket.Api/Ordering.Api contract changes and edits to already placed orders.
