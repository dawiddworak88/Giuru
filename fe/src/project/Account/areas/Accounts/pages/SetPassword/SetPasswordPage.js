import React from "react";
import { toast } from "react-toastify";
import { ThemeProvider } from "@mui/styles";
import GlobalHelper from "../../../../../../shared/helpers/globals/GlobalHelper";
import Header from "../../../../../../shared/components/Header/Header";
import Footer from "../../../../../../shared/components/Footer/Footer";
import Store from "../../../../../../shared/stores/Store";
import SetPasswordForm from "../../components/SetPassword/SetPasswordForm";

function SetPasswordPage(props) {
    
    toast.configure();

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
