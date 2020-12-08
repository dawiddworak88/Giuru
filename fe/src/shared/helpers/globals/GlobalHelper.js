import { createMuiTheme } from "@material-ui/core/styles";

export default class GlobalHelper {

  static initMuiTheme() {

    const theme = createMuiTheme({
      typography: {
        body1: {
          fontFamily: "'Poppins', sans-serif"
        },
        button: {
          textTransform: "none"
        }
      },
      palette: {
        primary: {
          main: "#BF202F"
        },
        secondary: {
          main: "#5E1916"
        }
      }
    });

    return theme;
  }
}
