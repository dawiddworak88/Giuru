import React from "react";
import { ToastContainer } from "react-toastify";
import { ThemeProvider } from "@mui/material/styles";
import GlobalHelper from "../../../../../../../shared/helpers/globals/GlobalHelper";
import Header from "../../../../../../../shared/components/Header/Header";
import Store from "../../../../../../../shared/stores/Store";
import Footer from "../../../../../../../shared/components/Footer/Footer";
import MenuTiles from "../../../../../../../shared/components/MenuTiles/MenuTiles";
import CountryForm from "../../components/CountryForm/CountryForm";

const CountryPage = (props) => {
  return (
    <ThemeProvider theme={GlobalHelper.initMuiTheme()}>
      <ToastContainer />
      <Store>
        <Header {...props.header}></Header>
        <MenuTiles {...props.menuTiles} />
        <CountryForm {...props.countryForm} />
        <Footer {...props.footer}></Footer>
      </Store>
    </ThemeProvider>
  );
}

export default CountryPage;
