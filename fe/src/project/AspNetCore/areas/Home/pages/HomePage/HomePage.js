import React from 'react';
import { Camera } from 'react-feather';
import favicon from '../../../../../../shared/layouts/images/favicon.png';
import Header from '../../../../shared/components/Header/Header';

function HomePage(props) {
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
        <p>
          <Camera />
        </p>
      </section>
    </div>
  );
}

export default HomePage;
