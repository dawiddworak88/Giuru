import React from "react";
import { ToastContainer } from "react-toastify";
import { ThemeProvider } from "@mui/material/styles";
import Store from "../../../../../../shared/stores/Store";
import GlobalHelper from "../../../../../../shared/helpers/globals/GlobalHelper";
import LocaleHelper from "../../../../../../shared/helpers/globals/LocaleHelper";
import Header from "../../../../../../shared/components/Header/Header";
import Footer from "../../../../../../shared/components/Footer/Footer";
import MenuTiles from "../../../../../../shared/components/MenuTiles/MenuTiles";
import OrderStatusDetail from "../../components/OrderStatusDetail/OrderStatusDetail";

function OrderStatusPage(props) {

  LocaleHelper.setMomentLocale(props.locale);

  return (
      <ThemeProvider theme={GlobalHelper.initMuiTheme(props.locale)}>
        <ToastContainer />
        <Store>
          <Header {...props.header}></Header>
          <MenuTiles {...props.menuTiles} />
          <OrderStatusDetail {...props.orderStatusDetail} />
          <Footer {...props.footer}></Footer>
        </Store>
      </ThemeProvider>
    );
}

export default OrderStatusPage;
