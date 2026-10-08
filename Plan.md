# Plan: managing discount codes and assigning them to clients (Grula)

Branch: `feature/Added-assigning-discount-codes-to-clients`

Reviewed against repository commit `d069e421f` and the locally installed Grula SDK 1.0.3 on 2026-10-05. Repository paths below are relative to `be/src` unless they start with `fe/`. Live Grula behaviour remains to be verified in Phase 0; SDK reflection proves the available contracts, not server matching or permissions.

## 1. Goal

1. Sellers manage discount codes in the admin panel (Seller.Web / Seller.Portal). The feature is visible and reachable only when Grula is enabled (`AppSettings.IsGrulaConfigured`).
2. Sellers assign discount codes to clients.
3. A discount code is used for pricing only when it **exists** and **can be applied for that client**. This holds on every path that accepts a code, in both the buyer and the seller application.

## 2. What exists today (verified in the code)

- A discount code is a free-text string (max 64 characters) stored on the Redis basket by `Basket.Api` and copied to the order at checkout (`BasketService.CheckoutAsync`, `Order.DiscountCode`).
- It reaches Grula as the price driver `Discount Code` (`PriceDriversConstants.DiscountCodeDriver`, added in `PriceService.CreatePriceDrivers`).
- Length and basket-input semantics are validated, but existence and client assignment are not. Any code within those input rules can be stored and sent to Grula.
- All current web pricing paths that accept a discount code build their client through `IPriceClientResolver.ResolveAsync(clientId, discountCode, token)`:
  - Buyer: `ClaimsPriceClientResolver` (client from claims).
  - Seller: `ClientRepositoryPriceClientResolver` (client looked up by id).
- Every basket write resolves the code through `BasketDiscountCodeCoordinator.ResolveAsync` (`Foundation.Pricing/Baskets`).
- Checkout (`BasketCheckoutApiController` in both apps) neither reprices nor rechecks the code. It places the order with whatever the basket holds.
- Buyer checkout passes request-body `model.ClientId` to `CheckoutBasketAsync`, although pricing uses `User.GetClientId()`. It also stock-checks `model.BasketId` but checks out the cookie basket. Both identities must be made consistent before claiming checkout enforcement.
- Baskets carry no client identity today. Revalidating a code for client B alone does not prove that persisted prices were calculated for B: a seller could price for A and then check out for B when both are assigned that code.
- `Client.Api` owns clients and their many-to-many links (`ClientsGroup`, `ClientsAccountManagers`). It has no Grula configuration; only Buyer.Web and Seller.Web do.
- Grula SDK 1.0.3 (the pinned version) exposes more than prices. Reflection over the assembly shows `GetDriversAsync(environmentId, pageIndex, itemsPerPage)`, `GetDriverItemsAsync(driverId, pageIndex, itemsPerPage, searchTerm)`, `CreateDriverItemAsync`, `UpdateDriverItemAsync`, and price adjustment policy endpoints. There is no delete for driver items. The client is not sealed and the methods this plan uses are `virtual`, so they can be substituted in unit tests.
- Integration tests run with Grula **configured but unreachable** (`ApiFixture.UnreachableGrulaUrl`) and a single mock token (`seller@user.com`, role `Seller`) for both web apps.
- `ClientRolesService` and the unfiltered `ClientsService.Get` list do not filter by seller. They are structural examples only; copying their queries would expose the new codes or assignments across sellers. `ClientsController.Save` currently requires authentication but no seller role.
- `be/src/docker-compose.override.yml` publishes Basket.Api on host port 5103. Production exposure is unknown; web-only enforcement requires a verified service boundary (section 8).

### Every place a discount code enters the system

| # | App | Entry point | Where the code comes from |
|---|-----|-------------|---------------------------|
| 1 | Buyer | `Orders/BasketsApiController.Index` | request body, or the stored basket when omitted |
| 2 | Buyer | `Orders/OrderFileApiController.Index` | multipart form field, or the stored basket |
| 3 | Buyer | `Orders/BasketCheckoutApiController.Checkout` | stored basket |
| 4 | Buyer | `Products/ProductsApiController` (`GetPrice`, `GetProductVariants`, `GetProductsQuantities`), `AvailableProductsApiController.Get`, `OutletApiController.Get`, `SearchProductsApiController.Get` | **query string** |
| 5 | Buyer | SSR builders: `AvailableProductsCatalogModelBuilder`, `OutletCatalogModelBuilder`, `SearchProductsCatalogModelBuilder`, `ProductDetailModelBuilder` | stored basket |
| 6 | Seller | `Orders/BasketsApiController.Index` | request body + `model.ClientId` |
| 7 | Seller | `Orders/OrderFileApiController.Index` | form field + `model.ClientId` |
| 8 | Seller | `Orders/BasketCheckoutApiController.Checkout` | stored basket + `model.ClientId` |
| 9 | Seller | `Products/ProductsApiController` (`GetProductsQuantities`, `GetPrice`) | query string + `clientId` |

Rows 4 and 9 matter most: a code passed in the query string is priced today without any check.

## 3. Target design

### 3.1 Ownership

| Concern | Owner | Why |
|---------|-------|-----|
| Which codes exist for the shop, and which clients may use them | `Client.Api` (new tables) | It already owns clients and their link tables, both web apps already call it, and the integration fixture already runs it |
| What a code is worth | Grula (price adjustment policies) | Unchanged. Giuru never stores amounts |
| "Does this code exist in Grula?" | Seller.Web, checked when a seller creates a code | Only the web apps have Grula credentials. Checking at management time keeps Grula out of the per-request verification path |

### 3.2 The applicability rule

A code can be applied for a client when all of these hold:

1. A `DiscountCode` row exists for the client's seller with that code (case-insensitive match) and `IsActive`.
2. It is not disabled (`IsDisabled == false`).
3. A `ClientsDiscountCode` row links it to the client.

`Client.Api` answers this through one endpoint and returns the **canonical** spelling of the code only for a valid result. That canonical value is what gets stored on the basket and sent to Grula, so `summer25` typed by a user prices the same as `SUMMER25`. Buyer responses collapse all inapplicable reasons into `NotApplicable`; detailed statuses are seller-only.

### 3.3 Enforcement at two seams

