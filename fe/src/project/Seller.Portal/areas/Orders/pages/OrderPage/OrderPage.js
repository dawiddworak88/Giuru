import React from "react";
import { toast } from "react-toastify";
import { ThemeProvider } from "@mui/styles";
import Store from "../../../../../../shared/stores/Store";
import GlobalHelper from "../../../../../../shared/helpers/globals/GlobalHelper";
import LocaleHelper from "../../../../../../shared/helpers/globals/LocaleHelper";
import Header from "../../../../../../shared/components/Header/Header";
import Footer from "../../../../../../shared/components/Footer/Footer";
import MenuTiles from "../../../../../../shared/components/MenuTiles/MenuTiles";
import OrderForm from "../../components/OrderForm/OrderForm";

function OrderPage(props) {

  toast.configure();

  LocaleHelper.setMomentLocale(props.locale);

  return (

      <ThemeProvider theme={GlobalHelper.initMuiTheme(props.locale)}>
        <Store>
          <div>
            <Header {...props.header}></Header>
            <MenuTiles {...props.menuTiles} />
            <OrderForm {...props.orderForm} />
            <Footer {...props.footer}></Footer>
          </div>
        </Store>
      </ThemeProvider>
    );
}

export default OrderPage;
