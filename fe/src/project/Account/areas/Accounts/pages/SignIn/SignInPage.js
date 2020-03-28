import React from 'react';
import Header from '../../../../../../shared/components/Header/Header';
import Footer from '../../../../../../shared/components/Footer/Footer';
import SignInForm from '../../components/SignIn/SignInForm';

/* eslint-disable no-unused-vars */
import favicon from '../../../../../../shared/layouts/images/favicon.png';
/* eslint-enable no-unused-vars */

function SignInPage(props) {
  return (
    <div>
      <Header {...props.header}></Header>
      <section className="section is-flex-centered">
        <div className="account-card">
          <SignInForm {...props.signInForm} />
        </div>
      </section>
      <Footer {...props.footer}></Footer>
    </div>
  );
}

export default SignInPage;