- **Pricing seam.** A decorator around `IPriceClientResolver` verifies the code and replaces `PriceClient.DiscountCode` with the canonical value, or with `null` when the code is not applicable or verification is unavailable. This covers current web pricing paths. Future paths must also use the decorated resolver; `IPriceService` itself accepts a `PriceClient` and is not an enforcement boundary.
- **Basket write seam (user feedback).** `BasketDiscountCodeCoordinator` decides what the user sees:
  - a newly applied code that is not applicable is **rejected** (HTTP 400, basket untouched);
  - a code already stored on the basket that has since become inapplicable is **removed**, the basket is repriced without it, and the response tells the user;
  - if applicability cannot be determined (Client.Api unreachable), the write is **rejected**. Failing closed avoids both pricing with an unverified code and silently raising a customer's prices because of a transient error.
- **Checkout.** The stored code is rechecked against the client the order is placed for. If it is no longer applicable, checkout returns 409 and the user must remove the code, which reprices the basket through the normal write path. An unavailable verification returns 503 without changing the basket. A discounted basket must also carry the server-stored client id from its last verified save; a missing or different id returns 409 and requires a save for the checkout client. Buyer checkout derives that client from the authenticated principal and uses the cookie basket for every check and for checkout.

Both seams call the same request-scoped validator, which remembers its answer for the request, so a basket save costs one extra Client.Api call, not two. Enforcement is enabled only when both `IsGrulaConfigured` and the new `DiscountCodeEnforcementEnabled` setting are true. All seams, checkout guards and optional load warnings use that same condition; management visibility depends on `IsGrulaConfigured` alone.

## 4. Decisions assumed by this plan

These are the defaults the steps below implement. Each one is isolated enough to change without restructuring the plan.

| # | Decision | Default in this plan | Alternative and where it would plug in |
|---|----------|----------------------|----------------------------------------|
| D1 | Code missing in Grula when a seller creates it | Refuse to save, with a clear message. The code text also takes Grula's spelling | Create the driver item automatically with `CreateDriverItemAsync` (step 2.2). It would still have no price effect until a price adjustment policy is defined in Grula |
| D2 | Who may use a code | Only clients it is assigned to | Add `IsAvailableToAllClients` to `DiscountCode` and one `OR` in the validation query (step 1.5) |
| D3 | Where assignment is edited | On the client form, as a multi-select next to client groups | Also a clients multi-select on the discount code form (deferred step 4.5) |
| D4 | Deleting an assigned code | Deactivates the code and its assignments together | Block with a 409 like `GroupDeleteClientConflict` |
| D5 | Validity dates | Not stored in Giuru. Grula's policies already carry `ValidFrom`/`ValidTo` and receive the pricing date | Add `ValidFrom`/`ValidTo` columns and two conditions in step 1.5 |
| D6 | Buyer-facing error text | One generic message for unknown, disabled and unassigned codes, so a buyer cannot probe which codes exist | Sellers get the specific reason, because they can fix it |

## 5. Implementation steps

Phases 0 to 4 deliver management and assignment without changing how orders are priced. Phases 5 to 8 turn on enforcement. They can ship as two releases (see section 8).

### Phase 0 - Grula spike (no committed code)

The SDK surface is known, but three behaviours are not visible from the assembly and decide how step 2.2 is written.

- **0.1** With the real token and environment id, call `GetDriversAsync` and confirm:
  - the token is authorised for driver endpoints (not only price queries);
  - a driver named exactly `Discount Code` exists;
  - whether `pageIndex` starts at 0 or 1.
- **0.2** Call `GetDriverItemsAsync` for that driver and confirm:
  - which field (`Name` or `Value`) holds the string the price API matches. Compare against a code known to produce a discount;
  - whether `searchTerm` is a prefix or contains match, and whether it is case-sensitive.
- **0.3** Record the three answers in the PR description. If the token cannot read drivers, stop and resolve that with Grula before Phase 2, because D1 depends on it.

### Phase 1 - Client.Api: data model, CRUD, assignment, validation

Use the `ClientRoles` slice as the structural template (flat entity, no translations), adding seller filtering explicitly.

- **1.1 Entities** in `Infrastructure/DiscountCodes/Entities/`:

  ```csharp
  public class DiscountCode : Entity
  {
      [Required, MaxLength(64)]
      public string Code { get; set; }

      [MaxLength(256)]
      public string Description { get; set; }

      public bool IsDisabled { get; set; }

      [Required]
      public Guid SellerId { get; set; }
  }

  public class ClientsDiscountCode : Entity
  {
      [Required]
      public Guid ClientId { get; set; }

      [Required]
      public Guid DiscountCodeId { get; set; }
  }
  ```

  64 matches the existing limit in `UpdateBasketModelValidator`, `CheckoutBasketServiceModelValidator` and `OrderingContext`.

- **1.2 `ClientContext`**: add `DbSet<DiscountCode> DiscountCodes` and `DbSet<ClientsDiscountCode> ClientsDiscountCodes`. Add `OnModelCreating` (the context has none today) with:
  - a unique index on `(SellerId, Code)` filtered to `[IsActive] = 1`, so a deleted code can be created again;
  - a unique index on `ClientsDiscountCodes (ClientId, DiscountCodeId)` filtered to `[IsActive] = 1`, preventing duplicate active assignments;
  - foreign keys from the link table to `Clients` and `DiscountCodes`, with restrictive delete behaviour (deletion is soft);
  - an explicit case-insensitive, accent-sensitive collation on `Code` (for example `Latin1_General_100_CI_AS`), so matching does not depend on the database default. Do not uppercase the query column; let the indexed collation perform the comparison;
  - call `base.OnModelCreating(modelBuilder)`.

- **1.3 Migration**: with the .NET SDK selected by `global.json` (10.0.203) and an EF tool compatible with the repository's EF Core 10 packages, run `dotnet ef migrations add AddedDiscountCodes --output-dir Infrastructure/Migrations` in `Project/Services/Client/Client.Api`. Supply the normal local design-time configuration if startup needs it. It is applied on startup by `ConfigureDatabaseMigrations`. Review the migration and snapshot: only the two new tables, their indexes and foreign keys should change; do not alter existing tables or database-wide collation.

