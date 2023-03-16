import React from "react";
import { ToastContainer } from "react-toastify";
import { ThemeProvider } from "@mui/material/styles";
import Store from "../../../../../../shared/stores/Store";
import GlobalHelper from "../../../../../../shared/helpers/globals/GlobalHelper";
import LocaleHelper from "../../../../../../shared/helpers/globals/LocaleHelper";
import Header from "../../../../shared/components/Header/Header";
import Footer from "../../../../../../shared/components/Footer/Footer";
import OrderItemForm from "../../components/OrderItemForm/OrderItemForm";
import MainNavigation from "../../../../shared/components/MainNavigation/MainNavigation";

const OrderItemPage = (props) => {

  LocaleHelper.setMomentLocale(props.locale);

  return (
      <ThemeProvider theme={GlobalHelper.initMuiTheme(props.locale)}>
        <ToastContainer />
        <Store>
          <Header {...props.header}></Header>
          <MainNavigation {...props.mainNavigation}></MainNavigation>
          <OrderItemForm {...props.orderItemForm} />
          <Footer {...props.footer}></Footer>
        </Store>
      </ThemeProvider>
    );
}

export default OrderItemPage;
