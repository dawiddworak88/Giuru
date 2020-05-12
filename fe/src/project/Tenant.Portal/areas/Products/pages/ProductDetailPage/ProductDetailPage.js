import React from 'react';
import { ThemeProvider } from '@material-ui/core/styles';
import GlobalHelper from '../../../../../../shared/helpers/globals/GlobalHelper';
import Header from '../../../../../../shared/components/Header/Header';
import Footer from '../../../../../../shared/components/Footer/Footer';
import MenuTiles from '../../../../../../shared/components/MenuTiles/MenuTiles';
import ProductDetailForm from '../../components/ProductDetail/ProductDetailForm';

/* eslint-disable no-unused-vars */
import favicon from '../../../../../../shared/layouts/images/favicon.png';
/* eslint-enable no-unused-vars */

function ProductDetailPage(props) {
  return (
    <ThemeProvider theme={GlobalHelper.initMuiTheme()}>
      <Store>
        <div>
          <Header {...props.header}></Header>
          <MenuTiles {...props.menuTiles} />
          <ProductDetailForm {...props.productDetailForm} />
          <Footer {...props.footer}></Footer>
        </div>
      </Store>
    </ThemeProvider>
  );
}

export default ProductDetailPage;