- **1.4 Service** `Services/DiscountCodes/IDiscountCodesService` + `DiscountCodesService`, with service models in `ServicesModels/DiscountCodes/` and FluentValidation validators in `Validators/DiscountCodes/`:
  - `CreateAsync`: trim the code; reject blank or longer than 64 and descriptions longer than 256; require a valid organisation claim; 409 `DiscountCodeExists` when an active code with the same text exists for the seller; set `SellerId` from the caller's organisation claim, never from the request. Translate only the relevant SQL unique-index violation to the same 409, so concurrent creates do not produce a 500. Use `FillCommonProperties` when creating entities and update `LastModifiedDate` on edits/deactivation.
  - `UpdateAsync`: only `Description` and `IsDisabled` change. The code text is immutable after creation (to rename, create a new code). 404 `DiscountCodeNotFound` when not found for this seller.
  - `DeleteAsync`: set `IsActive = false` on the code and on its `ClientsDiscountCodes` rows in one `SaveChangesAsync` (D4).
  - `GetAsync(id)`, `GetAsync(paged)` with `searchTerm` as `Code.StartsWith`, `ApplySort`, and the same paging rules as `ClientRolesService`. Every query filters by `SellerId == OrganisationId`.

- **1.5 Validation** `ValidateAsync(code, clientId, caller)` returning `{ IsValid, DiscountCode, Status }` where `Status` is `Valid | NotFound | Disabled | NotAssigned | ClientUnknown`:
  1. Require a valid organisation claim and load the active client by id. If the caller has the `Seller` role, require `client.SellerId == organisation claim`. Otherwise require both `client.OrganisationId == organisation claim` and `client.Email == authenticated email`, matching the buyer middleware's email-based client lookup. Organisation equality alone does not identify one client. Missing claims or an unauthorised client produce `ClientUnknown` internally, before querying the code. A buyer can therefore only ask about their own client.
  2. Load the active code where `SellerId == client.SellerId` and `Code == code` → else `NotFound`.
  3. `IsDisabled` → `Disabled`.
  4. No active link row → `NotAssigned`.
  5. Otherwise `Valid`, with `DiscountCode` set to the stored spelling. At the HTTP boundary, expose the specific inapplicable status only to sellers; buyers receive `NotApplicable` and no code or assignment metadata.

- **1.6 Controller** `v1/Controllers/DiscountCodesController` (`/api/v1/discountcodes`), request and response models in `v1/RequestModels` and `v1/ResponseModels`:
  - `GET` (paged list), `GET {id}`, `POST` (create, or update when `Id` is set), `DELETE {id}`: require the `Seller` role. No other Client.Api controller restricts by role today, but these endpoints decide who gets a discount.
  - `GET validation?code=&clientId=`: any authenticated caller. Always 200 for a well-formed request, because "not applicable" is an answer, not an error. Trim and require a nonblank code of at most 64 characters and a nonempty client Guid; 422 for invalid input. Use a literal `validation` route and `{id:guid}` for the item route. Configure model-binding failures consistently if promising 422 rather than the `[ApiController]` automatic 400.
  - Verify actual JWT role mapping with the mock issuer and the production identity service before relying on `User.IsInRole` / role attributes. `RegisterApiAccountDependencies` uses `ClaimTypes.Role`, while the mock issuer emits `role`; mapped claims must agree. Test both authorised sellers and authenticated non-sellers through HTTP.

- **1.7 Assignment through the client**:
  - `ClientRequestModel`, `CreateClientServiceModel`, `UpdateClientServiceModel`: add `IEnumerable<Guid> DiscountCodeIds`.
  - `ClientServiceModel`, `ClientResponseModel`: add `DiscountCodeIds` for seller reads. Populate the seller list, `GetAsync` and `GetByIds` paths and their response mappings; do not include assignment lists in buyer `email` / `organisation` responses. Add `SellerId == model.OrganisationId` to `ClientsService.Get` before adding assignment data to that currently unfiltered list.
  - `ClientsService.CreateAsync` / `UpdateAsync`: replace the client's link rows the same way groups and managers are replaced, with two differences:
    - **`null` means "leave unchanged"; an empty list means "clear".** When Grula is off the client form does not send the field, and that must not wipe assignments.
    - Require the seller role whenever `DiscountCodeIds` is non-null, including an empty list, in `ClientsController.Save` before mapping to either service call. Reject non-sellers with 403; protecting only `DiscountCodesController` would leave client-save assignment changes open. Carry trusted caller authorisation into the assignment service, never a body-supplied flag.
    - Validate and de-duplicate all ids before changing tracked client fields or links; reject ids that are not active codes of the caller's seller (422), so a crafted request cannot attach another seller's code. Save client and assignment changes atomically. A null list on create creates no assignments. Deleted codes must not appear in the selected ids or choices on later client reads.

- **1.8 Wiring**: register `IDiscountCodesService` in `CompositionRoot.RegisterClientApiDependencies`. Add `DiscountCodesApiEndpoint` and `DiscountCodesValidationApiEndpoint` to `ApiConstants.Client`.

- **1.9 Resources**: add the `ClientResources` keys from section 6 in `en`, `pl`, `de`.

**Done when:** the endpoints work from Swagger; a second seller cannot see or attach the first seller's codes; updating a client without `DiscountCodeIds` leaves its assignments intact.

### Phase 2 - Foundation.Pricing: shared contracts

New folder `Foundation.Pricing/DiscountCodes/`.

- **2.1 Validation contracts**

  ```csharp
  public enum DiscountCodeValidationStatus
  {
      Valid, NotFound, Disabled, NotAssigned, ClientUnknown, NotApplicable, Unavailable
  }

  public sealed class DiscountCodeValidation
  {
      public DiscountCodeValidationStatus Status { get; init; }
      public string DiscountCode { get; init; }   // canonical spelling when Valid
      public bool IsValid => Status is DiscountCodeValidationStatus.Valid;
  }

  // Port implemented by each web app: one HTTP call to Client.Api.
  // Use an explicit string status in the JSON response, mapped to this enum;
  // Client.Api must not reference Foundation.Pricing to share an HTTP DTO.
  public interface IDiscountCodeLookup
  {
      Task<DiscountCodeValidation> ValidateAsync(Guid clientId, string discountCode, string token,
          CancellationToken cancellationToken = default);
  }

  // Shared policy consumed by the coordinator callers, the resolver decorator and checkout.
  public interface IDiscountCodeValidator
  {
      Task<DiscountCodeValidation> ValidateAsync(Guid? clientId, string discountCode, string token,
          CancellationToken cancellationToken = default);
  }
  ```

  `DiscountCodeValidator` (scoped) implements the policy once for both apps:
  - blank code → `NotFound` without calling the lookup (callers skip validation when there is no code);
  - `clientId` is null or empty → `ClientUnknown` without calling the lookup;
  - trim the code before lookup and caching; reject longer-than-64 input as inapplicable without an HTTP call;
  - an expected transport failure or timeout from the lookup → `Unavailable`, logged without the token. A non-success HTTP response, empty/malformed response, unknown status, or `Valid` without a nonblank canonical code of at most 64 characters must also fail closed. Forward the optional cancellation token in the new lookup/repository APIs; controller callers pass `HttpContext.RequestAborted`. A caller cancellation must propagate rather than become an applicability result;
  - remembers the in-flight **policy-result task** per token, client id and trimmed code for the request, with a lock as in `ClientLookupService` so concurrent calls share one lookup. Compare codes with `OrdinalIgnoreCase`: the coordinator asks with user spelling and the decorator then asks with canonical spelling. Cache `Unavailable` for the remainder of this request too, so callers see a consistent decision; the next HTTP request gets a fresh validator. This differs from `ClientLookupService`'s faulted raw-task retry policy.

