import React from "react";
import { toast } from "react-toastify";
import { ThemeProvider } from "@material-ui/core/styles";
import GlobalHelper from "../../../../../../shared/helpers/globals/GlobalHelper";
import Store from "../../../../../../shared/stores/Store";
import Header from "../../../../shared/components/Header/Header";
import MainNavigation from "../../../../shared/components/MainNavigation/MainNavigation";
import BrandDetail from "../../components/BrandDetail/BrandDetail";
import Catalog from "../../../../shared/components/Catalog/Catalog";
import Footer from "../../../../../../shared/components/Footer/Footer";
import Breadcrumbs from "../../../../shared/components/Breadcrumb/Breadcrumbs";

function BrandPage(props) {

  toast.configure();

  return (
    <ThemeProvider theme={GlobalHelper.initMuiTheme()}>
      <Store>
        <div className="brand-page">
          <Header {...props.header}></Header>
          <MainNavigation {...props.mainNavigation}></MainNavigation>
          <Breadcrumbs {...props.breadcrumbs}></Breadcrumbs>
          <BrandDetail {...props.brandDetail}></BrandDetail>
          <Catalog {...props.catalog}></Catalog>
          <Footer {...props.footer}></Footer>
        </div>
      </Store>
    </ThemeProvider>
  );
}

export default BrandPage;
