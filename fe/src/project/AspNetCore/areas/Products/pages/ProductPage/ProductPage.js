import React from "react";
import { toast } from "react-toastify";
import { ThemeProvider } from "@material-ui/core/styles";
import GlobalHelper from "../../../../../../shared/helpers/globals/GlobalHelper";
import Store from "../../../../../../shared/stores/Store";
import Header from "../../../../shared/components/Header/Header";
import MainNavigation from "../../../../shared/components/MainNavigation/MainNavigation";
import ProductDetail from "../../components/ProductDetail/ProductDetail";
import Footer from "../../../../../../shared/components/Footer/Footer";

/* eslint-disable no-unused-vars */
import favicon from "../../../../../../shared/layouts/images/favicon.png";
/* eslint-enable no-unused-vars */

function ProductPage(props) {

  toast.configure();

  return (
    <ThemeProvider theme={GlobalHelper.initMuiTheme()}>
      <Store>
        <div className="category-page">
          <Header {...props.header}></Header>
          <MainNavigation {...props.mainNavigation}></MainNavigation>
          <ProductDetail {...props.productDetail}></ProductDetail>
          <Footer {...props.footer}></Footer>
        </div>
      </Store>
    </ThemeProvider>
  );
}

export default ProductPage;
