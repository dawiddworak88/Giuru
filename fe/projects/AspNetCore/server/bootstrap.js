require("ignore-styles");

require("@babel/register")({
    root: "./middleware",
    presets: ["@babel/preset-env", "@babel/preset-react"]
});

require("./index");