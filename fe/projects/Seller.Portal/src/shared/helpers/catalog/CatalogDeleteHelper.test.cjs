const assert = require("node:assert/strict");
const fs = require("node:fs");
const path = require("node:path");
const test = require("node:test");

const helperSource = fs
    .readFileSync(path.join(__dirname, "CatalogDeleteHelper.js"), "utf8")
    .replace("export default {", "module.exports = {");

assert.ok(helperSource.includes("module.exports = {"), "helper export shape changed - update this shim");

const helperModule = { exports: {} };

new Function("module", helperSource)(helperModule);

const CatalogDeleteHelper = helperModule.exports;

test("a catalog without custom parameters sends only the id", () => {
    const parameters = CatalogDeleteHelper.getDeleteParameters({ id: "code-1", version: "v1", name: "Summer" });

    assert.deepEqual(parameters, { id: "code-1" });
});

test("a catalog with custom parameters sends exactly what the page asks for", () => {
    const parameters = CatalogDeleteHelper.getDeleteParameters(
        { id: "code-1", version: "v1", name: "Summer" },
        (item) => ({ id: item.id, version: item.version }));

    assert.deepEqual(parameters, { id: "code-1", version: "v1" });
});

test("a custom parameters value that is not a function falls back to the id", () => {
    const parameters = CatalogDeleteHelper.getDeleteParameters({ id: "code-1", version: "v1" }, { id: "x" });

    assert.deepEqual(parameters, { id: "code-1" });
});
