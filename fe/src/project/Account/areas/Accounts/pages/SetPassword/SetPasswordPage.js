import React from "react";
import { ThemeProvider } from "@material-ui/core/styles";
import GlobalHelper from "../../../../../../shared/helpers/globals/GlobalHelper";
import Header from "../../../../../../shared/components/Header/Header";
import Footer from "../../../../../../shared/components/Footer/Footer";
import Store from "../../../../../../shared/stores/Store";
import SetPasswordForm from "../../components/SetPassword/SetPasswordForm";

function SetPasswordPage(props) {
    return (
        <ThemeProvider theme={GlobalHelper.initMuiTheme()}>
            <Store>
                <Header {...props.header}></Header>
                <SetPasswordForm {...props.setPasswordForm} />
                <Footer {...props.footer}></Footer>
            </Store>
        </ThemeProvider>
    );
}

export default SetPasswordPage;
