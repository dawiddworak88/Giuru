import React from 'react';
import Header from '../../../../../../shared/components/Header/Header';
import Footer from '../../../../../../shared/components/Footer/Footer';
import MenuTiles from '../../../../../../shared/components/MenuTiles/MenuTiles';
import Catalog from '../../../../../../shared/components/Catalog/Catalog';

/* eslint-disable no-unused-vars */
import favicon from '../../../../../../shared/layouts/images/favicon.png';
/* eslint-enable no-unused-vars */

function OrderPage(props) {
  return (
    <div className="home-page">
      <Header {...props.header}></Header>
      <MenuTiles {...props.menuTiles} />
      <Catalog {...props.catalog} />
      <section className="section">
        <p>
          {props.welcome}
        </p>
        <p>
          {props.learnMore}
        </p>
      </section>
      <Footer {...props.footer}></Footer>
    </div>
  );
}

export default OrderPage;
