# Giuru

A B2B ordering platform: a Seller publishes a catalog, and its Clients build Baskets and place Orders, priced by Grula.

## Language

### Parties

**Seller**:
The organisation running Giuru, and its staff working in the Seller portal to manage the catalog, Clients and Orders.
_Avoid_: Admin

**Client**:
A business customer of the Seller that places Orders through the Buyer portal, or has them placed on its behalf by the Seller.
_Avoid_: Customer, buyer, account

**Team Member**:
A user who belongs to a Client's organisation and acts on that Client's behalf; for pricing and Discount Codes they are the Client.
_Avoid_: Sub-account, employee

### Pricing

**Grula**:
The external pricing engine that computes every price Giuru shows or stores, including the effect of a Discount Code.

**Price Driver**:
A named attribute of the product or the Client sent to Grula to determine a price.

**Discount Code**:
A code issued by the Seller that a Client can apply to its Basket; Giuru decides who may use it, Grula decides what discount it gives.
_Avoid_: Coupon, promo code, voucher

**Assigned Clients**:
The Clients allowed to apply a given Discount Code; a code with no Assigned Clients cannot be used by anyone.
_Avoid_: Eligible customers, public code

**Inactive Discount Code**:
A Discount Code switched off by the Seller; nobody can apply it, but it stays listed and editable.
_Avoid_: Disabled code, deleted code

**Valid Discount Code**:
A Discount Code that exists, is active, and lists the Client among its Assigned Clients.

### Ordering

**Basket**:
The lines a Client, or the Seller on a Client's behalf, is preparing to order.

**Applied Discount Code**:
The Discount Code stored on a Basket and used to price its lines.