- **2.2 Grula existence check** `IGrulaDiscountCodeService` / `GrulaDiscountCodeService` (`GrulaApiClient`, `IPricingSettings`, logger):
  - `FindAsync(code, cancellationToken)` returns `Exists(canonicalCode) | Missing | Unavailable | NotConfigured`; the controller passes `HttpContext.RequestAborted`.
  - Find the driver named `PriceDriversConstants.DiscountCodeDriver` by paging `GetDriversAsync` until `HasNextPage` is false, using `Items` from the SDK's paged responses. Cache only a successful driver lookup for a bounded interval in a separate shared cache, keyed by API URL and environment id; do not make a service holding the typed HTTP client a singleton. Invalidate a stale driver id on a not-found response. Do not cache `Missing` or `Unavailable` indefinitely.
  - Page `GetDriverItemsAsync(driverId, …)` and compare exactly, ignoring case, on the field identified in step 0.2. Use `searchTerm` only if the spike proves it cannot exclude case variants or search by the wrong field; otherwise enumerate without a filter. Ambiguous case-insensitive matches are unusable configuration: log the reason and return `Unavailable` instead of arbitrarily choosing one. Revalidate the canonical string's 64-character limit before saving.
  - `ApiException`, `HttpRequestException`, timeout cancellation, or unusable responses → `Unavailable`; caller cancellation propagates. The existing 10 second timeout is per HTTP call, so also bound the total paged lookup and stop on non-advancing or inconsistent pages. `NotConfigured` must be handled explicitly and must never fall through to saving a code.

- **2.3 Wiring and rollout setting**: register `IGrulaDiscountCodeService` with a lifetime compatible with the typed HTTP client in `PricingCompositionRoot.RegisterPricingDependencies`. Register `IDiscountCodeValidator` as scoped only in Phases 6/7, together with each app's `IDiscountCodeLookup`; otherwise Release 1 has an unresolved constructor dependency. Add `DiscountCodeEnforcementEnabled` (default false) to `IPricingSettings` and both web `AppSettings` classes and bind/deploy it consistently. Update test settings implementations. This setting controls enforcement, not management visibility.

- **2.4 Unit tests**: see section 7.

### Phase 3 - Seller.Web: admin panel backend

Mirror the `ClientRole` slice in `Areas/Clients`.

- **3.1 Feature gate and authorisation**: a small resource filter in `Seller.Web/Shared` (for example `[RequireGrula]`) that returns 404 when `IOptions<AppSettings>.Value.IsGrulaConfigured` is false. Apply it and `[Authorize(Policy = "SellerOnly")]` to the three new controllers. The policy already exists in Seller.Web; hiding the menu does not protect typed URLs or API requests.

- **3.2 Repository** `Areas/Clients/Repositories/DiscountCodes/IDiscountCodesRepository` + `DiscountCodesRepository`: paged `GetAsync`, all-pages `GetAsync` (as `ClientGroupsRepository` does), `GetAsync(id)`, `SaveAsync`, `DeleteAsync`, `ValidateAsync`. Domain model `DomainModels/DiscountCode` (`Id`, `Code`, `Description`, `IsDisabled`, dates).

- **3.3 API controller** `ApiControllers/DiscountCodesApiController` (`Get`, `Index` POST, `Delete`). In `Index`, when creating:
  1. `IGrulaDiscountCodeService.FindAsync(model.Code)`.
  2. `Missing` → 422 with `DiscountCodeNotFoundInGrula`.
  3. `Unavailable` → 503 with `DiscountCodeGrulaUnavailable`. Nothing is saved.
  4. `Exists` → save with Grula's canonical spelling.
  5. `NotConfigured` → 404; no save (defence if configuration changes after the resource filter).

  Updates do not call Grula, because the code text cannot change.

- **3.4 Pages**: `Controllers/DiscountCodesController` (`Index`), `Controllers/DiscountCodeController` (`Edit`), view models (`DiscountCodesPageViewModel`, `DiscountCodePageViewModel`, `DiscountCodeFormViewModel`), model builders (`DiscountCodesPageCatalogModelBuilder`, `DiscountCodesPageModelBuilder`, `DiscountCodePageModelBuilder`, `DiscountCodeFormModelBuilder`), views `Views/DiscountCodes/Index.cshtml` and `Views/DiscountCode/Edit.cshtml` with `asp-prerender-module="DiscountCodesPage"` / `"DiscountCodePage"`.
  - Catalog columns: `code`, `description`, `isDisabled` with `IsActivityTag = true` (as `ClientsPageCatalogModelBuilder` does), `lastModifiedDate`, `createdDate`.

- **3.5 Menu**: add a "Discount codes" entry to `MenuTilesModelBuilder` and `DrawerMenuModelBuilder` (clients group), only when `IsGrulaConfigured`. `MenuTilesModelBuilder` already has `IOptionsMonitor<AppSettings>`; `DrawerMenuModelBuilder` needs it injected.

- **3.6 Client form**:
  - `ClientFormViewModel`: `IsDiscountCodeEnabled`, `DiscountCodes` (id + label), `DiscountCodeIds`, `DiscountCodesLabel`, `NoDiscountCodesText`.
  - `ClientFormModelBuilder`: inject `IOptions<AppSettings>` and load all pages of codes only when `IsGrulaConfigured`. Show disabled codes too, marked as inactive, so an existing assignment stays visible. On a load error, fail the form load or disable assignment saving with feedback; never turn an unavailable list into an empty selection that clears assignments.
  - `Client` domain model, `SaveClientRequestModel`, `IClientsRepository.SaveAsync`, `ClientsRepository`, `ClientsApiController.Index`: carry `DiscountCodeIds`. Pass `null` when Grula is off, even if a crafted browser request supplies ids. Require `SellerOnly` for assignment-changing requests before any organisation/client side effects.

