// Single source of truth for the Babel presets used by both the client bundles (babel-loader) and
// server-side rendering (@babel/register).
//
// Files under fe/shared sit outside either project's package root, so a project .babelrc cannot
// reach them and the presets have to be passed explicitly at each entry point. Passing them from
// here keeps the four entry points - two webpack configs and two SSR bootstraps - from drifting
// apart, which is the failure mode that silently compiles the shared code differently from the
// project code that imports it.
module.exports = ["@babel/preset-env", "@babel/preset-react"];
