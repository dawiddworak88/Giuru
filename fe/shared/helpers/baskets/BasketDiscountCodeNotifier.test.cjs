const assert = require("node:assert/strict");
const fs = require("node:fs");
const path = require("node:path");
const test = require("node:test");

// The helper is an ES module that imports react-toastify, so it is loaded the same way ResponseMessageHelper is:
// through a small shim that swaps the import for a recording toast.
const helperSource = fs
    .readFileSync(path.join(__dirname, "BasketDiscountCodeNotifier.js"), "utf8")
    .replace('import { toast } from "react-toastify";', "")
    .replace("export default class BasketDiscountCodeNotifier", "class BasketDiscountCodeNotifier");

assert.ok(helperSource.includes("class BasketDiscountCodeNotifier"), "helper export shape changed - update this shim");
assert.ok(!helperSource.includes("react-toastify"), "helper import shape changed - update this shim");

const load = () => {
    const warnings = [];
    const toast = { warning: (message) => warnings.push(message) };
    const Notifier = new Function("toast", `${helperSource}\nreturn BasketDiscountCodeNotifier;`)(toast);

    return { Notifier, warnings };
};

test("notifyIfRemoved warns with the server message and reports that it did", () => {
    const { Notifier, warnings } = load();

    const notified = Notifier.notifyIfRemoved({ id: "basket-1", discountCode: null, discountCodeRemovedMessage: "Discount code SUMMER25 is no longer available and was removed" });

    assert.equal(notified, true);
    assert.deepEqual(warnings, ["Discount code SUMMER25 is no longer available and was removed"]);
});

test("notifyIfRemoved stays silent when the save kept the code", () => {
    const { Notifier, warnings } = load();

    assert.equal(Notifier.notifyIfRemoved({ id: "basket-1", discountCode: "SUMMER25" }), false);
    assert.equal(Notifier.notifyIfRemoved({ id: "basket-1", discountCodeRemovedMessage: null }), false);
    assert.equal(Notifier.notifyIfRemoved({ id: "basket-1", discountCodeRemovedMessage: "" }), false);
    assert.deepEqual(warnings, []);
});

test("notifyIfRemoved tolerates a missing response", () => {
    const { Notifier, warnings } = load();

    assert.equal(Notifier.notifyIfRemoved(undefined), false);
    assert.equal(Notifier.notifyIfRemoved(null), false);
    assert.deepEqual(warnings, []);
});

test("every basket response handler goes through the notifier", () => {
    // The shared hook and the buyer's own order management are the two places a basket response is applied. If one of
    // them stops calling the notifier, a removed code would silently disappear from the basket.
    const hook = fs.readFileSync(path.join(__dirname, "..", "..", "hooks", "useBasketDiscountCode.js"), "utf8");
    const orderManagement = fs.readFileSync(
        path.join(__dirname, "..", "..", "..", "projects", "AspNetCore", "src", "shared", "hooks", "useOrderManagement.js"),
        "utf8"
    );

    assert.ok(hook.includes("BasketDiscountCodeNotifier.notifyIfRemoved"), "useBasketDiscountCode no longer notifies about a removed code");
    assert.ok(orderManagement.includes("BasketDiscountCodeNotifier.notifyIfRemoved"), "useOrderManagement no longer notifies about a removed code");
});

test("the success toast of an applied code is suppressed when the save removed the code", () => {
    const hook = fs.readFileSync(path.join(__dirname, "..", "..", "hooks", "useBasketDiscountCode.js"), "utf8");

    assert.ok(
        hook.includes("showSuccessMessage && !jsonResponse.discountCodeRemovedMessage"),
        "a save that removed the code must not also toast that a code was applied"
    );
});