- **3.7 DI**: register the repository and the four model builders in `Areas/Clients/DependencyInjection/CompositionRoot`.

**Done when:** with Grula off, the menu entry is absent, the URLs return 404, and saving a client keeps its assignments.

### Phase 4 - Seller.Portal: admin panel frontend

- **4.1** `src/areas/Clients/pages/DiscountCodesPage/` (`DiscountCodesPage.js`, `.scss`, `index.js`): copy of `ClientGroupsPage` using the shared `Catalog`.
- **4.2** `src/areas/Clients/pages/DiscountCodePage/` and `src/areas/Clients/components/DiscountCodeForm/DiscountCodeForm.js`, modelled on `ClientGroupForm`:
  - `code`: required, max 64, read-only once the record has an id;
  - `description`: optional, max 256;
  - active switch, as `ClientForm` does for `isDisabled`;
  - show the server message on failure, so the Grula messages from step 3.3 reach the seller.
- **4.3** Register both pages in `webpack.config.js` (`discountcodepage`, `discountcodespage`) and in `server/middleware/renderer.js` (import + component map).
- **4.4** `ClientForm.js`: add `discountCodeIds` to the state schema and a multi-select rendered only when `props.isDiscountCodeEnabled`, placed after the client groups select. Omit the field from the payload when the feature is off.
- **4.5 (deferred, D3)** A clients multi-select on `DiscountCodeForm` is a separate extension. It needs seller-scoped Client.Api assignment contracts, service logic, repository mappings and tests, not just a `ClientIds` form field. The default implementation uses the client form only.

### Phase 5 - Enforcement plumbing shared by both apps

- **5.1 Resolver decorator** `Foundation.Pricing/Services/VerifiedDiscountCodePriceClientResolver`:

  ```csharp
  public async Task<PriceClient> ResolveAsync(Guid? clientId, string discountCode, string token)
  {
      var enforce = _settings.IsGrulaConfigured && _settings.DiscountCodeEnforcementEnabled;
      var priceClient = await _inner.ResolveAsync(clientId, enforce ? null : discountCode, token);

      if (!enforce || priceClient is null)
      {
          return priceClient;
      }

      if (string.IsNullOrWhiteSpace(discountCode))
      {
          priceClient.DiscountCode = null;
          return priceClient;
      }

      var validation = await _validator.ValidateAsync(priceClient.Id, discountCode, token);

      priceClient.DiscountCode = validation.IsValid ? validation.DiscountCode : null;

      return priceClient;
  }
  ```

  It uses `priceClient.Id` rather than the `clientId` argument because the buyer resolver takes the client from claims and rejects an explicit id.

- **5.2 Coordinator** `BasketDiscountCodeCoordinator.ResolveAsync`: add the enforcement flag, `Func<string, Task<DiscountCodeValidation>> validateDiscountCode` and a status-aware message factory for rejections. Preserve the existing resolution when enforcement is off. New logic after the existing resolution:

  ```text
  Grula not configured          -> unchanged: keep the stored code, validate nothing
  enforcement off               -> existing DiscountCodeResolver behaviour, validate nothing
  applied to an empty basket    -> Rejected (unchanged, still checked first)
  resolved code is null         -> ForDiscountCode(null): a removal needs no validation
  validation is Valid           -> ForDiscountCode(canonical spelling)
  validation is Unavailable     -> Rejected(could-not-be-verified message)
  otherwise (not applicable):
      read the stored code (only now, and at most once)
      same as the stored code   -> Removed(code): continue with null and report it
      different                 -> Rejected(invalid-code message)
  ```

  Reuse the persisted-code read from the existing resolution through one lazy task; it may already have been read for an omitted/blank code or an empty basket. Compare trimmed stored and resolved values with `OrdinalIgnoreCase`. `BasketDiscountCodeOutcome` gains `RemovedDiscountCode` and a `Removed(code)` factory that sets the effective code to null. `DiscountCodeResolver` does not change.

- **5.3** Add `string DiscountCodeRemovedMessage` to `BasketResponseModel` in both web apps.

- **5.4 Bind discounted saves to the client**: add nullable `DiscountCodeClientId` to the Basket.Api update/read/Redis service and repository models and both web apps' downstream basket API DTOs and domain models. Add it to repository `SaveAsync` mappings. The web write controllers set it from the successfully validated client when saving a non-null code with enforcement on; clear it when clearing the code. Do not accept it in browser save models or use it as authorisation evidence by itself. Redis baskets missing the new property deserialize with null and must be saved once before discounted checkout. This metadata guards client switches; it does not guarantee fresh Grula policy amounts or atomicity across separate save/checkout requests.

- **5.5 Check out the validated snapshot**: generate a new nullable Guid `BasketVersion` inside Basket.Api for every basket write and store it in the same Redis JSON value as the code, client metadata and lines. Return it through the read/save models and both web repository mappings. Add optional `ExpectedBasketVersion` to downstream checkout DTOs/service models; while enforcement is on, web checkout sends the version of **every** stored basket it read, including those without a code (a concurrent save could add one). A stored basket lacking a version requires a save first. For an allowed custom order with no persisted basket, send `Guid.Empty` to assert absence. `BasketService.CheckoutAsync` compares the expected version with its single deserialized snapshot **before publishing any stock or order events**; mismatch, or an absence assertion when a basket now exists, → 409 using `DiscountCodeBasketNeedsRefresh`. Build all events from that checked snapshot, with no second basket read. A concurrent save after this read cannot change the snapshot becoming the order. Legacy callers without a version retain their existing contract while enforcement is off; the verified service boundary in section 8 is required to prevent bypass through such callers. This is snapshot consistency, not a redesign of order idempotency or distributed event delivery.

### Phase 6 - Seller.Web enforcement

