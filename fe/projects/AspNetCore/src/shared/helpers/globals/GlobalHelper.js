import { createTheme } from "@mui/material/styles";
import * as locales from "@mui/material/locale";

export default class GlobalHelper {

  static initMuiTheme(locale) {

    var themeDefinition = {
      typography: {
        fontFamily: "Nunito Sans",
        body1: {
          fontFamily: "Nunito Sans"
        },
        button: {
          textTransform: "none"
        }
      },
      palette: {
        primary: {
          main: "#1B5A6E"
        },
        secondary: {
          main: "#1B5A6E"
        },
        mint: {
          300: "#386876",
          400: "#1F5565",
          500: "#064254"
        },
        gray: {
          100: "#F7F7F7",
          200: "#EBECED",
          300: "#D5D7D8"
        },
        black: {
          300: "#7C8693"
        },
        whiteBase: "#FFFFFF",
        blackBase: "#171717"
      }
    };

    if (locale) {

      const localeMappings = {
        "pl": "plPL",
        "de": "deDE",
        "en": "enUS"
      };

      return createTheme(themeDefinition, locales[localeMappings[locale]]);
    }
    else {

      return createTheme(themeDefinition);
    }
  }

  static sanitizeHtml = (htmlString) => {
    const div = document.createElement("div");

    div.innerHTML = htmlString;

    const dangerousElements = [
      'script', 'style', 'iframe', 'object', 'embed', 'form', 
      'input', 'textarea', 'select', 'button', 'meta', 'link',
      'base', 'applet', 'svg', 'math', 'audio', 'video', 'source',
      'track', 'frame', 'frameset', 'noframes', 'noscript'
    ];

    div.querySelectorAll(dangerousElements).forEach(el => el.remove());

    return div.innerHTML;
  };

  static extractTextOnly = (htmlString) => {
    const div = document.createElement("div");
    div.innerHTML = htmlString;
    return div.textContent || div.innerText || "";
  };
}
