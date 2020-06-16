import React from 'react';
import { Plus, UploadCloud } from 'react-feather';
import Header from '../../../../../../shared/components/Header/Header';
import Footer from '../../../../../../shared/components/Footer/Footer';
import MenuTiles from '../../../../../../shared/components/MenuTiles/MenuTiles';

/* eslint-disable no-unused-vars */
import favicon from '../../../../../../shared/layouts/images/favicon.png';
/* eslint-enable no-unused-vars */

function ImportOrderPage(props) {
  return (
    <div>
      <Header {...props.header}></Header>
      <MenuTiles {...props.menuTiles} />
      <section className="section section-small-padding catalog">
        <h1 className="subtitle is-4">{props.title}</h1>
      </section>
      <Footer {...props.footer}></Footer>
    </div>
  );
}

export default ImportOrderPage;