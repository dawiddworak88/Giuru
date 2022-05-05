import React from "react";
import { toast } from "react-toastify";
import { ThemeProvider } from "@mui/styles";
import GlobalHelper from "../../../../../../shared/helpers/globals/GlobalHelper";
import Store from "../../../../../../shared/stores/Store";
import Header from "../../../../shared/components/Header/Header";
import MainNavigation from "../../../../shared/components/MainNavigation/MainNavigation";
import Footer from "../../../../../../shared/components/Footer/Footer";
import NewOrderForm from "../../components/NewOrderForm/NewOrderForm";
import LocaleHelper from "../../../../../../shared/helpers/globals/LocaleHelper";

const NewOrderPage = (props) => {

    toast.configure();
    LocaleHelper.setMomentLocale(props.locale);

    return (
        <ThemeProvider theme={GlobalHelper.initMuiTheme(props.locale)}>
            <Store>
                <Header {...props.header}></Header>
                <MainNavigation {...props.mainNavigation}></MainNavigation>
                <NewOrderForm {...props.newOrderForm} />
                <Footer {...props.footer}></Footer>
            </Store>
        </ThemeProvider>
    );
};

export default NewOrderPage;
