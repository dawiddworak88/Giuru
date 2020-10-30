import React from "react";
import { ThemeProvider } from "@material-ui/core/styles";
import GlobalHelper from "../../../../../../shared/helpers/globals/GlobalHelper";
import Header from "../../../../../../shared/components/Header/Header";
import Footer from "../../../../../../shared/components/Footer/Footer";
import SignInForm from "../../components/SignIn/SignInForm";

function SignInPage(props) {
  return (
    <ThemeProvider theme={GlobalHelper.initMuiTheme()}>
      <div>
        <Header {...props.header}></Header>
        <section className="section is-flex-centered">
          <div className="account-card">
            <SignInForm {...props.signInForm} />
          </div>
        </section>
        <Footer {...props.footer}></Footer>
      </div>
    </ThemeProvider>
    
  );
}

export default SignInPage;
