import React from "react";
import { toast } from "react-toastify";
import { ThemeProvider } from "@material-ui/core/styles";
import { Plus } from "react-feather";
import Store from "../../../../../../shared/stores/Store";
import GlobalHelper from "../../../../../../shared/helpers/globals/GlobalHelper";
import LocaleHelper from "../../../../../../shared/helpers/globals/LocaleHelper";
import Header from "../../../../../../shared/components/Header/Header";
import Footer from "../../../../../../shared/components/Footer/Footer";
import MenuTiles from "../../../../../../shared/components/MenuTiles/MenuTiles";
import ProductCatalog from "../../components/ProductCatalog/ProductCatalog";

function ProductPage(props) {

  toast.configure();

  LocaleHelper.setMomentLocale(props.locale);

  return (
    <ThemeProvider theme={GlobalHelper.initMuiTheme()}>
      <Store>
        <div>
          <Header {...props.header}></Header>
          <MenuTiles {...props.menuTiles} />
          <section className="section section-small-padding catalog">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div>
              {props.newUrl &&
                <a href={props.newUrl} className="button is-primary">
                  <span className="icon">
                    <Plus />
                  </span>
                  <span>
                    {props.newText}
                  </span>
                </a>
              }
            </div>
            <ProductCatalog {...props.catalog}></ProductCatalog>
          </section>
          <Footer {...props.footer}></Footer>
        </div>
      </Store>
    </ThemeProvider>
  );
}

export default ProductPage;