import React from "react";
import { ThemeProvider } from "@mui/material/styles";
import GlobalHelper from "../../../../shared/helpers/globals/GlobalHelper";
import Header from "../../../../shared/components/Header/Header";
import Footer from "../../../../shared/components/Footer/Footer";
import SignInForm from "../../components/SignIn/SignInForm";
import { ToastContainer } from "react-toastify";

function SignInPage(props) {
    return (
        <ThemeProvider theme={GlobalHelper.initMuiTheme()}>
            <ToastContainer />
            <Header {...props.header}></Header>
            <SignInForm {...props.signInForm} />
            <Footer {...props.footer}></Footer>
        </ThemeProvider>
    );
}

export default SignInPage;