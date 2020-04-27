import React from 'react';
import Header from '../../../../../../shared/components/Header/Header';
import Footer from '../../../../../../shared/components/Footer/Footer';
import MenuTiles from '../../../../../../shared/components/MenuTiles/MenuTiles';

/* eslint-disable no-unused-vars */
import favicon from '../../../../../../shared/layouts/images/favicon.png';
/* eslint-enable no-unused-vars */

function ProductDetailPage(props) {
  return (
    <div>
      <Header {...props.header}></Header>
      <MenuTiles {...props.menuTiles} />
      <Footer {...props.footer}></Footer>
    </div>
  );
}

export default ProductDetailPage;