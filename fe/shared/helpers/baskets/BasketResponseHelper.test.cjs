const assert = require("node:assert/strict");
const fs = require("node:fs");
const path = require("node:path");
const test = require("node:test");

const loadModule = (source, dependencies = {}) => {
    const module = { exports: {} };

    new Function("module", ...Object.keys(dependencies), source)(module, ...Object.values(dependencies));

    return module.exports;
};

const readSource = (relativePath) => fs.readFileSync(path.join(__dirname, relativePath), "utf8");

const responseMessageHelperSource = readSource("../responses/ResponseMessageHelper.js")
    .replace("export default {", "module.exports = {");
const basketResponseHelperSource = readSource("BasketResponseHelper.js")
    .replace('import ResponseMessageHelper from "../responses/ResponseMessageHelper";', "")
    .replace("export default {", "module.exports = {");

assert.ok(responseMessageHelperSource.includes("module.exports = {"), "ResponseMessageHelper export shape changed - update this shim");
assert.ok(basketResponseHelperSource.includes("module.exports = {"), "BasketResponseHelper export shape changed - update this shim");
assert.ok(!basketResponseHelperSource.includes("import "), "BasketResponseHelper imports changed - update this shim");

const ResponseMessageHelper = loadModule(responseMessageHelperSource);
const BasketResponseHelper = loadModule(basketResponseHelperSource, { ResponseMessageHelper });

const createResponse = ({ status, body }) => ({
    ok: status >= 200 && status < 300,
    status,
    json: async () => body
});

test("read applies a saved basket without a notice", async () => {
    const basket = { id: "basket-1", items: [], discountCode: "SUMMER25" };

    const result = await BasketResponseHelper.read(createResponse({ status: 200, body: basket }), "Something went wrong");

    assert.equal(result.isSaved, true);
    assert.equal(result.notice, null);
    assert.deepEqual(result.jsonResponse, basket);
});

test("read applies the basket of a 422 that removed the discount code and shows its message as a notice", async () => {
    const basket = { id: "basket-1", items: [], discountCode: null, message: "The discount code SUMMER25 is no longer valid" };

    const result = await BasketResponseHelper.read(createResponse({ status: 422, body: basket }), "Something went wrong");

    assert.equal(result.isSaved, true);
    assert.equal(result.notice, "The discount code SUMMER25 is no longer valid");
    assert.deepEqual(result.jsonResponse, basket);
});

test("read does not apply a 422 that carries only a message", async () => {
    const result = await BasketResponseHelper.read(
        createResponse({ status: 422, body: { message: "Basket prices could not be verified" } }),
        "Something went wrong");

    assert.equal(result.isSaved, false);
    assert.equal(result.notice, null);
    assert.equal(result.message, "Basket prices could not be verified");
});

test("read does not apply the body of any other failed save", async () => {
    for (const status of [400, 500]) {
        const result = await BasketResponseHelper.read(
            createResponse({ status, body: { id: "basket-1", message: "This discount code cannot be applied" } }),
            "Something went wrong");

        assert.equal(result.isSaved, false);
        assert.equal(result.notice, null);
        assert.equal(result.message, "This discount code cannot be applied");
    }
});

test("read does not apply a successful response without a body", async () => {
    const result = await BasketResponseHelper.read(createResponse({ status: 200, body: null }), "Something went wrong");

    assert.equal(result.isSaved, false);
    assert.equal(result.message, "Something went wrong");
});
