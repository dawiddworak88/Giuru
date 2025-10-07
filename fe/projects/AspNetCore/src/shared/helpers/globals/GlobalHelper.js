import { createTheme } from "@mui/material/styles";
import * as locales from "@mui/material/locale";
import DOMPurify from 'dompurify';

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
    return DOMPurify.sanitize(htmlString, {
      ALLOWED_TAGS: ['p', 'br', 'b', 'i', 'em', 'strong', 'a', 
        'ul', 'ol', 'li', 'h1', 'h2', 'h3', 'h4', 'h5', 'h6',
        'code', 'pre', 'blockquote', 'hr'],
      ALLOWED_ATTR: ['href', 'target', 'rel']
    });
  };

  static extractTextOnly = (htmlString) => {
    const div = document.createElement("div");
    div.innerHTML = htmlString;
    return div.textContent || div.innerText || "";
  };
}
