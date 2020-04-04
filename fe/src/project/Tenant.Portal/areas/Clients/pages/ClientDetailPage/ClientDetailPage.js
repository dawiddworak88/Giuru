import React from 'react';
import PropTypes from 'prop-types';
import Header from '../../../../../../shared/components/Header/Header';
import Footer from '../../../../../../shared/components/Footer/Footer';
import MenuTiles from '../../../../../../shared/components/MenuTiles/MenuTiles';
import ClientDetailForm from '../../components/ClientDetail/ClientDetailForm';

/* eslint-disable no-unused-vars */
import favicon from '../../../../../../shared/layouts/images/favicon.png';
/* eslint-enable no-unused-vars */

function ClientDetailPage(props) {
  return (
    <div>
      <Header {...props.header}></Header>
      <MenuTiles {...props.menuTiles} />
      <section className="section section-small-padding client-detail">
        <h1 className="subtitle is-4">{props.title}</h1>
        <div className="columns is-desktop">
            <div className="column is-half">
                <ClientDetailForm {...props.clientDetailForm} />
            </div>
        </div>
      </section>
      <Footer {...props.footer}></Footer>
    </div>
  );
}

ClientDetailPage.propTypes = {
    title: PropTypes.string.isRequired
};

export default ClientDetailPage;