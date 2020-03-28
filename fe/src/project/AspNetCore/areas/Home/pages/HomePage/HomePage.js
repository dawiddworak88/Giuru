import React from 'react';
import Header from '../../../../../../shared/components/Header/Header';
import Footer from '../../../../../../shared/components/Footer/Footer';

/* eslint-disable no-unused-vars */
import favicon from '../../../../../../shared/layouts/images/favicon.png';
/* eslint-enable no-unused-vars */

function HomePage(props) {
  return (
    <div className="home-page">
      <Header {...props.header}></Header>
      <Footer {...props.footer}></Footer>
    </div>
  );
}

export default HomePage;