- **6.1** `Shared/Services/DiscountCodes/ClientApiDiscountCodeLookup : IDiscountCodeLookup`, calling `IDiscountCodesRepository.ValidateAsync` with the seller token and explicit client id. Both repositories must URL-encode query values and check HTTP success and the validation payload; do not follow the existing basket-read pattern of converting HTTP failure to null/not-found.
- **6.2** `Shared/DependencyInjection/CompositionRoot`: register the lookup and scoped validator; register `ClientRepositoryPriceClientResolver` as itself and `IPriceClientResolver` through a factory constructing the decorator around the concrete resolver. Do not inject `IPriceClientResolver` as its own inner dependency. Resolve the complete graph with scope validation in tests.
- **6.3** `Orders/BasketsApiController.Index` and `Orders/OrderFileApiController.Index`: pass `code => _discountCodeValidator.ValidateAsync(model.ClientId, code, token)` to the coordinator. When the outcome reports a removed code, set `DiscountCodeRemovedMessage` on the response. Sellers get the specific reason (D6).
- **6.4** `Orders/BasketCheckoutApiController.Checkout`: inject settings and the validator. After reading the nonempty basket, when both enforcement settings are on, require its `BasketVersion` and send it as `ExpectedBasketVersion` to Basket.Api (5.5), even if no code is stored. Missing version → 409 with `DiscountCodeBasketNeedsRefresh`. When it has a code, validate for `model.ClientId`: `Unavailable` → 503 with `DiscountCodeCouldNotBeVerified`; inapplicable → 409 with `DiscountCodeNoLongerValid`; both occur before `CheckoutBasketAsync` and leave the basket untouched. For a valid result also require `basket.DiscountCodeClientId == model.ClientId` and exact canonical spelling; otherwise return 409 with `DiscountCodeBasketNeedsRefresh`. A save through the normal pricing path must precede discounted checkout for a different client, even when that client is assigned the same code.
- **6.5** No change needed in `Products/ProductsApiController`: the decorator covers `GetProductsQuantities` and `GetPrice`.

### Phase 7 - Buyer.Web enforcement

- **7.1** `Shared/Repositories/Clients/IDiscountCodesRepository` + implementation with `ValidateAsync` only, and `Shared/Services/DiscountCodes/ClientApiDiscountCodeLookup`.
- **7.2** `Shared/DependencyInjection/CompositionRoot`: register them and the scoped validator; register `ClaimsPriceClientResolver` as itself and expose the decorator as `IPriceClientResolver` through a factory, as in 6.2.
- **7.3** `Orders/BasketsApiController.Index` and `Orders/OrderFileApiController.Index`: pass `code => _discountCodeValidator.ValidateAsync(User.GetClientId(), code, token)`. Use the single generic message for every not-applicable status (D6).
- **7.4** `Orders/BasketCheckoutApiController.Checkout`: inject settings and the validator. Resolve the effective basket id once from a valid cookie; use it for the basket read, stock validation and `CheckoutBasketAsync`. Resolve the effective client from `User.GetClientId()`; reject a non-null body client id that differs (400 with existing `ClientNotFound` text), and pass the server-derived client id into `CheckoutBasketAsync`. Apply 6.4 using these effective identities, including the version precondition for baskets without a code; null client cannot validate a code. Preserve the existing custom-order path without basket lines and without a code; with enforcement on, assert an absent persisted basket using the `Guid.Empty` version sentinel in 5.5. This closes the body-client and body-basket bypasses instead of validating a different identity from the one ordered for.
- **7.5** No change needed in the product API controllers or the SSR catalog builders: they all price through the resolver, so query-string codes (table rows 4 and 5) are covered by the decorator.
- **7.6** Leave `CatalogModelBuilder`, `ProductDetailModelBuilder` and `OrderFormModelBuilder` showing the stored code. If they hid a revoked code, the "Remove discount code" button would disappear while checkout still refused the basket, leaving the buyer stuck.
- **7.7 (recommended)** `OrderFormModelBuilder`: when enforcement is on and the stored code is no longer applicable, set `DiscountCodeWarning` on the view model so the basket page explains it on load instead of at checkout. Use the separate verification-unavailable message for transport failure. This is read-only: do not mutate the basket on a GET, and keep the remove control available.

Do **not** put assigned codes into the cached buyer claims. `ClaimsEnrichmentMiddleware` caches for 30 minutes, which would delay a revocation by that long.

### Phase 8 - Order form feedback (frontend)

The shared discount field and hooks live in `fe/shared`; their per-project re-export shims need no change. The buyer's `useOrderManagement` and both order-form components remain app-specific and are addressed below.

- **8.1** `fe/shared/hooks/useBasketDiscountCode.js`: in `applyBasketResponse`, show `toast.warning(jsonResponse.discountCodeRemovedMessage)` when present. Seller `OrderForm.js` saves/uploads and buyer `NewOrderForm.js` uploads use it; buyer ordinary line saves use `useOrderManagement` (8.2). It already syncs `discountCode` back into the field. Suppress the "code applied" success toast when a save actually removed the code.
- **8.2** `fe/projects/AspNetCore/src/shared/hooks/useOrderManagement.js`: the catalog and sidebar handle basket responses themselves (around the two `onDiscountCodeChanged` calls). Show the same warning there, through one small shared helper so the two paths cannot drift.
- **8.3** Rejections (400) and the checkout refusal (409) need no new code: both forms already toast the server `message`.
- **8.4 (if 7.7 is done)** Render `discountCodeWarning` under `DiscountCodeField` in `NewOrderForm.js`, with its view-model and PropTypes wiring. Clear a warning supplied on page load after a successful basket save, so a repaired basket does not keep stale warning text.

## 6. Resource keys

`ClientResources` (`en`, `pl`, `de`):

| Key | English text |
|-----|--------------|
| `DiscountCodes` | Discount codes |
| `NewDiscountCode` | New discount code |
| `EditDiscountCode` | Edit discount code |
| `DiscountCodeDescriptionLabel` | Description |
| `NavigateToDiscountCodesText` | Back to discount codes |
| `NoDiscountCodesText` | No discount codes |
| `DiscountCodeRequiredErrorMessage` | Enter a discount code |
| `DiscountCodeSavedSuccessfully` | Discount code saved |
| `DiscountCodeDeletedSuccessfully` | Discount code deleted |
| `DiscountCodeNotFound` | Discount code not found |
| `DiscountCodeExists` | A discount code with this value already exists |
| `DiscountCodeNotFoundInGrula` | This code is not defined in Grula. Add it to the "Discount Code" driver in Grula first |
| `DiscountCodeGrulaUnavailable` | The code could not be checked in Grula. Try again |

`OrderResources` (`en`, `pl`, `de`):

