import { createTheme } from "@mui/material/styles";
import * as locales from "@mui/material/locale";

export default class GlobalHelper {

  static initMuiTheme(locale) {

    var themeDefinition = {
      typography: {
        body1: {
          fontFamily: "'Nunito', sans-serif"
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
        }
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
