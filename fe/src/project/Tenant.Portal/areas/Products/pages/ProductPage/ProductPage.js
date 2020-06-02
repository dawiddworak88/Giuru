import React from 'react';
import { ThemeProvider } from '@material-ui/core/styles';
import GlobalHelper from '../../../../../../shared/helpers/globals/GlobalHelper';
import Header from '../../../../../../shared/components/Header/Header';
import Footer from '../../../../../../shared/components/Footer/Footer';
import MenuTiles from '../../../../../../shared/components/MenuTiles/MenuTiles';
import Catalog from '../../../../../../shared/components/Catalog/Catalog';

/* eslint-disable no-unused-vars */
import favicon from '../../../../../../shared/layouts/images/favicon.png';
/* eslint-enable no-unused-vars */

function ProductPage(props) {
  return (
    <ThemeProvider theme={GlobalHelper.initMuiTheme()}>
      <div>
        <Header {...props.header}></Header>
        <MenuTiles {...props.menuTiles} />
        <Catalog {...props.catalog} />
        <Footer {...props.footer}></Footer>
      </div>
    </ThemeProvider>
  );
}

export default ProductPage;