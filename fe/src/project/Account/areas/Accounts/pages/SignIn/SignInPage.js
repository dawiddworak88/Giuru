import React from "react";
import { ThemeProvider } from "@material-ui/core/styles";
import GlobalHelper from "../../../../../../shared/helpers/globals/GlobalHelper";
import Header from "../../../../../../shared/components/Header/Header";
import Footer from "../../../../../../shared/components/Footer/Footer";
import SignInForm from "../../components/SignIn/SignInForm";
import { toast } from "react-toastify";

function SignInPage(props) {

    toast.configure();

    return (
        <ThemeProvider theme={GlobalHelper.initMuiTheme()}>
            <div>
                <Header {...props.header}></Header>
                <SignInForm {...props.signInForm} />
                <Footer {...props.footer}></Footer>
            </div>
        </ThemeProvider>
    );
}

export default SignInPage;
