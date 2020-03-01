require('ignore-styles');

require('@babel/register')({
    plugins: [
        "@babel/plugin-proposal-object-rest-spread",
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

require('./index');