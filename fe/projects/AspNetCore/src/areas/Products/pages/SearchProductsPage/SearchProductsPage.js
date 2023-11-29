import React from "react";
import { ToastContainer } from "react-toastify";
import { ThemeProvider } from "@mui/material/styles";
import GlobalHelper from "../../../../shared/helpers/globals/GlobalHelper";
import Store from "../../../../shared/stores/Store";
import Header from "../../../../shared/components/Header/Header";
import MainNavigation from "../../../../shared/components/MainNavigation/MainNavigation";
import Catalog from "../../../../shared/components/Catalog/Catalog";
import Footer from "../../../../shared/components/Footer/Footer";
import NotificationBar from "../../../../shared/components/NotificationBar/NotificationBar";

function SearchProductsPage(props) {
  return (
    <ThemeProvider theme={GlobalHelper.initMuiTheme()}>
      <ToastContainer />
      <Store>
        <div className="search-products-page">
          {props.notificationBar && props.notificationBar.items &&
            <NotificationBar {...props.notificationBar}></NotificationBar>
          }
          <Header {...props.header}></Header>
          <MainNavigation {...props.mainNavigation}></MainNavigation>
          <Catalog {...props.catalog}></Catalog>
          <Footer {...props.footer}></Footer>
        </div>
      </Store>
    </ThemeProvider>
  );
}

export default SearchProductsPage;
