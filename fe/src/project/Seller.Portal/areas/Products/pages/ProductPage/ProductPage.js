import React from "react";
import { toast } from "react-toastify";
import { ThemeProvider } from "@mui/styles";
import GlobalHelper from "../../../../../../shared/helpers/globals/GlobalHelper";
import Header from "../../../../../../shared/components/Header/Header";
import Store from "../../../../../../shared/stores/Store";
import Footer from "../../../../../../shared/components/Footer/Footer";
import MenuTiles from "../../../../../../shared/components/MenuTiles/MenuTiles";
import ProductForm from "../../components/ProductForm/ProductForm";

function ProductPage(props) {

  toast.configure();

  return (
    <ThemeProvider theme={GlobalHelper.initMuiTheme()}>
      <Store>
        <div>
          <Header {...props.header}></Header>
          <MenuTiles {...props.menuTiles} />
          <ProductForm {...props.productForm} />
          <Footer {...props.footer}></Footer>
        </div>
      </Store>
    </ThemeProvider>
  );
}

export default ProductPage;
