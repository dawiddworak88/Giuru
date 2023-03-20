import React from "react";
import { ToastContainer } from "react-toastify";
import { ThemeProvider } from "@mui/material/styles";
import LocaleHelper from "../.../.../../../../shared/helpers/globals/LocaleHelper";
import Store from "../../../../../../shared/stores/Store";
import Header from "../../../../shared/components/Header/Header";
import MainNavigation from "../../../../shared/components/MainNavigation/MainNavigation";
import Footer from "../../../../../../shared/components/Footer/Footer";
import GlobalHelper from "../../../../../../shared/helpers/globals/GlobalHelper";
import OrdersAnalyticsDetail from "../../../../shared/components/OrdersAnalytics/OrdersAnalyticsDetail";

const DashboardPage = (props) => {
    
    LocaleHelper.setMomentLocale(props.locale);
    
    return (
        <ThemeProvider theme={GlobalHelper.initMuiTheme(props.locale)}>
            <ToastContainer />
            <Store>
                <Header {...props.header}/>
                <MainNavigation {...props.mainNavigation} />
                <OrdersAnalyticsDetail {...props.ordersAnalyticsDetail }/>
                <Footer {...props.footer}/>
            </Store>
        </ThemeProvider>
    )
}

export default DashboardPage;