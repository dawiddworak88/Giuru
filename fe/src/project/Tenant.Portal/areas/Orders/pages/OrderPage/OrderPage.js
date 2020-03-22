import React from 'react';
import Header from '../../../../shared/components/Header/Header';
import Footer from '../../../../shared/components/Footer/Footer';

/* eslint-disable no-unused-vars */
import favicon from '../../../../../../shared/layouts/images/favicon.png';
/* eslint-enable no-unused-vars */

function OrderPage(props) {
  return (
    <div className="home-page">
      <Header {...props.header}></Header>
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
