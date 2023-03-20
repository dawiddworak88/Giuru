import React from "react";
import { ToastContainer } from "react-toastify";
import { ThemeProvider } from "@mui/material/styles";
import GlobalHelper from "../../../../shared/helpers/globals/GlobalHelper";
import Header from "../../../../shared/components/Header/Header";
import Footer from "../../../../shared/components/Footer/Footer";
import Store from "../../../../shared/stores/Store";
import RegisterForm from "../../components/RegisterForm/RegisterForm";

const RegisterPage = (props) => {
    
    return (
        <ThemeProvider theme={GlobalHelper.initMuiTheme()}>
            <ToastContainer />
            <Store>
                <Header {...props.header}></Header>
                <RegisterForm {...props.registerForm} />
                <Footer {...props.footer}></Footer>
            </Store>
        </ThemeProvider>
    );
}

export default RegisterPage;
