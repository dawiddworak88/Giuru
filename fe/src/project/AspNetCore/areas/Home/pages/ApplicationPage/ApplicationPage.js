import React from "react";
import { ToastContainer } from "react-toastify";
import { ThemeProvider } from "@mui/material/styles";
import GlobalHelper from "../../../../../../shared/helpers/globals/GlobalHelper";
import Footer from "../../../../../../shared/components/Footer/Footer";
import Store from "../../../../../../shared/stores/Store";
import ApplicationForm from "../../components/ApplicationForm/ApplicationForm";

const ApplicationPage = (props) => {
    
    return (
        <ThemeProvider theme={GlobalHelper.initMuiTheme()}>
            <ToastContainer />
            <Store>
                <ApplicationForm {...props.applicationForm} />
                <Footer {...props.footer}></Footer>
            </Store>
        </ThemeProvider>
    );
}

export default ApplicationPage;
