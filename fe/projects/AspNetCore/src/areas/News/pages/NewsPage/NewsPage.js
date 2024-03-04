import React from "react";
import { ToastContainer } from "react-toastify";
import { ThemeProvider } from "@mui/material/styles";
import GlobalHelper from "../../../../shared/helpers/globals/GlobalHelper";
import LocaleHelper from "../../../../shared/helpers/globals/LocaleHelper";
import Store from "../../../../shared/stores/Store";
import Header from "../../../../shared/components/Header/Header";
import MainNavigation from "../../../../shared/components/MainNavigation/MainNavigation";
import Footer from "../../../../shared/components/Footer/Footer";
import NewsCatalog from "../../components/NewsCatalog/NewsCatalog";
import NotificationBar from "../../../../shared/components/NotificationBar/NotificationBar";

const NewsPage = (props) => {
    
    LocaleHelper.setMomentLocale(props.locale);

    return (
        <ThemeProvider theme={GlobalHelper.initMuiTheme()}>
            <ToastContainer />
            <Store>
                {props.notificationBar && props.notificationBar.items &&
                    <NotificationBar {...props.notificationBar}></NotificationBar>
                }
                <Header {...props.header}></Header>
                <MainNavigation {...props.mainNavigation} />
                <NewsCatalog {...props.newsCatalog} />
                <Footer {...props.footer}></Footer>
            </Store>
        </ThemeProvider>
    )
}

export default NewsPage;