import React from "react";
import { ToastContainer } from "react-toastify";
import { ThemeProvider } from "@mui/material/styles";
import GlobalHelper from "../../../../shared/helpers/globals/GlobalHelper";
import Store from "../../../../shared/stores/Store";
import Header from "../../../../shared/components/Header/Header";
import MainNavigation from "../../../../shared/components/MainNavigation/MainNavigation";
import Footer from "../../../../shared/components/Footer/Footer";
import LocaleHelper from "../../../../shared/helpers/globals/LocaleHelper";
import Catalog from "../../../../shared/components/OrderCatalog/Catalog";
import NotificationBar from "../../../../shared/components/NotificationBar/NotificationBar";

const ListOrdersPage = (props) => {

    LocaleHelper.setMomentLocale(props.locale);

    return (
        <ThemeProvider theme={GlobalHelper.initMuiTheme(props.locale)}>
            <ToastContainer />
            <Store>
                {props.notificationBar && props.notificationBar.items &&
                    <NotificationBar {...props.notificationBar}></NotificationBar>
                }
                <Header {...props.header}></Header>
                <MainNavigation {...props.mainNavigation}></MainNavigation>
                <Catalog {...props.catalog}/>
                <Footer {...props.footer}></Footer>
            </Store>
        </ThemeProvider>
    );
};

export default ListOrdersPage;
