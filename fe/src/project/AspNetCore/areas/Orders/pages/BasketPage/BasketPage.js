import React from "react";
import { ToastContainer } from "react-toastify";
import { ThemeProvider } from "@mui/material/styles";
import GlobalHelper from "../../../../../../shared/helpers/globals/GlobalHelper";
import Store from "../../../../../../shared/stores/Store";
import Header from "../../../../shared/components/Header/Header";
import MainNavigation from "../../../../shared/components/MainNavigation/MainNavigation";
import Footer from "../../../../../../shared/components/Footer/Footer";
import BasketForm from "../../components/BasketForm/BasketForm";
import LocaleHelper from "../../../../../../shared/helpers/globals/LocaleHelper";

const BasketPage = (props) => {

    LocaleHelper.setMomentLocale(props.locale);

    return (
        <ThemeProvider theme={GlobalHelper.initMuiTheme(props.locale)}>
            <ToastContainer />
            <Store>
                <Header {...props.header}></Header>
                <MainNavigation {...props.mainNavigation}></MainNavigation>
                <BasketForm {...props.basketForm} />
                <Footer {...props.footer}></Footer>
            </Store>
        </ThemeProvider>
    );
};

export default BasketPage;