| Key | English text |
|-----|--------------|
| `DiscountCodeInvalid` | This discount code cannot be applied |
| `DiscountCodeDisabled` (seller) | This discount code is inactive |
| `DiscountCodeNotAssignedToClient` (seller) | This discount code is not assigned to the selected client |
| `DiscountCodeCouldNotBeVerified` | The discount code could not be verified. Try again |
| `DiscountCodeRemovedFromBasket` | Discount code {0} is no longer available and was removed |
| `DiscountCodeNoLongerValid` | Discount code {0} can no longer be applied. Remove it to continue |
| `DiscountCodeBasketNeedsRefresh` | Save the basket for the selected client before placing the order |

`DiscountCodeLabel` currently belongs to `OrderResources`; inject that localizer when reusing it on the client administration form and catalog. Do not request it from `ClientResources` unless explicitly adding that key there. Reuse existing global active/status/date labels. Add localized length/invalid-assignment messages to `ClientResources` for the new validators. Removal text deliberately makes no price-success promise: existing basket repricing clears price fields when Grula is unreachable and may still save successfully.

## 7. Tests

### Unit tests (`Giuru.UnitTests`, xUnit + NSubstitute)

| Subject | Cases |
|---------|-------|
| `DiscountCodeValidator` / HTTP lookup | null client → `ClientUnknown` without a lookup; blank/oversized code → no lookup; HTTP failure, timeout, malformed payload or unknown status → `Unavailable`; caller cancellation propagates; same trimmed/case-variant arguments, including concurrent calls → one lookup; canonical spelling reuses that result; different token or client → new lookup; `Unavailable` is reused only within the request; special characters are URL-encoded |
| `BasketDiscountCodeCoordinator` | Grula off or enforcement off → no validation and existing semantics; new valid code → canonical spelling; new invalid code → rejected and stored code untouched; stored invalid code → removed and reported, including explicit case variants; `Unavailable` → rejected; explicit removal → no validation; empty-basket rule still wins; stored code read at most once across both resolution and invalid-code comparison |
| `VerifiedDiscountCodePriceClientResolver` | valid → canonical; not applicable → null; `Unavailable` → null; no code → validator not called and inner code cleared; uses the inner client's nullable id; Grula off or enforcement off → no validation; enforcement on → inner resolver receives null code |
| `GrulaDiscountCodeService` | driver found on a later page; item matched ignoring case and canonical value returned; search cannot exclude case variants; ambiguous matches → refusal; no driver or no item → `Missing`; API/transport failure, timeout or broken paging → `Unavailable`; not configured → `NotConfigured`; bounded cache expires and is isolated by API URL/environment. Substitute with `Substitute.For<GrulaApiClient>("http://localhost", new HttpClient())` and configure the exact overloads used, or use a fake HttpMessageHandler for wire-contract tests; avoid an unconfigured partial substitute invoking live HTTP |
| Seller and Buyer `BasketCheckoutApiController` | invalid stored code → 409 and no checkout; unavailable → 503 and no checkout; valid with matching stored client/canonical code → proceeds; missing/different stored client → 409 even if both clients are assigned; Grula/enforcement off → no code validation; seller validates checkout client; buyer rejects a differing body client id, passes the principal's client id and uses cookie id for reads/stock validation/checkout; custom orders without a code retain existing behaviour |
| Seller `DiscountCodesApiController` | `Missing` → 422, nothing saved; `Unavailable` → 503, nothing saved; `NotConfigured` → 404, nothing saved; `Exists` → saved with canonical spelling; oversized canonical value rejected; update does not call Grula |
| DI / settings | resolve both web graphs with scope validation; Release 1 has no unresolved lookup dependency; default enforcement flag is false; enabled flag applies to pricing, writes, checkout and optional warnings together |
| Basket.Api snapshot guard | changed/stored-missing version → 409 before any events; matching version → events use the checked code/client/lines snapshot; covers concurrent introduction of a code to a code-free basket and an absence assertion when a custom-order basket now exists; a save interleaved after that read cannot alter events; each save generates a new version; legacy unguarded calls keep their off-state contract |

Update for the new constructor parameters and behaviour: `BasketsApiControllerFlowTests`, `OrderFileApiControllerFlowTests`, `PriceClientResolverTests`. `DiscountCodeResolverTests` stays as it is.

### Integration tests (`Giuru.IntegrationTests`)

- Add a `ClientApiClient` to `ApiFixture`. Codes must be seeded through Client.Api directly, because Seller.Web cannot verify them against the unreachable Grula. Explicitly enable `DiscountCodeEnforcementEnabled` in enforcement fixtures and retain separate off-state coverage.
- Extend mock-auth token generation / fixture clients to issue a real buyer token without `Seller`, and tokens for a second seller and buyer organisation. The existing single seller token cannot prove buyer authorisation, generic API responses or seller isolation. Verify JWT role mapping through HTTP. Create isolated client/code data before buyer requests and clear that email's cached enrichment claims if reusing it.
- New `DiscountCodeManagementTests` against Client.Api: create, case-insensitive duplicate → 409 (including concurrent creates), update without renaming, delete deactivates assignments, recreate deleted code does not inherit old assignments, de-duplicated assignment through client save, null keeps assignments and empty clears them, invalid/foreign ids → 422 with no partial update, non-seller assignment attempts (including empty lists) → 403, and the full seller validation matrix (`Valid`, `NotFound`, `Disabled`, `NotAssigned`, `ClientUnknown`).
- Use SQL Server integration coverage for the collation, filtered unique indexes and foreign keys. Seller B cannot list/read/edit/delete Seller A's codes or retrieve A's assignments via the client list; buyer invalid responses all expose `NotApplicable` without canonical code or assignment details. Check missing organisation/email claims and route/input validation.
- Seller.Web: creating a code with Grula unreachable is refused and nothing is stored.
- Seller.Web and Buyer.Web baskets: assigned code is stored with canonical spelling and server-derived `DiscountCodeClientId`; unassigned code → 400 with basket untouched; a stored code that is unassigned afterwards is removed on the next JSON save and multipart upload with `DiscountCodeRemovedMessage`; checkout with a revoked code → 409. Client.Api unavailable rejects writes/checkout without mutation. Legacy discounted baskets lacking client metadata need a save. Check that a code assigned to both A and B cannot carry A's persisted pricing into B's checkout without another save.
- Product APIs and SSR: use a deterministic Grula stub or inspect price-driver requests to prove invalid/query-string codes are omitted and valid codes are canonical. The unreachable-Grula fixture alone cannot prove discount enforcement: discounted and undiscounted requests both return no price. Use the stub to prove removing a revoked code reprices without that driver. Check read paths on Client.Api failure omit the driver without altering stored baskets.
- **Existing test that changes meaning:** `BasketDiscountCodeTests.SaveBuyerBasket_WithDiscountCode_WhenConfiguredGrulaIsUnavailable_StillStoresTheCode` saves `SUMMER25` for a user who has no client. After enforcement that request is rejected. Rewrite it as two tests: an unregistered code is rejected, and a registered and assigned code is still stored while Grula is unreachable.
  - The positive buyer case needs a client whose email and organisation match the new buyer token, so `ClaimsEnrichmentMiddleware` produces a client id and Client.Api accepts its identity. `Clients.Email` is `giuru@tests.com` today. Seed the client through Client.Api with the seller token and matching buyer organisation before the first buyer request; do not depend on the Seller.Web organisation-creation path or reuse its Seller token as buyer authorisation coverage. Assert that a valid assigned code is retained while unreachable Grula clears the basket's prices, matching current repricing behaviour.
