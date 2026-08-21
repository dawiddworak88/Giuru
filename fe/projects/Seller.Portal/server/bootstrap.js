const path = require("path");

const projectRoot = path.resolve(__dirname, "..");
const sharedRoot = path.resolve(__dirname, "../../../shared");

// fe/shared has no node_modules of its own, so give its modules access to this project's.
require(path.join(sharedRoot, "build/registerSharedModuleResolution.cjs"))(projectRoot);

require("ignore-styles");
require("@babel/register")({
    root: "./middleware",
    presets: require(path.join(sharedRoot, "build/babelPresets.cjs")),
    // Naming `only` clears @babel/register's default node_modules ignore, so restate it - without
    // it every dependency reached during SSR is transpiled on the way in.
    ignore: [/node_modules/],
    only: [projectRoot, sharedRoot]
});
require("./index");
