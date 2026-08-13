const assert = require("node:assert/strict");
const fs = require("node:fs");
const path = require("node:path");
const test = require("node:test");

const helperSource = fs
    .readFileSync(path.join(__dirname, "ResponseMessageHelper.js"), "utf8")
    .replace("export default {", "module.exports = {");

assert.ok(helperSource.includes("module.exports = {"), "helper export shape changed - update this shim");

const helperModule = { exports: {} };

new Function("module", helperSource)(helperModule);

const ResponseMessageHelper = helperModule.exports;

const createResponse = ({ body, throws = false }) => {
    let jsonCalls = 0;

    return {
        response: {
            json: async () => {
                jsonCalls += 1;

                if (throws) {
                    throw new Error("Response body is not JSON");
                }

                return body;
            }
        },
        getJsonCalls: () => jsonCalls
    };
};

test("read returns a non-blank server message and parsed response", async () => {
    const { response, getJsonCalls } = createResponse({
        body: { message: "Basket prices could not be verified", id: "basket-1" }
    });

    const result = await ResponseMessageHelper.read(response, "Something went wrong");

    assert.deepEqual(result.jsonResponse, { message: "Basket prices could not be verified", id: "basket-1" });
    assert.equal(result.message, "Basket prices could not be verified");
    assert.equal(getJsonCalls(), 1);
});

test("read uses the fallback for blank and absent server messages", async () => {
    for (const body of [{ message: "   " }, {}, null]) {
        const { response } = createResponse({ body });

        const result = await ResponseMessageHelper.read(response, "Something went wrong");

        assert.equal(result.message, "Something went wrong");
    }
});

test("read uses the fallback when the response body is empty or non-JSON", async () => {
    const { response, getJsonCalls } = createResponse({ throws: true });

    const result = await ResponseMessageHelper.read(response, "Something went wrong");

    assert.equal(result.jsonResponse, null);
    assert.equal(result.message, "Something went wrong");
    assert.equal(getJsonCalls(), 1);
});