- The existing `Basket.Api`-level storage tests keep their meaning: `Basket.Api` still stores what it is given by trusted callers. Add coverage for the new nullable client metadata; no discount validation is added at that layer in this design.
- Frontend/SSR checks: build Seller.Portal and AspNetCore, render the two new management pages, verify disabled assignments remain visible, feature-off payloads omit the field, server errors reach the form, and every save/upload path shows removal feedback without a contradictory success toast.

### Manual checks before release

1. Grula off: no menu entry, admin URLs return 404, order forms show no discount field, client save keeps assignments.
2. Create a code that exists in Grula, one that does not, and one with different casing.
3. Buyer: apply an assigned code, an unassigned one, and a nonexistent one. The last two show the same message.
4. Buyer: call `Products/ProductsApi/GetPrice?sku=…&discountCode=<another client's code>` directly. The price must equal the undiscounted price.
5. Seller: apply client A's code while ordering for client B → rejected with the specific reason.
6. Unassign a code while a basket holds it: the next basket change removes it with a notice, and checkout without any change returns the 409 message.
7. Price a seller basket for A, then try checkout for B with the same code assigned to both: require a basket save for B. Tamper with a buyer checkout's client/basket ids: the body cannot change the client or basket used for checkout checks.
8. Client.Api unavailable: reads omit the discount driver, writes and checkout refuse without altering the stored basket. Grula unavailable after valid assignment verification: follow existing unpriced-basket behaviour and show no claim that prices were successfully updated.
9. Both enforcement settings off/on: management remains reachable whenever Grula is configured, and all enforcement seams switch together. Exercise the resource filters and role authorisation through HTTP, including authenticated non-sellers.

## 8. Rollout

1. **Release 1: Phases 0 to 4.** Management and assignment only. Pricing behaves exactly as before.
2. **Seed.** Before enforcement, legitimate codes in use must be registered and assigned, otherwise they stop working. Sources: the `Discount Code` driver items in Grula, and distinct seller/client/code pairs on orders since discount codes shipped (`Order.DiscountCode`, migration `20260723203823_AddedDiscountCodes`). Historical orders are candidates, not authorisation: the old system accepted arbitrary codes, so sellers must review intended assignments. Import idempotently, preserving seller boundaries and canonical spelling; do not restore assignments from historical misuse.
3. **Release 2: Phases 5 to 8.** Deploy Basket.Api's nullable metadata and snapshot contract before the web checkout callers, with enforcement still off. Verify seeding, snapshot checks and the service boundary below, then enable `DiscountCodeEnforcementEnabled` in both web apps together. Open baskets holding an unregistered code lose it on their next change, with a notice. Existing baskets must be saved once to acquire a version before checkout; discounted baskets also acquire the client metadata. Make that refresh requirement visible to users.

If both releases ship together, the same default-off setting permits management and seeding first. Setting it back to false restores legacy acceptance of arbitrary codes, so rollback also removes the assignment restriction; treat it as a deliberate operational change.

**Required boundary check before enabling enforcement:** confirm that browser/buyer tokens cannot directly write or check out Basket.Api, bypassing the web tier. Local compose publishes port 5103 and Basket.Api's controllers currently use general `[Authorize]`, so authentication alone does not establish that trust. If production permits those calls, restrict service ingress and caller identity to the trusted web services, or move verification and trusted pricing into the actual write/checkout boundary before enabling this feature. Until that is verified, the guarantee is limited to the web entry points in section 2. The snapshot precondition in 5.5 is required because separate web validation and Basket.Api checkout reads are not atomic.

## 9. Risks and open points

| Risk | Impact | Mitigation |
|------|--------|------------|
| The Grula token may not be allowed to read drivers | D1 cannot be implemented as planned | Phase 0 answers this before any code depends on it |
| Unknown match field and paging base in the Grula driver API | `GrulaDiscountCodeService` matches nothing | Phase 0, plus the unit tests in section 7 pin the chosen behaviour |
| A code is removed in Grula after it was registered in Giuru | The code is accepted but has no price effect, silently | Out of scope. A later "check against Grula" action on the list page could reuse `IGrulaDiscountCodeService` |
| One extra Client.Api call per priced request that carries a code | Latency on catalog pages for buyers with an applied code | Indexed client/code/link queries, remembered per request. Measure first; any future cross-request read cache must document its revocation delay; writes and checkout stay uncached |
| `Basket.Api` stores any code and any prices it is sent | Direct calls can bypass web validation | Port 5103 is published in local compose. Verify production ingress and trusted caller identity before enabling enforcement; otherwise fix the actual service boundary as described in section 8 |
| A save changes the basket between web checkout validation and Basket.Api's read | A different code or client context can become the order | The required basket-version check and single event snapshot in 5.5; client metadata alone does not close this race |
| Grula unavailable during removal/repricing | The code is removed but trusted prices may be cleared rather than recalculated | Preserve existing price-clearing semantics; removal feedback must not promise updated prices |
| Enforcement before seeding | Codes in use stop working | The two-release order in section 8 |
| `OnModelCreating` is new in `ClientContext` | An unintended schema diff in the migration | Review the generated migration in step 1.3 |

## 10. Out of scope

- Defining discount amounts or price adjustment policies from Giuru.
- Assigning codes to client groups.
- Usage limits, one-time codes, usage reporting.
- Replacing the free-text field on the seller order form with a dropdown of the selected client's codes. It is a natural follow-up once assignment exists.
