require("ignore-styles");

require("@babel/register")({
    root:"./middleware",
    presets: ["@babel/preset-env","@babel/preset-react"],
    plugins: [
        ["@babel/plugin-proposal-class-properties"],
        [
            "file-loader",
            {
				"name": "[name].[ext]",
				"publicPath": "/dist/images",
				"outputPath": "/dist/images"
            }
        ]
	]
});

require("./index");