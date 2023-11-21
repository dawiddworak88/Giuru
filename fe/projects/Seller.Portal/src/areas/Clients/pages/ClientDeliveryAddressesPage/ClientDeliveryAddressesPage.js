import React from "react";
import { ToastContainer } from "react-toastify";
import { ThemeProvider } from "@mui/material/styles";
import GlobalHelper from "../../../../shared/helpers/globals/GlobalHelper";
import Header from "../../../../shared/components/Header/Header";
import Store from "../../../../shared/stores/Store";
import Footer from "../../../../shared/components/Footer/Footer";
import MenuTiles from "../../../../shared/components/MenuTiles/MenuTiles";
import LocaleHelper from "../../../../shared/helpers/globals/LocaleHelper";
import Catalog from "../../../../shared/components/Catalog/Catalog";

const ClientDeliveryAddressesPage = (props) => {

  LocaleHelper.setMomentLocale(props.locale);

  return (
    <ThemeProvider theme={GlobalHelper.initMuiTheme()}>
      <ToastContainer />
      <Store>
        <Header {...props.header}></Header>
        <MenuTiles {...props.menuTiles} />
        <Catalog {...props.catalog}></Catalog>
        <Footer {...props.footer}></Footer>
      </Store>
    </ThemeProvider>
  );
}

export default ClientDeliveryAddressesPage;
