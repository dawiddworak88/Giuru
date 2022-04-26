import { createMuiTheme } from "@material-ui/core/styles";
import * as locales from "@material-ui/core/locale";

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

      return createMuiTheme(themeDefinition, locales[localeMappings[locale]]);
    }
    else {
      
      return createMuiTheme(themeDefinition);
    }
  }
}
