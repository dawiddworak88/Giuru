import React from 'react';
import Header from '../../../../shared/components/Header/Header';

function HomePage(props) {
  return (
    <div className="home-page">
      <Header {...props.header}></Header>
      <p>
        {props.welcome}
      </p>
      <p>
        {props.learnMore}
      </p>
    </div>
  );
}

export default HomePage;
