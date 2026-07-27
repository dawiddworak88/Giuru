import React from "react";
import { StyledEngineProvider } from "@mui/material/styles";
import { ThemeProvider } from "@mui/material/styles";
import GlobalHelper from "../../helpers/globals/GlobalHelper";

function AppThemeProvider({ locale, children }) {
  return (
    <StyledEngineProvider injectFirst>
      <ThemeProvider theme={GlobalHelper.initMuiTheme(locale)}>
        {children}
      </ThemeProvider>
    </StyledEngineProvider>
  );
}

export default AppThemeProvider;
