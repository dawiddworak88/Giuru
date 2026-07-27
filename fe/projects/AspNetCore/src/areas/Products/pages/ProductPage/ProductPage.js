import React from "react";
import { ToastContainer } from "react-toastify";
import AppThemeProvider from "../../../../shared/components/AppThemeProvider/AppThemeProvider";
import LocaleHelper from "../../../../shared/helpers/globals/LocaleHelper";
import Store from "../../../../shared/stores/Store";
import Header from "../../../../shared/components/Header/Header";
import MainNavigation from "../../../../shared/components/MainNavigation/MainNavigation";
import ProductDetail from "../../components/ProductDetail/ProductDetail";
import Footer from "../../../../shared/components/Footer/Footer";
import Breadcrumbs from "../../../../shared/components/Breadcrumb/Breadcrumbs";
import NotificationBar from "../../../../shared/components/NotificationBar/NotificationBar";

function ProductPage(props) {

  LocaleHelper.setMomentLocale(props.locale);

  return (
    <AppThemeProvider locale={props.locale}>
      <ToastContainer />
      <Store>
        <div className="category-page">
          {props.notificationBar && props.notificationBar.items &&
            <NotificationBar {...props.notificationBar}></NotificationBar>
          }
          <Header {...props.header}></Header>
          <MainNavigation {...props.mainNavigation}></MainNavigation>
          <Breadcrumbs {...props.breadcrumbs}></Breadcrumbs>
          <ProductDetail locale={props.locale} {...props.productDetail}></ProductDetail>
          <Footer {...props.footer}></Footer>
        </div>
      </Store>
    </AppThemeProvider>
  );
}

export default ProductPage;
