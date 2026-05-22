const path = require("path");

/** @type { import('@storybook/react-webpack5').StorybookConfig } */
module.exports = {
  stories: ["../src/**/*.stories.js"],
  
  addons: [
    "@storybook/addon-links",
    "@storybook/addon-webpack5-compiler-swc",
    "@storybook/addon-docs"
  ],

  framework: {
    name: "@storybook/react-webpack5"
  },

  webpackFinal: async (config) => {
    config.module.rules.push({
      test: /\.(scss|sass)$/,
      use: [
        "style-loader",
        "css-loader",
        "postcss-loader",
        "sass-loader",
      ],
      include: path.resolve(__dirname, "../"),
    });
    return config;
  },

  typescript: {
    reactDocgen: "react-docgen-typescript"
  }
};