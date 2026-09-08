const path = require("path");
const Module = require("module");

/**
 * Lets modules under fe/shared resolve the packages installed in a project.
 *
 * Node resolves a bare import ("react", "@mui/material") by walking node_modules up from the
 * importing file. fe/shared has no node_modules of its own and fe/node_modules does not carry the
 * UI dependencies, so a shared module that imports React fails with MODULE_NOT_FOUND during SSR
 * even though the same import resolves fine in the browser bundle - webpack is told about the
 * project's node_modules through resolve.modules, and this is the server-side counterpart.
 *
 * The project's node_modules is appended rather than prepended, so every path that already
 * resolves keeps resolving exactly as before and this only ever acts as a last resort. Node caches
 * modules by resolved filename, so shared and project files end up sharing a single copy of React
 * rather than each instantiating their own - two copies would break hooks.
 *
 * @param {string} projectRoot Absolute path of the project whose node_modules should back fe/shared.
 */
module.exports = function registerSharedModuleResolution(projectRoot) {
    const projectNodeModules = path.resolve(projectRoot, "node_modules");
    const originalNodeModulePaths = Module._nodeModulePaths;

    Module._nodeModulePaths = function (from) {
        const paths = originalNodeModulePaths.call(this, from);

        return paths.indexOf(projectNodeModules) === -1
            ? paths.concat(projectNodeModules)
            : paths;
    };
};
