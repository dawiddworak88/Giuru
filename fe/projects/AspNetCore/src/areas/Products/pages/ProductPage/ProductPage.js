import React from "react";
import { ToastContainer } from "react-toastify";
import { ThemeProvider } from "@mui/material/styles";
import GlobalHelper from "../../../../../../shared/helpers/globals/GlobalHelper";
import LocaleHelper from "../.../.../../../../shared/helpers/globals/LocaleHelper";
import Store from "../../../../../../shared/stores/Store";
import Header from "../../../../shared/components/Header/Header";
import MainNavigation from "../../../../shared/components/MainNavigation/MainNavigation";
import ProductDetail from "../../components/ProductDetail/ProductDetail";
import Footer from "../../../../../../shared/components/Footer/Footer";
import Breadcrumbs from "../../../../shared/components/Breadcrumb/Breadcrumbs";

function ProductPage(props) {

  LocaleHelper.setMomentLocale(props.locale);

  return (
    <ThemeProvider theme={GlobalHelper.initMuiTheme(props.locale)}>
      <ToastContainer />
      <Store>
        <div className="category-page">
          <Header {...props.header}></Header>
          <MainNavigation {...props.mainNavigation}></MainNavigation>
          <Breadcrumbs {...props.breadcrumbs}></Breadcrumbs>
          <ProductDetail {...props.productDetail}></ProductDetail>
          <Footer {...props.footer}></Footer>
        </div>
      </Store>
    </ThemeProvider>
  );
}

export default ProductPage;
