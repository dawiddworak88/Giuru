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
          main: "#647b76"
        },
        secondary: {
          main: "#647b76"
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
